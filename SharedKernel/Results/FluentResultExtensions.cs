using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Http = Microsoft.AspNetCore.Http;

namespace SharedKernel.Results;

public static class FluentResultExtensions
{

    public static Http.IResult ToProblem<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Http.Results.Ok(result.Value);
        }

        return result.Errors.ToProblem();
    }

    public static Http.IResult ToProblem(this List<IError> errors)
    {
        var problem = new ProblemDetails
        {
            Title = "",
            Status = Http.StatusCodes.Status400BadRequest,
            Detail = string.Join(";", errors.Select(e => e.Message)),
        };

        foreach (var error in errors)
        {
            foreach (var metadata in error.Metadata)
            {
                problem.Extensions.Add(metadata);
            }
        }

        return Http.Results.Problem(problem);
    }

    public static TResult Match<TValue, TResult>(this Result<TValue> result, Func<TValue, TResult> success, Func<List<IError>, TResult> failure)
    {
        if (result.IsSuccess)
        {
            return success(result.Value);
        }

        return failure(result.Errors);
    }

    public static TResult Match<TResult>(this Result result, Func<TResult> onSuccess, Func<List<IError>, TResult> onFailure)
    {
        return result.IsSuccess
            ? onSuccess()
            : onFailure(result.Errors);
    }
}
