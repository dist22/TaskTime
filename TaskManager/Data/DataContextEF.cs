using Microsoft.EntityFrameworkCore;
using TaskManager.Configurations;
using TaskManager.Models;

namespace TaskManager.Data;

public class DataContextEF(DbContextOptions<DataContextEF> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<TaskTime> Tasks { get; set; }
    public DbSet<Category> Categories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TaskTimeConfigure());
        modelBuilder.ApplyConfiguration(new CategorieConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}