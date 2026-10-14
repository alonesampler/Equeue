using EQ.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EQ.WebApi.Infrastructure.EfCore.EntityConfigurations;

// ============================================================
// WindowSection
// ============================================================
public class WindowSectionConfiguration : IEntityTypeConfiguration<WindowSection>
{
    public void Configure(EntityTypeBuilder<WindowSection> builder)
    {
        builder.ToTable("window_sections");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd(); // long: Id выдаёт база

        builder.Property(x => x.StartedAt).IsRequired();
        builder.Property(x => x.EndedAt);
        builder.Ignore(x => x.IsOpen);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Одно окно - одна открытая сессия; один оператор - одна открытая сессия.
        // Если включите snake_case naming convention, поправьте имя колонки в фильтре.
        builder.HasIndex(x => x.WindowId)
            .IsUnique()
            .HasFilter("\"EndedAt\" IS NULL");

        builder.HasIndex(x => x.UserId)
            .IsUnique()
            .HasFilter("\"EndedAt\" IS NULL");
    }
}
