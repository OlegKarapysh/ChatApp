using Chat.Domain.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Chat.Domain.Entities;

public class User : IdentityUser<int>, ICreatableEntity, IEntity<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime TokenExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IList<Message> Messages { get; set; } = new List<Message>();
}