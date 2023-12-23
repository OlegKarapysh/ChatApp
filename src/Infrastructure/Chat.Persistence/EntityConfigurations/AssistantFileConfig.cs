using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Chat.Domain.Entities;
using Chat.Persistence.Contexts;

namespace Chat.Persistence.EntityConfigurations;

public sealed class AssistantFileConfig : IEntityTypeConfiguration<AssistantFile>
{
    public void Configure(EntityTypeBuilder<AssistantFile> builder)
    {
        builder.Property(x => x.UpdatedAt)
               .IsRequired()
               .HasDefaultValueSql(ChatDbContext.SqlGetDateFunction)
               .ValueGeneratedOnUpdate();
    }
}