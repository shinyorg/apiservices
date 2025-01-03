// using BuildingOps.Data;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace BuildingOps.Modules;
//
//
// public class HealthChecksModule : IInfrastructureModule
// {
//     public void Add(WebApplicationBuilder builder)
//     {
//         // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0
//         builder.Services
//             .AddHealthChecks()
//             .AddDbContextCheck<AppDbContext>();
//     }
//
//     
//     public void Use(WebApplication app)
//     {
//         app
//             .MapHealthChecks("/health")
//             .RequireHost("*:5001");
//         // .RequireAuthorization()
//     }
// }