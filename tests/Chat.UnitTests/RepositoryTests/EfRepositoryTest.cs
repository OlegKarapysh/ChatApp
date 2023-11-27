namespace Chat.UnitTests.RepositoryTests;

public sealed class EfRepositoryTest : IDisposable
{
    private readonly EfRepository<User, int> _sut;
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ChatDbContext> _dbOptions;
    private readonly ChatDbContext _context;

    public EfRepositoryTest()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _dbOptions = new DbContextOptionsBuilder<ChatDbContext>().UseSqlite(_connection).Options;
        DbSeedHelper.RecreateAndSeedDb(CreateDbContext());
        _context = CreateDbContext();
        _sut = new EfRepository<User, int>(_context);
    }

    public ChatDbContext CreateDbContext() => new(_dbOptions);

    public void Dispose()
    {
        _connection.Dispose();
        _context.Dispose();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsFoundEntity()
    {
        // Arrange.
        var expectedEntity = (await _sut.GetAllAsync()).First();
        
        // Act.
        var result = await _sut.GetByIdAsync(expectedEntity.Id);

        // Assert.
        result!.Should()!.NotBeNull()!.And!.BeEquivalentTo(expectedEntity);
    }
    
    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenEntityNotFound()
    {
        // Arrange.
        const int invalidId = int.MinValue;
        
        // Act.
        var result = await _sut.GetByIdAsync(invalidId);

        // Assert.
        result!.Should()!.BeNull();
    }
    
    [Fact]
    public async Task AddAsync_AddsEntityAndReturnsIt()
    {
        // Arrange.
        var expectedCount = (await _sut.GetAllAsync()).Count + 1;
        var entity = TestDataGenerator.GenerateUser();
        
        // Act.
        var result = await _sut.AddAsync(entity);
        await _context.SaveChangesAsync();
        var resultCount = (await _sut.GetAllAsync()).Count;
        
        // Assert.
        result.Should()!.BeEquivalentTo(entity);
        resultCount.Should()!.Be(expectedCount);
    }

    [Fact]
    public async Task RemoveAsync_RemovesEntityAndReturnsTrue_WhenEntityFound()
    {
        // Arrange.
        var context = CreateDbContext();
        var sut = new EfRepository<ConversationParticipants, int>(context);
        var all = await sut.GetAllAsync();
        var expectedCount = all.Count - 1;
        var existingId = all.First().Id;
        
        // Act.
        var result = await sut.RemoveAsync(existingId);
        await context.SaveChangesAsync();
        var resultCount = (await sut.GetAllAsync()).Count;

        // Assert.
        result.Should()!.BeTrue();
        resultCount.Should()!.Be(expectedCount);
    }
    
    [Fact]
    public async Task RemoveAsync_ReturnsFalse_WhenEntityNotFound()
    {
        // Arrange.
        var context = CreateDbContext();
        var sut = new EfRepository<ConversationParticipants, int>(context);
        const int invalidId = int.MinValue;
        var expectedCount = (await sut.GetAllAsync()).Count;
        
        // Act.
        var result = await sut.RemoveAsync(invalidId);
        await context.SaveChangesAsync();
        var resultCount = (await sut.GetAllAsync()).Count;

        // Assert.
        result.Should()!.BeFalse();
        resultCount.Should()!.Be(expectedCount);
    }

    [Fact]
    public async Task Update_ReturnsUpdatedEntity()
    {
        // Arrange.
        var existingEntity = (await _sut.GetAllAsync()).First();
        existingEntity.FirstName += "updated";
        existingEntity.LastName += "updated";
        existingEntity.UserName += "updated";
        existingEntity.PhoneNumber += "updated";
        
        // Act.
        var result = _sut.Update(existingEntity);
        await _context.SaveChangesAsync();
        var foundResult = await _sut.GetByIdAsync(result.Id);

        // Assert.
        result.Should()!.BeEquivalentTo(existingEntity);
        foundResult!.Should()!.NotBeNull()!.And!.BeEquivalentTo(existingEntity);
    }

    [Fact]
    public async Task FindAllAsync_ReturnsAllEntitiesThatMatchPredicate()
    {
        // Arrange.
        const int minId = 3;
        
        // Act.
        var result = await _sut.FindAllAsync(entity => entity.Id > minId);

        // Assert.
        result.Should()!.Satisfy(entity => entity.Id > minId);
    }

    [Fact]
    public void AsQueryable_ReturnsEntitiesAsQueryable()
    {
        // Act.
        var result = _sut.AsQueryable();
        
        // Assert.
        result.Should()!.BeAssignableTo<IQueryable<User>>();
    }
}