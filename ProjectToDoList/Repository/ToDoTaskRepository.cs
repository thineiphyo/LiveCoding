using Microsoft.EntityFrameworkCore;
using ProjectToDoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ProjectToDoList.Repository
{

    public class ToDoTaskRepository : IToDoTaskRepository
    {
        private readonly ProjectTodoDBContext _context;

        public ToDoTaskRepository(ProjectTodoDBContext context)
        {
            _context = context;

        }

        public Task<IQueryable<ToDoTask>> GetToDoTasks()
        {
            return Task.Run(() => _context.ToDoTasks.AsQueryable());
        }

   

        public async Task<ToDoTask> GetToDoTask(int id)
        {
            var ToDoTask = await _context.ToDoTasks.FindAsync(id);



            return ToDoTask;
        }
        public  bool IsDuplicateTasks(string Title) 
        { 
        var ToDoTask = _context.ToDoTasks.Where(i=>i.Title==Title).FirstOrDefault();
            return ToDoTask!=null?true:false;
        }
        public async Task<ToDoTask> CreateToDoTask(ToDoTask toDoTask)
        {
          
                _context.ToDoTasks.Add(toDoTask);
                await _context.SaveChangesAsync();

           return await GetToDoTask (toDoTask.Id);
           
        }

        public async Task<ToDoTask> UpdateToDoTask(ToDoTask toDoTask)
        {
            _context.Entry(toDoTask).State = EntityState.Modified;           
            await _context.SaveChangesAsync();
            return toDoTask;
        }

        public async Task<ToDoTask> DeleteToDoTask(int id)
        {
            var toDoTask = await _context.ToDoTasks.FindAsync(id);
            if (toDoTask != null)
            {
                _context.ToDoTasks.Remove(toDoTask);
                await _context.SaveChangesAsync();              
            }  
            return toDoTask;
        }

        public bool ToDoTaskExists(int id)
        {
            return _context.ToDoTasks.Any(e => e.Id == id);
        }
    }
}
