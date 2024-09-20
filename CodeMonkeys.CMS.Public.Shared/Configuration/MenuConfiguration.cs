using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CodeMonkeys.CMS.Public.Shared.Configuration
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(m => m.MenuId);
            builder.Property(m => m.MenuId).ValueGeneratedOnAdd();

            builder.HasOne(m => m.Site)
                .WithMany(s => s.Menus)
                .HasForeignKey(m => m.SiteId)
                .IsRequired();

            builder.HasMany(m => m.Items)
                .WithOne(i => i.Menu)
                .HasForeignKey(i => i.MenuId)
                .IsRequired();
        }
    }
}
