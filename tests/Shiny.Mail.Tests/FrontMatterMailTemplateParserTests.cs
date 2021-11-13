using System;
using System.Threading.Tasks;
using FluentAssertions;
using Shiny.Mail.Impl;
using Xunit;


namespace Shiny.Mail.Tests
{
    public class FrontMatterMailTemplateParserTests
    {
        readonly FrontMatterMailTemplateParser parser = new FrontMatterMailTemplateParser();


        [Fact]
        public async Task Variables_Invalid()
        {
            try
            {
                await this.parser.Parse(@"
to: hello@hello.com
test: fail!
---
test
");
                //Assert.Fail("");
            }
            catch (ArgumentException ex)
            {
                ex.Message.Should().Contain("'test'");
                ex.Message.Should().Contain("line 2");
            }
        }


        [Fact]
        public async Task Variables_IgnoresUnset()
        {

        }


        [Fact]
        public async Task Variables_EmailDisplayName()
        {

        }

        [Fact]
        public async Task MissingFrontMatter()
        {

        }


        [Fact]
        public async Task Variables_All()
        {

        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Body_IsHtml(bool expected)
        {

        }
    }
}