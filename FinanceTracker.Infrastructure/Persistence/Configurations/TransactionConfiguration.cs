using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Amount).HasPrecision(18, 2);
        builder.Property(t => t.Description).HasMaxLength(250);
        builder.Property(t => t.Type).HasConversion<string>().HasMaxLength(10);

        // Raporlar hep "bu kullanıcının şu tarih aralığı" diye sorgulayacak
        builder.HasIndex(t => new { t.UserId, t.Date });

        builder.HasOne(t => t.User)
               .WithMany(u => u.Transactions)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        // İçinde işlem olan bir kategori silinemesin
        builder.HasOne(t => t.Category)
               .WithMany(c => c.Transactions)
               .HasForeignKey(t => t.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}