using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Chat.Domain.Entities;
using Chat.Persistence.Contexts;

namespace Chat.Persistence.EntityConfigurations;

public sealed class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.Property(x => x.UpdatedAt)
               .IsRequired()
               .HasDefaultValueSql(ChatDbContext.SqlGetDateFunction)
               .ValueGeneratedOnUpdate();
        
        builder.HasOne(d => d.Conversation)
               .WithMany(p => p.Messages)
               .HasForeignKey(d => d.ConversationId);
        
        builder.HasOne(d => d.Sender)
               .WithMany(p => p.Messages)
               .HasForeignKey(d => d.SenderId);
    }
}