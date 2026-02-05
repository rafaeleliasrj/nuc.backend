using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NautiHub.Domain.Entities;
using NautiHub.Domain.ValueObjects;

namespace NautiHub.Infrastructure.DataContext.Mappings;

public class ReviewMapping : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasIndex(x => x.BookingId);

        builder.HasIndex(x => x.BoatId);

        builder.HasIndex(x => x.CustomerId);

        builder.HasIndex(x => x.Rating);

        builder.HasIndex(x => x.CreatedAt);

        builder.HasIndex(x => x.UserId);
    }
}