namespace Shiny.Extensions.Webhooks.Tests;

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Shiny.Extensions.Webhooks.Infrastructure;
using Xunit;

public class DbRepositoryTests : IDisposable
{
	readonly DbRepository<SqliteConnection> repository = new DbRepository<SqliteConnection>(new DbRepositoryConfig(
		"Data Source=hello.db"
	));


    [Fact]
    public async Task SubscribeTest()
    {
        //this.repository.Subscribe
    }


    public void Dispose()
    {
        File.Delete("hello.db");
    }
}