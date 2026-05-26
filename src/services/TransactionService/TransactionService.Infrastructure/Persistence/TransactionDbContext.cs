using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Aggregates;

namespace TransactionService.Infrastructure.Persistence;

public class TransactionDbContext(DbContextOptions<TransactionDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Status).HasConversion<string>();
            entity.Property(u => u.Type).HasConversion<string>();
            entity.Property(u => u.Amount).HasColumnType("decimal(18,2)");
            entity.Property(u => u.Currency).HasMaxLength(3);
            entity.HasIndex(u => u.IdempotencyKey).IsUnique();
        });
    }
}