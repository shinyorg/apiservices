using System;
using System.Threading.Tasks;
using FluentAssertions;
using Shiny.Extensions.Mail.Impl;
using Xunit;


namespace Shiny.Extensions.Mail.Tests
{
    public class FrontMatterMailTemplateConverterTests
    {
        readonly FrontMatterMailTemplateConverter parser = new FrontMatterMailTemplateConverter();


        [Fact]
        public async Task Variables_Invalid()
        {
            try
            {
                await this.parser.Convert(@"
to: test@shinylib.net
test: fail!
---
test
");
                //Assert.Fail("");
            }
            catch (ArgumentException ex)
            {
                ex.Message.Should().Contain("'test'");
                ex.Message.Should().Contain("line 3"); // enter @ causes it to be line 3
            }
        }


        [Fact]
        public async Task Variables_IgnoresUnset()
        {
            var mail = await this.parser.Convert(@"
to: 
---
test
");
            mail.To.Count.Should().Be(0);
        }


        [Fact]
        public async Task Variables_EmailDisplayName()
        {
            var mail = await this.parser.Convert(@"
from: Shiny Library <test@shinylib.net>
---
test
");
            mail.From.Address.Should().Be("test@shinylib.net");
            mail.From.DisplayName.Should().Be("Shiny Library");
        }


        [Fact]
        public async Task Variables_MultipleAddress()
        {
            var mail = await this.parser.Convert(@"
to: Shiny Library <test@shinylib.net>; hello@shinylib.net; GitHub <hello@github.com>
---
test
");
            mail.To.Count.Should().Be(3);
            mail.To[0].Address.Should().Be("test@shinylib.net");
            mail.To[1].Address.Should().Be("hello@shinylib.net");
            mail.To[2].Address.Should().Be("hello@github.com");
        }


        [Fact]
        public async Task Missing_FrontMatter()
        {
            var mail = await this.parser.Convert(@"
this is body only
");
        }


        [Fact]
        public async Task Missing_Body()
        {
            var mail = await this.parser.Convert(@"
subject: test
---
");
            // TODO: should throw exception?
        }


        //[Fact(Skip = "TODO")]
        //public async Task Variables_All()
        //{

        //}


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Body_IsHtml(bool expected)
        {
            var result = await this.parser.Convert(@$"
subject: test
html: {expected}
---
The body doesn't matter here
");
            result.IsBodyHtml.Should().Be(expected);
        }
    }
}