using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.UnitsOfWork;

public sealed class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
    }
}