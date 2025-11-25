using EnrollmentSystem.GradeService.Models;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem.GradeService.Data;

public class GradeDbContext : DbContext
{
    public GradeDbContext(DbContextOptions<GradeDbContext> options) : base(options) { }

    public DbSet<Grade> Grades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Grade>()
            .HasIndex(g => new { g.StudentId, g.CourseId })
            .IsUnique();
    }
}