namespace Chat.Persistence.EntityConfigurations;

public sealed class GroupConfig : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasOne(x => x.Creator)
               .WithMany(x => x.OwnGroups)
               .HasForeignKey(x => x.CreatorId);
        
        builder.HasMany(x => x.Members)
               .WithMany(x => x.ForeignGroups)
               .UsingEntity<GroupMember>(
                   l => l.HasOne(x => x.User)
                         .WithMany(x => x.GroupMembers)
                         .HasForeignKey(x => x.UserId)
                         .OnDelete(DeleteBehavior.NoAction),
                   r => r.HasOne(x => x.Group)
                         .WithMany(x => x.GroupMembers)
                         .HasForeignKey(x => x.GroupId)
                         .OnDelete(DeleteBehavior.NoAction));

        builder.HasMany(x => x.Files)
               .WithOne(x => x.Group)
               .HasForeignKey(x => x.GroupId);
    }
}