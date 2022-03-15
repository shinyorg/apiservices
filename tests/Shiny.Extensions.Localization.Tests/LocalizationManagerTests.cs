using FluentAssertions;
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
    }
}
