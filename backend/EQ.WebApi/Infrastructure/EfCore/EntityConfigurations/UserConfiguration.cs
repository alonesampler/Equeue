using EQ.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EQ.WebApi.Infrastructure.EfCore.EntityConfigurations;

// ============================================================
// User  (User <-> Chapter: многие-ко-многим, таблица user_chapters)
// ============================================================
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Login).IsRequired().HasMaxLength(100);
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Role).IsRequired().HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(x => x.Login).IsUnique();

        builder.OwnsOne(x => x.FullName, fn =>
        {
            fn.Property(x => x.Name).HasColumnName("FullName_FirstName");
            fn.Property(x => x.Surname).HasColumnName("FullName_LastName");
            fn.Property(x => x.Patronymic).HasColumnName("FullName_MiddleName");
        });

        builder.HasMany(x => x.Chapters)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "user_chapters",
                right => right.HasOne<Chapter>().WithMany()
                    .HasForeignKey("ChapterId").OnDelete(DeleteBehavior.Cascade),
                left => left.HasOne<User>().WithMany()
                    .HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("user_chapters");
                    join.HasKey("UserId", "ChapterId");
                });

        builder.Navigation(x => x.Chapters).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
