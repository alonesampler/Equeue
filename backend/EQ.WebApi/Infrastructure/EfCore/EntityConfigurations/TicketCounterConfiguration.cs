using EQ.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EQ.WebApi.Infrastructure.EfCore.EntityConfigurations;

// ============================================================
// TicketCounter
// ============================================================
public class TicketCounterConfiguration : IEntityTypeConfiguration<TicketCounter>
{
    public void Configure(EntityTypeBuilder<TicketCounter> builder)
    {
        builder.ToTable("ticket_counters");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.LastNumber).IsRequired();

        builder.HasOne<Service>().WithMany()
            .HasForeignKey(x => x.ServiceId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ServiceId).IsUnique();

        // Два клиента одновременно: второй получит DbUpdateConcurrencyException -> повтор в Application
        builder.Property<uint>("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();
    }
}
