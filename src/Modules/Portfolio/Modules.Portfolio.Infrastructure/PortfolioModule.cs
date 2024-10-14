using Asp.Versioning;
using Asp.Versioning.Builder;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Portfolio.Domain;
using Modules.Portfolio.Domain.UserPortfolios.Abstraction;
using Modules.Portfolio.Infrastructure.Persistance;
using Modules.Portfolio.Infrastructure.Persistance.Repositories;
using SharedKernel.Infrastructure;
using System.Reflection;

namespace Modules.Portfolio.Infrastructure;

public class PortfolioModule : IModule
{
    public Assembly GetApplicationAssembly() => Application.AssemblyReference.Assembly;

    public Assembly GetDomainAssembly() => Domain.AssemblyReference.Assembly;

    public Assembly GetInfrasturctureAssembly() => Infrastructure.AssemblyReference.Assembly;


    public void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        RegisterMediator(services);
        RegisterEndpointVersioning(services);
        RegisterPersistance(services, configuration);
        RegisterIntegrationEventHandlers(services);
    }

    public void UseModule(WebApplication app)
    {
        MapEndpoints(app);
    }

    public void ConfigureMassTransit(IBusRegistrationConfigurator bus)
    {
    }


    private static void RegisterMediator(IServiceCollection services)
    {
        services.AddMediatR(options => options.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly));
    }

    private static void RegisterEndpointVersioning(IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

    }

    private static void RegisterPersistance(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PortfolioDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("Portfolio")));

        services.AddScoped<IUnitOfWork, PortfolioDbContext>();

        services.AddScoped<IUserPortfolioRepository, UserPortfolioRepository>();
    }

    private static void RegisterIntegrationEventHandlers(IServiceCollection services)
    {

    }

    private static void MapEndpoints(WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder routeGroup = app
            .MapGroup("api/v{version:apiVersion}/portfolio")
            .WithApiVersionSet(apiVersionSet);

        app.MapEndpoints(Application.AssemblyReference.Assembly, routeGroup);
    }
}
