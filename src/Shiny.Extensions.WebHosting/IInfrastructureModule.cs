using Microsoft.AspNetCore.Builder;

namespace Shiny;


public interface IInfrastructureModule
{
    void Add(WebApplicationBuilder builder);
    void Use(WebApplication app);
}