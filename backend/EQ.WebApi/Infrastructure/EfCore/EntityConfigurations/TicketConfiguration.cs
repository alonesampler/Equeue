using EQ.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EQ.WebApi.Infrastructure.EfCore.EntityConfigurations;

// ============================================================
// Ticket
// ============================================================
public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.ChapterCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Number).IsRequired();
        builder.Property(x => x.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasOne<Chapter>().WithMany()
            .HasForeignKey(x => x.ChapterId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Service>().WithMany()
            .HasForeignKey(x => x.ServiceId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>().WithMany()
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Window>().WithMany()
            .HasForeignKey(x => x.WindowId).OnDelete(DeleteBehavior.Restrict);

        // Выбор следующего талона: услуга + статус + порядок создания
        builder.HasIndex(x => new { x.ServiceId, x.Status, x.CreatedAt });
        // Активные талоны окна
        builder.HasIndex(x => new { x.WindowId, x.Status });

        // Оптимистичная блокировка: два оператора не возьмут один талон.
        // Второй получит DbUpdateConcurrencyException.
        builder.Property<uint>("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();
    }
}
