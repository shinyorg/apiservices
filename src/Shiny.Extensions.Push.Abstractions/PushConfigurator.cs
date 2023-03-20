using Microsoft.Extensions.DependencyInjection;

namespace Shiny.Extensions.Push;


public class PushConfigurator
{
    public PushConfigurator(IServiceCollection services) => this.Services = services;

    public IServiceCollection Services { get; }

    public int MaxParallelization { get; set; } = 3;
}