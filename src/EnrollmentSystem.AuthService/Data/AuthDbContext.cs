using EnrollmentSystem.AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem.AuthService.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                Username = "student1", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Email = "student1@university.edu",
                Role = "Student"
            },
            new User 
            { 
                Id = 2, 
                Username = "faculty1", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Email = "faculty1@university.edu",
                Role = "Faculty"
            }
        );
    }
}