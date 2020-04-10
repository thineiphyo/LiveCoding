using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectToDoList.Models;
using ProjectToDoList.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   
    public class ToDoTaskController : ControllerBase
    {
        // GET: /<controller>/
        private IToDoTaskRepository _ToDoTaskRepository;     

        public ToDoTaskController(IToDoTaskRepository ToDoTaskRepository)
        {
            _ToDoTaskRepository = ToDoTaskRepository;
      
        }
     
        [HttpGet]
        public async Task<IQueryable<ToDoTask>> GetToDoList()
        {
            return  await  _ToDoTaskRepository.GetToDoTasks();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTaskDetail(int Id, ToDoTask toDoTask)
        {
            if (Id != toDoTask.Id)
            {
                return BadRequest();
            }
            var res= await  _ToDoTaskRepository.UpdateToDoTask(toDoTask);   
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTaskDetail([FromBody] ToDoTask toDoTask)
        {
         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isDuplicate =  _ToDoTaskRepository.IsDuplicateTasks(toDoTask.Title);
            if (isDuplicate) 
            {
                ModelState.AddModelError("Title", "Duplicate task name");
                return BadRequest(ModelState);
            }

            var insertedToDoTask= await _ToDoTaskRepository.CreateToDoTask(toDoTask);
            return Ok(insertedToDoTask); 
        }

      
        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDoTask>> DeleteToDoTask(int id)
        {
           if( _ToDoTaskRepository.ToDoTaskExists(id))
            {
                var res = await _ToDoTaskRepository.DeleteToDoTask(id);
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        
        }

    
    }
}
