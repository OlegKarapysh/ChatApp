using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence;

public sealed class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options);