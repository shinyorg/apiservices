using FluentAssertions;
using Shiny.Extensions.Localization.Impl;
using Xunit;


namespace Shiny.Extensions.Localization.Tests
{
    public class ResxLocalizationProviderTests
    {
        const string ResourcePath = "Shiny.Extensions.Localization.Tests.Resources.";


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Assembly_NoTrim_Names(bool trim)
        {
            var provider = new AssemblyResxLocalizationProvider(this.GetType().Assembly, trim, true);
            var sources = provider.Load();
            sources.Length.Should().Be(2);

            var path1 = "Strings1";
            var path2 = "Strings2";
            if (!trim)
            {
                path1 = ResourcePath + path1;
                path2 = ResourcePath + path2;
            }

            sources[0].Name.Should().Be(path1);
            sources[1].Name.Should().Be(path2);
        }


        [Fact]
        public void CaseInsensitive()
        {
            var cfg = new LocalizationBuilder()
                .AddAssemblyResources(this.GetType().Assembly, true, true)
                .Build();

            cfg["strings1:helloworld"].Should().Be("Hello World");
        }
    }
}
