using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDLL.Models.ApplicationDBContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Person> Person {  get; set; }
        public DbSet<TaskListTemplate> TaskListTemplate { get; set; }
        public DbSet<TaskGroup> TaskGroup { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<TaskListInstance> TaskListInstance { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskListTemplate>()
            .HasOne(t => t.CreatedByPerson)
            .WithMany(p => p.CreatedTaskListTemplates)
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

            // Configuring the relationship between TaskListInstance and TaskListTemplate
            modelBuilder.Entity<TaskListInstance>()
                .HasOne(tli => tli.TaskListTemplate)
                .WithMany(tlt => tlt.TaskListInstances)
                .HasForeignKey(tli => tli.TaskListTemplateID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuring the relationship between TaskListInstance and Person
            modelBuilder.Entity<TaskListInstance>()
                .HasOne(tli => tli.AssignedPerson)
                .WithMany(p => p.AssignedTaskListInstances)
                .HasForeignKey(tli => tli.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
