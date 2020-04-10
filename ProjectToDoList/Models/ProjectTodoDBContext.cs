using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProjectToDoList.Models
{
    public class ProjectTodoDBContext: DbContext
    {
        public ProjectTodoDBContext(DbContextOptions<ProjectTodoDBContext> options) : base(options)
        {

          

       }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoTask>().HasIndex(p => new { p.Title }).IsUnique();
        }
        public DbSet<ToDoTask> ToDoTasks { get; set; }
    }
}
