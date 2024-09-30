//using CodeMonkeys.CMS.Public.Shared.Entities;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace CodeMonkeys.CMS.Public.Shared.Configuration
//{
//    public class VideoContentConfiguration : IEntityTypeConfiguration<VideoContent>
//    {
//        public void Configure(EntityTypeBuilder<VideoContent> builder)
//        {
//            builder.Property(content => content.SourceUrl)
//                .IsRequired();

//            builder.Property(content => content.AltText)
//                .IsRequired();

//            builder.Property(content => content.ThumbnailUrl)
//                .IsRequired();

//            builder.Property(content => content.Duration)
//                .IsRequired();

//            builder.Property(content => content.Width)
//                .IsRequired()
//                .HasDefaultValue(100);

//            builder.Property(content => content.Height)
//                .IsRequired()
//                .HasDefaultValue(100);
//        }
//    }
//}