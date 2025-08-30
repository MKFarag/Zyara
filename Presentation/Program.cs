#region Usings

using Presentation;
using Serilog;
using Scalar.AspNetCore;
using Hangfire;
using HangfireBasicAuthenticationFilter;

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();

    app.MapOpenApi();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization =
    [
        new HangfireCustomBasicAuthenticationFilter
         {
             User = app.Configuration.GetValue<string>("Hangfire:Username"),
             Pass = app.Configuration.GetValue<string>("Hangfire:Password")
         }
    ],
    DashboardTitle = "Zyara Hangfire Dashboard",
    IsReadOnlyFunc = context => true
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
