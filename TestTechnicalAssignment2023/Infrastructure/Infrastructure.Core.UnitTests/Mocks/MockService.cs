using Infrastructure.Core.Services;
using Infrastructure.Core.Services.Interfaces;

namespace Infrastructure.Core.UnitTests.Mocks;

public class MockService : TransactionService<MockDbContext>
{
    public MockService(
        IDbContextWrapper<MockDbContext> dbContextWrapper,
        ILogger<MockService> logger)
        : base(dbContextWrapper, logger)
    {
    }

    public async Task RunWithException()
    {
        await ExecuteSafeAsync<bool>(() => throw new Exception());
    }

    public async Task RunWithoutException()
    {
        await ExecuteSafeAsync<bool>(() => Task.FromResult(true));
    }
}