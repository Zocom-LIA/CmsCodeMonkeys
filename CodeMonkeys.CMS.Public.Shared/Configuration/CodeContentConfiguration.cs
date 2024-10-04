//using CodeMonkeys.CMS.Public.Shared.Entities;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace CodeMonkeys.CMS.Public.Shared.Configuration
//{
//    public class CodeContentConfiguration : IEntityTypeConfiguration<CodeContent>
//    {
//        public void Configure(EntityTypeBuilder<CodeContent> builder)
//        {
//            builder.Property(content => content.Code)
//                .IsRequired();

//            builder.Property(content => content.Language)
//                .IsRequired()
//                .HasMaxLength(100)
//                .HasDefaultValue("csharp");
//        }
//    }
//}