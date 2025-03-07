using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinanceManager.Domain.Entities;

namespace PersonalFinanceManager.Infrastructure.Configurations;

public class SavingsGoalConfiguration : IEntityTypeConfiguration<SavingsGoal>
{
    public void Configure(EntityTypeBuilder<SavingsGoal> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.GoalName).IsRequired();
        builder.Property(s => s.TargetAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.CurrentAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.Deadline).IsRequired();

        builder.HasOne(s => s.User)
            .WithMany(u => u.SavingsGoals)
            .HasForeignKey(s => s.UserId);
        builder.ToTable("SavingsGoals");
    }
}
