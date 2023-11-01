using Chat.Domain.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Chat.Domain.Models;

public class User : IdentityUser<int>, ICreatableEntity
{
    public string? AvatarUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IList<Message> Messages { get; set; } = new List<Message>();
}