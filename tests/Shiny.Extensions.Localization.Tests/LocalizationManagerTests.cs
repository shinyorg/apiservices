using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using Xunit;


namespace Shiny.Extensions.Localization.Tests
{
    public class LocalizationManagerTests
    {
        [Fact]
        public void Keys_KeyWithSection()
        {
            var manager = new LocalizationBuilder()
                .AddAssemblyResources(this.GetType().Assembly, true)
                .Build();

            manager["Strings1:HelloWorld"].Should().Be("Hello World");
        }


        [Fact]
        public void Keys_CaseInsensitive()
        {
            var manager = new LocalizationBuilder()
                .AddAssemblyResources(this.GetType().Assembly, true)
                .Build();

            manager["Strings1:HelloWorld"].Should().Be("Hello World");
        }


        [Fact]
        public void DependencyInjection_CategoryType()
        {
            var services = new ServiceCollection();
            services.ConfigureLocalization(x => x
                .AddAssemblyResources(this.GetType().Assembly, true)
            );
            var sp = services.BuildServiceProvider();
            var localize = sp.GetService<ILocalize<Strings1>>();

            localize.Should().NotBeNull("Localize is null");
            localize["HelloWorld"].Should().Be("Hello World");
        }
    }

    public class Strings1 {} 
}
