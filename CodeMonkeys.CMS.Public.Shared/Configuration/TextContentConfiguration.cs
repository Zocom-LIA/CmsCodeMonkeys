//using CodeMonkeys.CMS.Public.Shared.Entities;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace CodeMonkeys.CMS.Public.Shared.Configuration
//{
//    public class TextContentConfiguration : IEntityTypeConfiguration<TextContent>
//    {
//        public void Configure(EntityTypeBuilder<TextContent> builder)
//        {
//            builder.Property(content => content.Text)
//                .IsRequired();

//            builder.Property(content => content.TextColor)
//                .IsRequired()
//                .HasMaxLength(7)
//                .HasDefaultValue("Black");

//            builder.Property(content => content.FontFamily)
//                .IsRequired()
//                .HasMaxLength(100)
//                .HasDefaultValue("Arial");

//            builder.Property(content => content.FontSize)
//                .IsRequired()
//                .HasDefaultValue(16);

//            builder.Property(content => content.IsBold)
//                .IsRequired()
//                .HasDefaultValue(false);

//            builder.Property(content => content.IsItalic)
//                .IsRequired()
//                .HasDefaultValue(false);
//        }
//    }
//}