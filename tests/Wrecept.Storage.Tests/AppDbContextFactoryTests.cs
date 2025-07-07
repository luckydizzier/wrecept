using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Tests;

public class AppDbContextFactoryTests
{
    [Fact]
    public void CreateDbContext_WithoutArgs_UsesAppDb()
    {
        var factory = new AppDbContextFactory();
        using var context = factory.CreateDbContext(Array.Empty<string>());

        Assert.Equal("app.db", context.Database.GetDbConnection().DataSource);
    }

    [Fact]
    public void CreateDbContext_WithArg_UsesGivenFile()
    {
        var factory = new AppDbContextFactory();
        using var context = factory.CreateDbContext(new[] { "test.db" });

        Assert.Equal("test.db", context.Database.GetDbConnection().DataSource);
    }
}
