// using Microsoft.AspNetCore.Builder;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
//
// namespace BuildingOps.Modules;
//
//
// public class SwaggerModule : IInfrastructureModule
// {
//     public void Add(WebApplicationBuilder builder)
//     {
//         builder.Services.AddEndpointsApiExplorer();
//         builder.Services.AddSwaggerGen(x =>
//         {
//             // x.CustomOperationIds(y =>
//             // {
//             // });
//             x.CustomSchemaIds(y =>
//             {
//                 var typeName = y.Name.Replace("`1", String.Empty);
//                 var prefix = y.Namespace!.Substring(y.Namespace!.LastIndexOf(".") + 1);
//                 if (prefix.Equals("Contracts"))
//                     return typeName;
//
//                 var schemaId = $"{prefix}{typeName}";
//                 return schemaId;
//             });
//         });
//     }
//
//     public void Use(WebApplication app)
//     {
//         if (app.Environment.IsDevelopment())
//         {
//             app.UseSwagger();
//             app.UseSwaggerUI();
//         }
//     }
// }