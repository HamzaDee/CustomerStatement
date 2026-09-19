using CustomerStatement.Application.Interfaces;
using CustomerStatement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerStatement.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<AccountStatement> AccountStatements { get; set; }

    public DbSet<StatementTransaction> StatementTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(320);
        });

        modelBuilder.Entity<AccountStatement>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.OpeningBalance).HasPrecision(18, 2);
            entity.Property(x => x.ClosingBalance).HasPrecision(18, 2);
            entity.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(x => new
            {
                x.CustomerId,
                x.StatementMonth
            })
            .IsUnique();
        });

        modelBuilder.Entity<StatementTransaction>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.Debit)
                .HasPrecision(18, 2);

            entity.Property(x => x.Credit)
                .HasPrecision(18, 2);

            entity.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.AccountStatement)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.AccountStatementId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}