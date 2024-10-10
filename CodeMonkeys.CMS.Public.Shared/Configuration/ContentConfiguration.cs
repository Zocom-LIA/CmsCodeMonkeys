using CodeMonkeys.CMS.Public.Shared.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeMonkeys.CMS.Public.Shared.Configuration
{
    public class ContentConfiguration : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.ToTable("Contents");

            builder.HasKey(content => content.ContentId);

            builder.Property(c => c.ContentType).IsRequired();
            builder.Property(content => content.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(site => site.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(site => site.LastModifiedDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(content => content.OrdinalNumber)
                .IsRequired();

            builder.Property(content => content.Color)
                .IsRequired()
                .HasMaxLength(7)
                .HasDefaultValue("#1e1e1e");

            builder.HasOne(content => content.Author)
                .WithMany()
                .HasForeignKey(content => content.AuthorId);

            builder.HasOne(content => content.WebPage)
                .WithMany()
                .HasForeignKey(content => content.WebPageId);

            builder.HasDiscriminator<string>("ContentType")
                .HasValue<TextContent>(nameof(TextContent))
                .HasValue<ImageContent>(nameof(ImageContent))
                .HasValue<VideoContent>(nameof(VideoContent))
                .HasValue<LinkContent>(nameof(LinkContent))
                .HasValue<CodeContent>(nameof(CodeContent))
                .HasValue<FileContent>(nameof(FileContent))
                .HasValue<CodeContent>(nameof(CodeContent))
                .HasValue<QuoteContent>(nameof(QuoteContent));
        }
    }
}