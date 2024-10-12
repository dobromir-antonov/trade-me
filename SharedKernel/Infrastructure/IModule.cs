using MassTransit;
using MassTransit.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedKernel.Infrastructure;

public interface IModule
{
    void AddModule(IServiceCollection services, IConfiguration configuration);
    void UseModule(WebApplication app);
    void ConfigureMassTransit(IBusRegistrationConfigurator bus);

    Assembly GetDomainAssembly();
    Assembly GetApplicationAssembly();
    Assembly GetInfrasturctureAssembly();
}
