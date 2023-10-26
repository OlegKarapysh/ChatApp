using Chat.Domain.Abstract;

namespace Chat.Domain.Models;

public class User : EntityBase<int>
{
    public string Name { get; set; } = string.Empty;
}