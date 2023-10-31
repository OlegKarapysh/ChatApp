﻿using Chat.Domain.Models.Conversations;
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
    }
}