using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;

namespace NautiHub.Infrastructure.DataContext.Mappings;

/// <summary>
/// Configuração de mapeamento da entidade ScheduledTour para EF Core.
/// </summary>
public class ScheduledTourMapping : IEntityTypeConfiguration<ScheduledTour>
{
    public void Configure(EntityTypeBuilder<ScheduledTour> builder)
    {
        builder.HasKey(st => st.Id);

        builder.HasQueryFilter(st => !st.IsDeleted);

        builder.HasOne(st => st.Boat)
            .WithMany()
            .HasForeignKey(st => st.BoatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(st => st.BoatId);
        builder.HasIndex(st => st.TourDate);
        builder.HasIndex(st => st.Status);
        builder.HasIndex(st => st.CreatedAt);
        builder.HasIndex(st => new { st.BoatId, st.TourDate });
        builder.HasIndex(st => new { st.TourDate, st.Status });
        builder.HasIndex(st => new { st.BoatId, st.TourDate, st.Status });
    }
}