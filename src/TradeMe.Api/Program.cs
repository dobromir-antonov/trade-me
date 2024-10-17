using TradeMe.Api;
using TradeMe.Api.DependencyInjection;
using SharedKernel.Infrastructure;
using TradeMe.Api.OpenApi;

List<IModule> modules = ModuleHelper.GetModules(AppDomain.CurrentDomain.BaseDirectory);

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

    foreach (var module in modules)
    {
        module.AddModule(builder.Services, builder.Configuration);
    }

    builder.Services
        .AddGlobalErrorHandling()
        .AddSwaggerPackage()
        .AddMessageBroker(modules);

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






