namespace Chat.IntegrationTests.TestHelpers;

internal sealed class TestDbHelper
{
    private readonly TestApplicationFactory _testAppFactory;

    internal TestDbHelper(TestApplicationFactory testAppFactory)
    {
        _testAppFactory = testAppFactory;
    }

    internal Message? GetMessageById(int messageId)
    {
        return GetFromDb(x => x.Messages.Find(messageId));
    }

    internal int CountUsers()
    {
        return GetFromDb(x => x.Users.AsNoTracking().Count());
    }

    internal User? GetUserByEmail(string email)
    {
        return GetFromDb(x => x.Users.AsNoTracking().FirstOrDefault(u => u.Email == email));
    }
    
    internal T GetFromDb<T>(Func<ChatDbContext, T> getFunc)
    {
        using var scope = _testAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        return getFunc.Invoke(dbContext);
    }
}