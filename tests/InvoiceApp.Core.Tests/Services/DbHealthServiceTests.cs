using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using InvoiceApp.Data.Data;
using InvoiceApp.Data.Services;
using InvoiceApp.Core.Services;
using Xunit;

namespace InvoiceApp.Core.Tests.Services;

public class DbHealthServiceTests
{
    private class FailFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext() => throw new InvalidOperationException();
        public ValueTask<AppDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
            => throw new InvalidOperationException();
    }

    private class LogSpy : ILogService
    {
        public Exception? Last;
        public Task LogError(string message, Exception ex)
        {
            Last = ex;
            return Task.CompletedTask;
        }
    }

    private class FailResultFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext() => new FakeContext();
        public ValueTask<AppDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
            => ValueTask.FromResult<AppDbContext>(new FakeContext());

        private class FakeContext : AppDbContext
        {
            private readonly DatabaseFacade _facade;
            public FakeContext() : base(new DbContextOptions<AppDbContext>())
            {
                _facade = new FakeFacade(this);
            }

            public override DatabaseFacade Database => _facade;
        }

        private class FakeFacade : DatabaseFacade
        {
            public FakeFacade(DbContext context) : base(context) { }

            public DbConnection GetDbConnection() => new FakeConnection();
            public Task OpenConnectionAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public Task CloseConnectionAsync() => Task.CompletedTask;
        }

        private class FakeConnection : DbConnection
        {
#pragma warning disable CS8765
            public override string ConnectionString { get; set; } = string.Empty;
            public override string Database => string.Empty;
            public override string DataSource => string.Empty;
            public override string ServerVersion => string.Empty;
            public override ConnectionState State => ConnectionState.Open;
            public override void ChangeDatabase(string databaseName) { }
            public override void Close() { }
            public override void Open() { }
            public override Task OpenAsync(CancellationToken cancellationToken) => Task.CompletedTask;
            protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => throw new NotImplementedException();
            protected override DbCommand CreateDbCommand() => new FakeCommand();
#pragma warning restore CS8765
        }

        private class FakeCommand : DbCommand
        {
#pragma warning disable CS8765
            public override string CommandText { get; set; } = string.Empty;
            public override int CommandTimeout { get; set; }
            public override CommandType CommandType { get; set; }
            protected override DbConnection DbConnection { get; set; } = new FakeConnection();
            protected override DbParameterCollection DbParameterCollection { get; } = new FakeParameterCollection();
            protected override DbTransaction DbTransaction { get; set; } = null!;
            public override bool DesignTimeVisible { get; set; }
            public override UpdateRowSource UpdatedRowSource { get; set; }
            public override void Cancel() { }
            public override int ExecuteNonQuery() => 0;
            public override object ExecuteScalar() => "failed";
            public override Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken) => Task.FromResult<object?>("failed");
            protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => throw new NotImplementedException();
            public override void Prepare() { }
            protected override DbParameter CreateDbParameter() => new FakeParameter();
#pragma warning restore CS8765
        }

        private class FakeParameter : DbParameter
        {
#pragma warning disable CS8765
            public override DbType DbType { get; set; }
            public override ParameterDirection Direction { get; set; }
            public override bool IsNullable { get; set; }
            public override string ParameterName { get; set; } = string.Empty;
            public override string SourceColumn { get; set; } = string.Empty;
            public override object? Value { get; set; }
            public override void ResetDbType() { }
            public override int Size { get; set; }
            public override bool SourceColumnNullMapping { get; set; }
#pragma warning restore CS8765
        }

        private class FakeParameterCollection : DbParameterCollection
        {
#pragma warning disable CS8765 // Nullability of parameter doesn't match base member
            public override int Count => 0;
            public override object SyncRoot { get; } = new();
            public override int Add(object value) => 0;
            public override void AddRange(Array values) { }
            public override void Clear() { }
            public override bool Contains(object value) => false;
            public override bool Contains(string value) => false;
            public override void CopyTo(Array array, int index) { }
            public override IEnumerator GetEnumerator() => Array.Empty<object>().GetEnumerator();
            public override int IndexOf(object value) => -1;
            public override int IndexOf(string parameterName) => -1;
            public override void Insert(int index, object value) { }
            public override bool IsFixedSize => false;
            public override bool IsReadOnly => false;
            public override bool IsSynchronized => false;
            public override void Remove(object value) { }
            public override void RemoveAt(int index) { }
            public override void RemoveAt(string parameterName) { }
            protected override DbParameter GetParameter(int index) => throw new NotImplementedException();
            protected override DbParameter GetParameter(string parameterName) => throw new NotImplementedException();
            protected override void SetParameter(int index, DbParameter value) { }
            protected override void SetParameter(string parameterName, DbParameter value) { }
#pragma warning restore CS8765
        }
    }

    [Fact]
    public async Task CheckAsync_ReturnsTrue_ForValidDb()
    {
        var services = new ServiceCollection();
        services.AddDbContextFactory<AppDbContext>(o => o.UseSqlite("Data Source=:memory:"));
        await using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using (var ctx = await factory.CreateDbContextAsync())
        {
            await ctx.Database.EnsureCreatedAsync();
        }
        var svc = new DbHealthService(factory, new NullLogService());
        var ok = await svc.CheckAsync();
        Assert.True(ok);
    }

    [Fact]
    public async Task CheckAsync_LogsAndReturnsFalse_OnException()
    {
        var log = new LogSpy();
        var svc = new DbHealthService(new FailFactory(), log);
        var ok = await svc.CheckAsync();
        Assert.False(ok);
        Assert.NotNull(log.Last);
    }

    [Fact]
    public async Task CheckAsync_LogsAndReturnsFalse_WhenResultNotOk()
    {
        var log = new LogSpy();
        var svc = new DbHealthService(new FailResultFactory(), log);
        var ok = await svc.CheckAsync();
        Assert.False(ok);
        Assert.NotNull(log.Last);
        Assert.Contains("database", log.Last?.Message, StringComparison.OrdinalIgnoreCase);
    }
}
