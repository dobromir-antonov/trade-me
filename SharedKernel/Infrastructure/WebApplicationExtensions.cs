using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application;
using System.Reflection;

namespace SharedKernel.Infrastructure;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder MapEndpoints(this WebApplication app, Assembly assembly, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEndpointRouteBuilder builder = routeGroupBuilder is null 
            ? app 
            : routeGroupBuilder;

        IEndpoint[] endpoints = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .Where(type => type.IsAssignableTo(typeof(IEndpoint)))
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>()
            .ToArray();

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}
