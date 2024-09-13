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
        public DbSet<TaskInstance> TaskInstance { get; set; }
        public DbSet<TaskAttachment> TaskAttachment { get; set; }
        public DbSet<PersonHistory> PersonHistory { get; set; }
        public DbSet<TaskListTemplateHistory> TaskListTemplateHistory { get; set; }
        public DbSet<TaskGroupHistory> TaskGroupHistory { get; set; }
        public DbSet<TaskHistory> TaskHistory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskListTemplate>()
            .HasOne(t => t.CreatedByPerson)
            .WithMany(p => p.CreatedTaskListTemplates)
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskListInstance>()
            .HasOne(tli => tli.TaskListTemplate)
            .WithMany(tlt => tlt.TaskListInstances)
            .HasForeignKey(tli => tli.TaskListTemplateID)
            .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<TaskAttachment>()
            .HasOne(ta => ta.TaskInstance)
            .WithMany(ti => ti.TaskAttachments)
            .HasForeignKey(ta => ta.TaskInstanceID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAttachment>()
            .HasOne(ta => ta.UploadedByPerson)
            .WithMany(p => p.UploadedTaskAttachments)
            .HasForeignKey(ta => ta.UploadedBy)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskListTemplateHistory>()
            .HasOne(tlth => tlth.CreatedByPerson)
            .WithMany(p => p.CreatedTaskListTemplateHistories)
            .HasForeignKey(tlth => tlth.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskListTemplateHistory>()
            .HasOne(tlth => tlth.ChangedByPerson)
            .WithMany(p => p.ChangedTaskListTemplateHistories)
            .HasForeignKey(tlth => tlth.ChangedBy)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PersonHistory>()
            .HasOne(ph => ph.ChangedByPerson)
            .WithMany()
            .HasForeignKey(ph => ph.ChangedBy)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskGroupHistory>()
            .HasOne(tgh => tgh.ChangedByPerson)
            .WithMany()
            .HasForeignKey(tgh => tgh.ChangedBy)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskHistory>()
            .HasOne(th => th.ChangedByPerson)
            .WithMany()
            .HasForeignKey(th => th.ChangedBy);

            modelBuilder.Entity<TaskHistory>()
            .HasOne(th => th.TaskGroup)
            .WithMany()
            .HasForeignKey(th => th.TaskGroupID);

            modelBuilder.Entity<TaskHistory>()
            .HasOne(th => th.DependancyTask)
            .WithMany()
            .HasForeignKey(th => th.DependancyTaskID);

            base.OnModelCreating(modelBuilder);
        }

    }
}
