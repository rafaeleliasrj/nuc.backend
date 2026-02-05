using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NautiHub.Domain.Entities;
using NautiHub.Domain.ValueObjects;
using NautiHub.Domain.Enums;

namespace NautiHub.Infrastructure.DataContext.Mappings;

/// <summary>
/// Configuração do mapeamento da entidade Payment para o EF Core
/// </summary>
internal class PaymentMapping : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.AsaasPaymentId)
            .HasMaxLength(100);

        builder.Property(e => e.Value)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(e => e.NetValue)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(e => e.Method)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.InvoiceUrl)
            .HasMaxLength(2048);

        builder.Property(e => e.BankSlipUrl)
            .HasMaxLength(2048);

        builder.Property(e => e.TransactionReceiptUrl)
            .HasMaxLength(2048);

        builder.Property(e => e.PixQrCode)
            .HasColumnType("text");

        builder.Property(e => e.PixEncodedImage)
            .HasColumnType("text");

        builder.Property(e => e.DueDate)
            .HasColumnType("timestamp");

        builder.Property(e => e.ConfirmedDate)
            .HasColumnType("timestamp");

        builder.Property(e => e.PaymentDate)
            .HasColumnType("timestamp");

        builder.Property(e => e.CreditDate)
            .HasColumnType("timestamp");

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.ExternalReference)
            .HasMaxLength(255);

        builder.Property(e => e.BillingType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp");

        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasColumnType("timestamp");

        builder.Property(e => e.IsDeleted)
            .IsRequired();

        // Índices
        builder.HasIndex(e => e.AsaasPaymentId);
        builder.HasIndex(e => e.BookingId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.Method);
        builder.HasIndex(e => e.DueDate);
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.ExternalReference);

        builder.Property(e => e.BookingId)
            .IsRequired()
            .HasMaxLength(36);

        builder.OwnsOne(e => e.CreditCardInfo, creditCard =>
        {
            creditCard.Property(cc => cc.HolderName)
                .HasColumnName("CreditCardHolderName")
                .HasMaxLength(255);

            creditCard.Property(cc => cc.LastFourDigits)
                .HasColumnName("CreditCardLastFourDigits")
                .HasMaxLength(4);

            creditCard.Property(cc => cc.ExpiryMonth)
                .HasColumnName("CreditCardExpiryMonth");

            creditCard.Property(cc => cc.ExpiryYear)
                .HasColumnName("CreditCardExpiryYear");

            creditCard.Property(cc => cc.Brand)
                .HasColumnName("CreditCardBrand")
                .HasMaxLength(50);

            creditCard.Property(cc => cc.Token)
                .HasColumnName("CreditCardToken")
                .HasMaxLength(255);
        });

        // Relacionamento com Booking
        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Configuração para PaymentSplits como owned collection
        builder.OwnsMany(e => e.Splits, split =>
        {
            split.ToTable("PaymentSplits");
            
            split.WithOwner().HasForeignKey("PaymentId");
            
            split.HasKey("Id");
            
            split.Property<Guid>("Id")
                .ValueGeneratedOnAdd();
            
            split.Property(s => s.WalletId)
                .IsRequired()
                .HasMaxLength(100);
            
            split.Property(s => s.FixedValue)
                .HasPrecision(18, 4);
            
            split.Property(s => s.PercentualValue)
                .HasPrecision(5, 2);
            
            split.Property(s => s.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("PENDING");
            
            split.Property(s => s.RefusalReason)
                .HasMaxLength(500);
            
            split.Property(s => s.ExternalReference)
                .HasMaxLength(255);
            
            split.Property(s => s.Description)
                .HasMaxLength(500);
            
            split.Property<DateTime>("CreatedAt")
                .IsRequired()
                .HasColumnType("timestamp")
                .HasDefaultValueSql("NOW()");
            
            split.Property<DateTime>("UpdatedAt")
                .IsRequired()
                .HasColumnType("timestamp")
                .HasDefaultValueSql("NOW()");
            
            // Índices para a tabela PaymentSplits
            split.HasIndex("PaymentId");
            split.HasIndex(s => s.WalletId);
            split.HasIndex(s => s.Status);
        });

        // Query filter para soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}