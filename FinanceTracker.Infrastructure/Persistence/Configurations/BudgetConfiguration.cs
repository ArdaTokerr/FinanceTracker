using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.MonthlyLimit).HasPrecision(18, 2);

        // Bir kategoriye aynı ay için tek bütçe tanımlanabilsin
        builder.HasIndex(b => new { b.UserId, b.CategoryId, b.Year, b.Month }).IsUnique();

        builder.HasOne(b => b.User)
               .WithMany(u => u.Budgets)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Category)
               .WithMany()
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}