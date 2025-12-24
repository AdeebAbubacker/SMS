using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Models;

namespace SchoolManagementSystem.Domain.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassBatch> Classes { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RefreshToken>(b =>
            {
                b.Property(e => e.Id)
                     .ValueGeneratedOnAdd()
                     .HasDefaultValueSql("NEWID()");
            });
            modelBuilder.Entity<User>(b =>
            {
                b.Property(e => e.Id)
                     .ValueGeneratedOnAdd()
                     .HasDefaultValueSql("NEWID()");

                b.HasIndex(u => u.Email).IsUnique();
            });
            modelBuilder.Entity<Department>(b =>
            {
                b.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NEWID()");
                b.HasIndex(d => d.Name).IsUnique();
            });
            modelBuilder.Entity<Course>(b =>
            {
                b.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NEWID()");
                b.HasOne(c => c.Department)
                 .WithMany(d => d.Courses)
                 .HasForeignKey(c => c.DepartmentId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<ClassBatch>(b =>
            {
                b.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NEWID()");

                b.HasOne(cb => cb.Course)
                 .WithMany(c => c.Classes)
                 .HasForeignKey(cb => cb.CourseId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(cb => cb.Teacher)
                 .WithMany()
                 .HasForeignKey(cb => cb.TeacherId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Attendance>(b =>
            {
                b.Property(e => e.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

                b.HasOne(a => a.Student)
                  .WithMany()
                  .HasForeignKey(a => a.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

                b.HasOne<User>()
                 .WithMany()
                 .HasForeignKey(a => a.MarkedByTeacherId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StudentClass>(b =>
            {
                b.Property(e => e.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

                b.HasOne(sc => sc.Student)
                 .WithMany()
                 .HasForeignKey(sc => sc.StudentId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(sc => sc.ClassBatch)
                 .WithMany(cb => cb.StudentClasses)
                 .HasForeignKey(sc => sc.ClassBatchId)
                 .OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(sc => new { sc.StudentId, sc.ClassBatchId })
                 .IsUnique();
            });
            modelBuilder.Entity<Assignment>(b =>
            {
                b.Property(e => e.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

                b.HasOne(a => a.ClassBatch)
                 .WithMany(cb => cb.Assignments)
                 .HasForeignKey(a => a.ClassBatchId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<User>()
                 .WithMany()
                 .HasForeignKey(a => a.MarkedByTeacherId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Submission>(b =>
            {
                b.Property(e => e.Id)
                  .ValueGeneratedOnAdd()
                  .HasDefaultValueSql("NEWID()");

                b.HasOne(s => s.Assignment)
                 .WithMany(a => a.Submissions)
                 .HasForeignKey(s => s.AssignmentId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(s => s.Student)
                 .WithMany()
                 .HasForeignKey(s => s.StudentId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}