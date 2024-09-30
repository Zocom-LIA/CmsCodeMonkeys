//using CodeMonkeys.CMS.Public.Shared.Entities;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace CodeMonkeys.CMS.Public.Shared.Configuration
//{
//    public class FileContentConfiguration : IEntityTypeConfiguration<FileContent>
//    {
//        public void Configure(EntityTypeBuilder<FileContent> builder)
//        {
//            builder.Property(content => content.FileName)
//                .IsRequired();

//            builder.Property(content => content.FileUrl)
//                .IsRequired();

//            builder.Property(content => content.Description)
//                .IsRequired();
//        }
//    }
//}