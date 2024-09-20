using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CodeMonkeys.CMS.Public.Shared.Configuration
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.HasKey(mi => mi.MenuItemId);
            builder.Property(mi => mi.MenuItemId).ValueGeneratedOnAdd();

            builder.HasOne(mi => mi.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(mi => mi.ParentId)
                .IsRequired(false); // Allow null parent

            builder.HasOne(mi => mi.Menu)
                .WithMany(m => m.Items)
                .HasForeignKey(mi => mi.MenuId)
                .IsRequired();

            builder.HasOne(mi => mi.WebPage)
                .WithMany() // No inverse navigation property
                .HasForeignKey(mi => mi.WebPageId)
                .IsRequired(false); // Allow null web page
        }
    }
}