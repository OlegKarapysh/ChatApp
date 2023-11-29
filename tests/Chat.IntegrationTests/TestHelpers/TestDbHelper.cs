namespace Chat.IntegrationTests.TestHelpers;

internal sealed class TestDbHelper
{
    private readonly TestApplicationFactory _testAppFactory;

    internal TestDbHelper(TestApplicationFactory testAppFactory)
    {
        _testAppFactory = testAppFactory;
    }

    internal List<int> GetAllUserConversationIds(int userId)
    {
        return GetFromDb(x => x.ConversationParticipants.AsNoTracking()
                               .Where(c => c.UserId == userId)
                               .Select(c => c.ConversationId).ToList());
    }

    internal int CountUsers()
    {
        return GetFromDb(x => x.Users.AsNoTracking().Count());
    }

    internal User? GetUserByEmail(string email)
    {
        return GetFromDb(x => x.Users.AsNoTracking().FirstOrDefault(u => u.Email == email));
    }

    internal User? GetUserById(int id)
    {
        return GetFromDb(x => x.Users.AsNoTracking().FirstOrDefault(u => u.Id == id));
    }
    
    internal User? GetFirstUser()
    {
        return GetFromDb(x => x.Users.AsNoTracking().FirstOrDefault());
    }
    
    internal List<User> GetAllUsers()
    {
        return GetFromDb(x => x.Users.AsNoTracking().ToList());
    }
    
    internal T GetFromDb<T>(Func<ChatDbContext, T> getFunc)
    {
        using var scope = _testAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        return getFunc.Invoke(dbContext);
    }
}