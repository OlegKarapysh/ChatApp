using Chat.Domain.Entities.Conversations;
using Chat.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Persistence.EntityConfigurations;

public sealed class ConversationConfig : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.Property(x => x.UpdatedAt)
               .IsRequired()
               .HasDefaultValueSql(ChatDbContext.SqlGetDateFunction)
               .ValueGeneratedOnUpdate();
        
        builder.HasMany(x => x.Members)
               .WithMany(x => x.Conversations)
               .UsingEntity<ConversationParticipant>(
                   l => l.HasOne(x => x.User)
                         .WithMany(x => x.ConversationParticipants)
                         .HasForeignKey(x => x.UserId),
                   r => r.HasOne(x => x.Conversation)
                         .WithMany(x => x.ConversationParticipants)
                         .HasForeignKey(x => x.ConversationId));
    }
}