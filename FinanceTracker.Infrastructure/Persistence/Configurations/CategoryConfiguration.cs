using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Color).HasMaxLength(7);
        builder.Property(c => c.Type).HasConversion<string>().HasMaxLength(10);

        // Aynı kullanıcı aynı isimde iki gider kategorisi açamasın
        builder.HasIndex(c => new { c.UserId, c.Name, c.Type }).IsUnique();

        builder.HasOne(c => c.User)
               .WithMany(u => u.Categories)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}