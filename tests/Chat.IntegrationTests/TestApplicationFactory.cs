namespace Chat.IntegrationTests;

internal sealed class TestApplicationFactory : WebApplicationFactory<Program>
{
    private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=ChatDb;Trusted_Connection=True";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ChatDbContext>));
            services.AddSqlServer<ChatDbContext>(ConnectionString);
            RecreateDb(services);
        });
        builder.UseEnvironment("Development");
        base.ConfigureWebHost(builder);
    }

    private void RecreateDb(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}