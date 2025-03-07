using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinanceManager.Domain.Entities;

namespace PersonalFinanceManager.Infrastructure.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Category).IsRequired();
        builder.Property(b => b.Amount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.StartDate).IsRequired();
        builder.Property(b => b.EndDate).IsRequired();

        builder.HasOne(b => b.User)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.Id);
        builder.ToTable("Budgets");
    }
}
