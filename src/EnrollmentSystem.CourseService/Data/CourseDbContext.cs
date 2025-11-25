using EnrollmentSystem.CourseService.Models;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem.CourseService.Data;

public class CourseDbContext : DbContext
{
    public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.StudentId, e.CourseId })
            .IsUnique();

        // Seed data
        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Code = "CS101", Name = "Introduction to Programming", Description = "Learn the basics of programming", MaxStudents = 30, IsOpen = true, Instructor = "Dr. Smith" },
            new Course { Id = 2, Code = "CS201", Name = "Data Structures", Description = "Advanced data structures and algorithms", MaxStudents = 25, IsOpen = true, Instructor = "Dr. Johnson" },
            new Course { Id = 3, Code = "CS301", Name = "Database Systems", Description = "Relational and NoSQL databases", MaxStudents = 20, IsOpen = true, Instructor = "Prof. Williams" }
        );
    }
}