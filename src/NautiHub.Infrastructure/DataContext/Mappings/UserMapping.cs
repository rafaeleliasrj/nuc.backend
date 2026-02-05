using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;

namespace NautiHub.Infrastructure.DataContext.Mappings;

/// <summary>
/// Configuração do mapeamento da entidade User para o EF Core
/// </summary>
internal class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.UserType)
            .IsRequired()
            .HasConversion<string>();

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.UserName).IsUnique();
        builder.HasIndex(e => e.UserType);
        builder.HasIndex(e => e.CreatedAt);
    }
}