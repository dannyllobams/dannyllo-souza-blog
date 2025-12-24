using dbs.domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dbs.infra.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.UrlSlug)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.HasIndex(p => p.UrlSlug)
                .IsUnique();

            builder.Property(p => p.UrlMainImage)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Content)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(p => p.Summary)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(p => p.Status)
                .IsRequired();

            builder.OwnsOne(p => p.SEO, seo =>
            {
                seo.Property(s => s.MetaTitle)
                    .HasColumnName("MetaTitle")
                    .HasColumnType("varchar(200)");

                seo.Property(s => s.MetaDescription)
                    .HasColumnName("MetaDescription")
                    .HasColumnType("varchar(500)");
            });

            builder.HasMany(p => p.Categories)
                .WithMany(c => c.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostCategories",
                    j => j
                        .HasOne<Category>()
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                     j =>
                     {
                         j.HasKey("PostId", "CategoryId");
                     }
                );

            builder.HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTags",
                    j => j
                        .HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                     j =>
                     {
                         j.HasKey("PostId", "TagId");
                     }
                );

            builder.HasMany(p => p.Comments)
                .WithOne() 
                .HasForeignKey("PostId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
