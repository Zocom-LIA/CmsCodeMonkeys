using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.Pages)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId)
                .IsRequired(false) // Allow null author
                .OnDelete(DeleteBehavior.SetNull); // Set AuthorId to null if User is deleted
        }
    }
}