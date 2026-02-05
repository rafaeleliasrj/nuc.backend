using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;

namespace NautiHub.Infrastructure.DataContext.Mappings;

/// <summary>
/// Configuração de mapeamento da entidade Boat para EF Core.
/// </summary>
public class BoatMapping : IEntityTypeConfiguration<Boat>
{
    public void Configure(EntityTypeBuilder<Boat> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasQueryFilter(b => !b.IsDeleted);

        builder.Property(b => b.Images)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                v => System.Text.Json.JsonSerializer.Deserialize<ICollection<string>>(v, (System.Text.Json.JsonSerializerOptions)null!) ?? new List<string>())
            .Metadata.SetValueComparer(
                new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<ICollection<string>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c == null ? 0 : c.Aggregate(0, (h, item) => HashCode.Combine(h, item.GetHashCode())),
                    c => c == null ? new List<string>() : new List<string>(c)));

        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.UserId);

        builder.HasIndex(b => b.Document)
            .IsUnique();

        builder.HasIndex(b => new { b.LocationCity, b.LocationState });

        builder.HasIndex(b => b.BoatType);

        builder.HasIndex(b => b.Status);

        builder.HasIndex(b => b.IsActive);

        builder.HasIndex(b => b.PricePerPerson);

        builder.HasIndex(b => new { b.Status, b.IsActive });
    }
}