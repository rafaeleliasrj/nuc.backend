using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NautiHub.Domain.Entities;

namespace NautiHub.Infrastructure.DataContext.Mappings;

/// <summary>
/// Configuração de mapeamento para a entidade ChatMessage no Entity Framework Core.
/// </summary>
internal class ChatMessageMapping : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasIndex(b => b.BookingId);
        builder.HasIndex(b => b.SenderId);
        builder.HasIndex(b => b.IsRead);
        builder.HasIndex(b => new { b.BookingId, b.CreatedAt });

        builder.HasOne<Booking>()
            .WithMany()
            .HasForeignKey(b => b.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(b => b.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(b => b.IsRead)
            .HasDefaultValue(false);

        builder.Property(b => b.CreatedAt)
            .IsRequired();
    }
}