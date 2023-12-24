namespace Chat.IntegrationTests.TestHelpers;

internal sealed class TestApplicationFactory : WebApplicationFactory<Program>
{
    private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=ChatDb;Trusted_Connection=True";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ChatDbContext>));
            services.AddSqlServer<ChatDbContext>(ConnectionString);
            RecreateTestDb(services);
        });
        builder.UseEnvironment("Development");
        base.ConfigureWebHost(builder);
    }

    private void RecreateTestDb(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        dbContext.Database.EnsureCreated();
        ReseedDb(dbContext);
    }

    private void ReseedDb(ChatDbContext dbContext)
    {
        dbContext.Database.ExecuteSqlRaw(DbScripts.DeleteAllData);
        dbContext.Database.ExecuteSqlRaw(DbScripts.SeedTestData);
    }
}