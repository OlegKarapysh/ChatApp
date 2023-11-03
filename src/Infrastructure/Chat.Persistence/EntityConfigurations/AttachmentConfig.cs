using Chat.Domain.Models.Attachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Persistence.EntityConfigurations;

public sealed class AttachmentConfig : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasOne(x => x.Message)
               .WithMany(x => x.Attachments)
               .HasForeignKey(x => x.MessageId);
    }
}