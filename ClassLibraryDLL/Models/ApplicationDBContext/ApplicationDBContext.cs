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
        public DbSet<TaskGroupInstance> TaskGroupInstance { get; set; }
        public DbSet<TaskInstance> taskInstance { get; set; }


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

            modelBuilder.Entity<TaskGroupInstance>()
            .HasOne(tgi => tgi.TaskGroup)
            .WithMany(tg => tg.TaskGroupInstances)
            .HasForeignKey(tgi => tgi.TaskGroupID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskGroupInstance>()
                .HasOne(tgi => tgi.TaskListInstance)
                .WithMany(tli => tli.TaskGroupInstances)
                .HasForeignKey(tgi => tgi.TaskListInstanceID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskGroupInstance>()
                .HasOne(tgi => tgi.AssignedPerson)
                .WithMany(p => p.TaskGroupInstances)
                .HasForeignKey(tgi => tgi.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskInstance>()
            .HasOne(ti => ti.Task)
            .WithMany(t => t.TaskInstances)
            .HasForeignKey(ti => ti.TaskID);

            modelBuilder.Entity<TaskInstance>()
                .HasOne(ti => ti.TaskGroupInstance)
                .WithMany(tgi => tgi.TaskInstances)
                .HasForeignKey(ti => ti.TaskGroupInstanceID);

            base.OnModelCreating(modelBuilder);
        }

    }
}
