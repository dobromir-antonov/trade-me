using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class LayerTests : BaseTest
{
    [Fact]
    public void Domain_Should_Not_DependOnApplication()
    {
        var domain = Types.InAssemblies(DomainAssemblies);

        var application = ApplicationAssemblies
            .Select(a => a.FullName)
            .ToArray();

        var rule = domain
            .Should()
            .NotHaveDependencyOnAny(application);

        var result = rule.GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Domain_Should_Not_DependOnInfrastructure()
    {
        var domain = Types.InAssemblies(DomainAssemblies);

        var infrastructre = InfrastructureAssemblies
            .Select(a => a.FullName)
            .ToArray();

        var rule = domain
            .Should()
            .NotHaveDependencyOnAny(infrastructre);

        var result = rule.GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Application_Should_Not_DependOnInfrastructure()
    {
        var application = Types.InAssemblies(ApplicationAssemblies);

        var infrastructre = InfrastructureAssemblies
            .Select(a => a.FullName)
            .ToArray();

        var rule = application
            .Should()
            .NotHaveDependencyOnAny(infrastructre);

        var result = rule.GetResult();

        result.IsSuccessful
            .Should()
            .BeTrue();
    }
}
