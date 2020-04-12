using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
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
        [EnableQuery(AllowedLogicalOperators = AllowedLogicalOperators.All)]
        //Get Specific Todo
        // http://localhost:50231/api/ToDoTask?$filter=id eq 4
        //Today
        //http://localhost:50231/api/ToDoTask?$filter=expireDate eq 2020-10-03
        //week
        //http://localhost:50231/api/ToDoTask?$filter=expireDate gt 2020-10-03 and expireDate lt 2020-10-06
        public async Task<IQueryable<ToDoTask>> GetToDoList()
        {
            return  await  _ToDoTaskRepository.GetToDoTasks();
        }

       // [HttpPut("{id}")]
       [HttpPut]
        public async Task<IActionResult> PutToDoTask(int Id, ToDoTask toDoTask)
        {
            if (Id != toDoTask.Id)
            {
                return BadRequest();
            }
            var res= await  _ToDoTaskRepository.UpdateToDoTask(toDoTask);   
            return Ok(res);
        }

        [HttpPut("{id}/CompletePercent/{completePercent}")]
        public async Task<IActionResult> PutToDoTaskPercent(int Id, int CompletePercent)
        {
            if (CompletePercent<0 || CompletePercent>100)
            {
                ModelState.AddModelError("CompletePercent", "Invalid Input");
                return BadRequest(ModelState);
            }
            var toDoTask = await _ToDoTaskRepository.GetToDoTask(Id);
            toDoTask.CompletePercent = CompletePercent;
            toDoTask.IsCompleted = false;
            if (CompletePercent == 100)
            {
                toDoTask.IsCompleted = true;
            }
            var res = await _ToDoTaskRepository.UpdateToDoTask(toDoTask);
            return Ok(res);
        }
        

        [HttpPost]
        // date format is ISO date "expireDate": "2020-04-15T00:00:00",
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

      
       // [HttpDelete("{id}")]
       [HttpDelete]
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
