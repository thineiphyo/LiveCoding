using ProjectToDoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectToDoList.Repository
{
    public interface IToDoTaskRepository
    {
        public Task<IQueryable<ToDoTask>> GetToDoTasks();
        public  Task<ToDoTask> GetToDoTask(int id);
        public bool IsDuplicateTasks(string Title);

        public Task<ToDoTask> CreateToDoTask(ToDoTask toDoTask);
        public  Task<ToDoTask> DeleteToDoTask(int id);
        public  Task<ToDoTask> UpdateToDoTask(ToDoTask toDoTask);
        public bool ToDoTaskExists(int id);
    }
}
