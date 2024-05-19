using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Shiny.Extensions.EntityFramework.Tests;

public partial class EfTests
{

    [Fact]
    public async Task DidLog()
    {
        await this.DoDb(async data =>
        {
            await data.Manufacturers.ToListAsync();
        });

        await this.DoDb(async data =>
        {
            var logs = await data.QueryLogs.ToListAsync();
            logs.Count.Should().BeGreaterOrEqualTo(1, "No query logs");
        });
    }
}