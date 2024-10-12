using Lumina.Api;
using Lumina.Api.DependencyInjection;
using Lumina.Api.OpenApi;
using SharedKernel.Infrastructure;

List<IModule> modules = ModuleHelper.GetModules(AppDomain.CurrentDomain.BaseDirectory);

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
{
    builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
    builder.Host.UseDefaultServiceProvider(config =>
    {
        config.ValidateOnBuild = true;
    });

    foreach (var module in modules)
    {
        module.AddModule(builder.Services, builder.Configuration);
    }

    builder.Services
        .AddGlobalErrorHandling()
        .AddSwaggerPackage()
        .AddMassTransitDependency(modules);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                //builder.WithOrigins("http://localhost:4200")
                //       .AllowAnyHeader()
                //       .AllowAnyMethod();
            });
    });
}


var app = builder.Build();

app.MapDefaultEndpoints();
{
    app.UseGlobalErrorHandling();

    foreach (var module in modules)
    {
        module.UseModule(app);
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerPackage();
    }

    app.UseCors("AllowSpecificOrigin");
}


app.Run();






