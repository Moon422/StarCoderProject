using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class StarDb : DbContext
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<TaskModel> Tasks { get; set; }

    public StarDb(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(
            options =>
            {
                options.HasOne(p => p.User)
                    .WithOne(u => u.Profile)
                    .HasForeignKey<Profile>(p => p.UserId)
                    .IsRequired();

                options.HasIndex(p => p.Email)
                    .IsUnique();
            }
        );

        modelBuilder.Entity<User>(
            options =>
            {
                options.Property(u => u.Password)
                    .HasMaxLength(60)
                    .IsFixedLength()
                    .IsRequired();

                options.HasIndex(u => u.Username)
                    .IsUnique();
            }
        );

        modelBuilder.Entity<RefreshToken>(
            options =>
            {
                options.Property(r => r.Token)
                    .HasMaxLength(32)
                    .IsFixedLength()
                    .IsRequired();

                options.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .IsRequired();
            }
        );

        modelBuilder.Entity<TaskModel>(
            options =>
            {
                options.HasOne(t => t.Profile)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(t => t.ProfileId)
                    .IsRequired();
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}
