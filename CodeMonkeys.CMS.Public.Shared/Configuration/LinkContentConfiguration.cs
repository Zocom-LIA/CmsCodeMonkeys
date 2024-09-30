//using CodeMonkeys.CMS.Public.Shared.Entities;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace CodeMonkeys.CMS.Public.Shared.Configuration
//{
//    public class LinkContentConfiguration : IEntityTypeConfiguration<LinkContent>
//    {
//        public void Configure(EntityTypeBuilder<LinkContent> builder)
//        {
//            builder.Property(content => content.LinkUrl)
//                .IsRequired();

//            builder.Property(content => content.Label)
//                .IsRequired();

//            builder.Property(content => content.IsEnabled)
//                .IsRequired()
//                .HasDefaultValue(true);
//        }
//    }
//}