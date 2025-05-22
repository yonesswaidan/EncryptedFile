using Microsoft.EntityFrameworkCore;
using EncryptedFileApp.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EncryptedFileApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<RegistrationAttempt> RegistrationAttempts { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<Bruger> Brugere { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is UserFile file)
                {
                    if (entry.State == EntityState.Modified)
                    {
                    }
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFile>(entity =>
            {
                entity.Property(f => f.FileName)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(f => f.ContentType)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(f => f.EncryptedData)
                      .IsRequired();

                entity.Property(f => f.UploadTime)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<UserFile>()
                .HasIndex(f => new { f.BrugerId, f.FileName })
                .IsUnique();

            modelBuilder.Entity<Bruger>(entity =>
            {

                entity.Property(b => b.Brugernavn)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(b => b.Kodeord)
                      .IsRequired()
                      .HasMaxLength(255); 
            });
        }
    }
}
