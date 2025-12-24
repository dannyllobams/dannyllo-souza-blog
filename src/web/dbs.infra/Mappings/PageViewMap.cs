using dbs.domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dbs.infra.Mappings
{
    public class PageViewMap : IEntityTypeConfiguration<PageView>
    {
        public void Configure(EntityTypeBuilder<PageView> builder)
        {
            builder.ToTable("PageViews");

            builder.HasKey(p => p.Id);

            builder.HasIndex(p => new { p.PageId, p.Date }).IsUnique();

            builder.Property(p => p.PageId)
                .IsRequired();

            builder.Property(p => p.Date)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(p => p.TotalViews)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
