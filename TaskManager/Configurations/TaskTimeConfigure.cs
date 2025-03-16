using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Models;

namespace TaskManager.Configurations;

public class TaskTimeConfigure : IEntityTypeConfiguration<TaskTime>
{
    public void Configure(EntityTypeBuilder<TaskTime> builder)
    {
        builder.HasKey(f => f.TaskId);

        builder
            .HasMany(f => f.Users)
            .WithMany(u => u.Tasks);

        builder
            .HasOne(t => t.Category)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}