using FluentAssertions;
using NetArchTest.Rules;
using SharedKernel.Application;
using SharedKernel.Application.Abstraction.Data;
using SharedKernel.Messaging;
using System.Reflection;

namespace ArchitectureTests;

public class ApplicationTests : BaseTest
{
    [Fact]
    public void Endpoints_Should_BeSealedAndNonPublic()
    {
        var endpoints = Types
            .InAssemblies(ApplicationAssemblies)
            .That()
            .ImplementInterface(typeof(IEndpoint));

        var rule = endpoints
            .Should()
            .BeSealed()
            .And()
            .NotBePublic();

        var result = rule.GetResult();  

        result.FailingTypeNames
            .Should()
            .BeNullOrEmpty("should be sealed and non public");
    }

    [Fact]
    public void QueryHandlers_Should_Not_UseUnitOfWork()
    {
        var queryHandlerTypes = Types
            .InAssemblies(ApplicationAssemblies)
            .That()
            .ImplementInterface(typeof(IQueryHander<,>))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach (var queryHandlerType in queryHandlerTypes)
        {
            var injectUnitOfWork = queryHandlerType
               .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
               .SelectMany(c => c.GetParameters())
               .Any(p => p.ParameterType.IsAssignableTo(typeof(IUnitOfWork)));

            var hasUnitOfWorkField = queryHandlerType
               .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
               .Any(f => f.FieldType.IsAssignableTo(typeof(IUnitOfWork)));

            if (injectUnitOfWork || hasUnitOfWorkField)
            {
                failingTypes.Add(queryHandlerType);
            }
        }

        string because = string.Join(",", failingTypes.Select(t => t.Name));

        failingTypes
            .Should()
            .BeEmpty("queries should not mutate the state");
    }
}
