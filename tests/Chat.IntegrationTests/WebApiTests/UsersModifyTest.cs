namespace Chat.IntegrationTests.WebApiTests;

[Collection("Sequential")]
public class UsersModifyTest : IClassFixture<IntegrationTest>
{
    private readonly IntegrationTest _test;
    
    public UsersModifyTest(IntegrationTest test)
    {
        _test = test;
    }

    [Fact]
    public async Task Update_UpdatesUser()
    {
        // Arrange.
        
        // Act.

        // Assert.
    }
    
}