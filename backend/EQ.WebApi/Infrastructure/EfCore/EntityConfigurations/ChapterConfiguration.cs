using EQ.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EQ.WebApi.Infrastructure.EfCore.EntityConfigurations;

// ============================================================
// Chapter  (Chapter <-> Service: многие-ко-многим, таблица chapter_services)
// ============================================================
public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.ToTable("chapters");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Code).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasMany(x => x.Services)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "chapter_services",
                right => right.HasOne<Service>().WithMany()
                    .HasForeignKey("ServiceId").OnDelete(DeleteBehavior.Cascade),
                left => left.HasOne<Chapter>().WithMany()
                    .HasForeignKey("ChapterId").OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("chapter_services");
                    join.HasKey("ChapterId", "ServiceId");
                    join.HasIndex("ServiceId").IsUnique();
                });

        builder.Navigation(x => x.Services).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
