using EQ.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EQ.WebApi.Infrastructure.EfCore.EntityConfigurations;

// ============================================================
// Window  (Window 1 -> N WindowSection)
// ============================================================
public class WindowConfiguration : IEntityTypeConfiguration<Window>
{
    public void Configure(EntityTypeBuilder<Window> builder)
    {
        builder.ToTable("windows");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd(); // int: Id выдаёт база

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();

        // Требует навигацию Window.Sections (см. сообщение)
        builder.HasMany(x => x.Sections)
            .WithOne()
            .HasForeignKey(x => x.WindowId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Sections).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
