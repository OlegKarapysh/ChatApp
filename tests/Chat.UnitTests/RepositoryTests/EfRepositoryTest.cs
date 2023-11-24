using System.Data.Common;
using Chat.Domain.Entities.Conversations;
using Chat.Persistence.Contexts;
using Chat.Persistence.Repositories;
using Chat.UnitTests.TestHelpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chat.UnitTests.RepositoryTests;

public sealed class EfRepositoryTest : IDisposable
{
    private EfRepository<Conversation, int>? _sut;
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ChatDbContext> _dbOptions;

    public EfRepositoryTest()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _dbOptions = new DbContextOptionsBuilder<ChatDbContext>().UseSqlite(_connection).Options;
        DbSeedHelper.RecreateAndSeedDb(CreateDbContext());
    }

    public ChatDbContext CreateDbContext() => new(_dbOptions);
    
    public void Dispose() => _connection.Dispose();
    
    [Fact]
    public async Task AddAsync_AddsEntityAndReturnsIt()
    {
        // Arrange.
        var context = CreateDbContext();
        _sut = new EfRepository<Conversation, int>(context);
        var expectedCount = (await _sut.GetAllAsync()).Count + 1;
        var conversation = new Conversation { Title = "title1", Type = ConversationType.Dialog };
        
        // Act.
        var result = await _sut.AddAsync(conversation);
        context.SaveChanges();
        var resultCount = (await _sut.GetAllAsync()).Count;
        
        // Assert.
        result.Title.Should()!.Be(conversation.Title);
        result.Type.Should()!.Be(conversation.Type);
        resultCount.Should()!.Be(expectedCount);
    }
}