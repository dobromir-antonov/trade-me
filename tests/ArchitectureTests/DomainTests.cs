using FluentAssertions;
using NetArchTest.Rules;

using SharedKernel.Domain;
using System.Collections;
using System.Reflection;

namespace ArchitectureTests;

public class DomainTests : BaseTest
{
    [Fact]
    public void Entities_Should_HavePrivateSettersOnly()
    {
        var entityTypes = Types
            .InAssemblies(DomainAssemblies)
            .That()
            .Inherit(typeof(Entity<>))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach (var entityType in entityTypes)
        {
            var hasPublicSetter = entityType
                .GetProperties()
                .Any(p => !p.PropertyType.IsAssignableTo(typeof(IEnumerable)) && p.SetMethod is null or { IsPublic: true });

            if (hasPublicSetter)
            {
                failingTypes.Add(entityType);
            }
        }

        string because = string.Join(",", failingTypes.Select(t => t.Name));

        failingTypes
            .Should()
            .BeEmpty("should not have properties with public setters");
    }

    [Fact]
    public void Entities_Should_HaveParameterlessConstructor()
    {
        var entityTypes = Types
            .InAssemblies(DomainAssemblies)
            .That()
            .Inherit(typeof(Entity<>))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach (var entityType in entityTypes)
        {
            var hasParameterlessConstructor = entityType
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .Any(c => c.IsPrivate && c.GetParameters().Length == 0);

            if (!hasParameterlessConstructor)
            {
                failingTypes.Add(entityType);
            }
        }

        string because = string.Join(",", failingTypes.Select(t => t.Name));

        failingTypes
            .Should()
            .BeEmpty("should have parameterless constructor");
    }

    [Fact]
    public void ValueObjects_Should_BeSealed()
    {
        var valueObjects = Types
            .InAssemblies(DomainAssemblies)
            .That()
            .Inherit(typeof(ValueObject));

        var result = valueObjects
            .Should()
            .BeSealed()
            .GetResult();

        result.FailingTypeNames
            .Should()
            .BeNullOrEmpty("all value objects should be sealed");
    }
}
