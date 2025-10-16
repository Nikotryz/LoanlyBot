using Microsoft.EntityFrameworkCore;

namespace LoanlyBot.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Loaner> Loaners { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<Loaner>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("loaners_pkey");

            entity.ToTable("loaners");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.HisLoan)
                .HasColumnType("money")
                .HasColumnName("his_loan");
            entity.Property(e => e.MyLoan)
                .HasColumnType("money")
                .HasColumnName("my_loan");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.RecipientId).HasColumnName("recipient_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");

            entity.HasOne(d => d.Recipient).WithMany(p => p.PaymentRecipients)
                .HasForeignKey(d => d.RecipientId)
                .HasConstraintName("payments_recipient_id_fkey");

            entity.HasOne(d => d.Sender).WithMany(p => p.PaymentSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("payments_sender_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
