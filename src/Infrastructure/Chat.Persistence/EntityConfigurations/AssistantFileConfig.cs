using Microsoft.EntityFrameworkCore;

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