using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NautiHub.Core.DomainObjects;
using NautiHub.Domain.Entities;

namespace NautiHub.Infrastructure.DataContext.Mappings;

internal class BookingMapping : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasIndex(r => r.UserId);
        builder.HasIndex(b => b.BookingNumber).IsUnique();
        builder.HasIndex(b => b.BoatId);
        builder.HasIndex(b => b.GuestId);
        builder.HasIndex(b => b.StartDate);
        builder.HasIndex(b => b.EndDate);
        builder.HasIndex(b => b.Status);

builder.HasOne(b => b.Boat)
            .WithMany()
            .HasForeignKey(b => b.BoatId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

builder.HasOne(b => b.Guest)
            .WithMany()
            .HasForeignKey(b => b.GuestId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
