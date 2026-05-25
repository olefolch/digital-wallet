using AccountService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Persistence;

public class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.OwnerId).IsRequired();
            entity.Property(u => u.Status).HasConversion<string>();
            entity.OwnsOne(a => a.Balance, money =>
                {
                    money.Property(m => m.Amount)
                         .HasColumnName("Balance")
                         .HasColumnType("decimal(18,2)");
                    money.Property(m => m.Currency)
                         .HasColumnName("Currency")
                         .HasMaxLength(3);
                });
            entity.HasIndex(u => u.OwnerId).IsUnique();
        });
    }
}