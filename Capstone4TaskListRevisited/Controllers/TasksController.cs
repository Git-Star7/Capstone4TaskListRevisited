using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Capstone4TaskListRevisited.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Capstone4TaskListRevisited.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly TaskListDbContext _context;
        public TasksController(TaskListDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //gets the id (or primary key) of the user currently logged in
            //HINT: ONLY USE THIS IN AUTHORIZED VIEWS (or include validation to be sure the id is not null)
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Tasks> thisEmployeesTasks = _context.Tasks.Where(x => x.EmployeeId == id).ToList();
            return View(thisEmployeesTasks);
        }
        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTask(Tasks newTask)
        {
            //sets the EmployeeID of the newTask to the user that is signed in
            newTask.EmployeeId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(ModelState.IsValid)
            {
                _context.Tasks.Add(newTask);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult ChangeStatus(int id)
        {
            Tasks found = _context.Tasks.Find(id);
            if (found != null)
            {
                //Change the things!
                found.Complete = !found.Complete;
                //modify the state of this entry in the database
                _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(found);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteTask(int id)
        {
            Tasks found = _context.Tasks.Find(id);
            if (found != null)
            {
                _context.Tasks.Remove(found);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult SortBy(int id)
        {
            string id2 = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Tasks> thisEmployeesTasks = _context.Tasks.Where(x => x.EmployeeId == id2).ToList();

            if(id == 1)
            {
                List<Tasks> sortedList = thisEmployeesTasks.OrderBy(Tasks => Tasks.TaskName).ToList();
                return View("Index", sortedList);
            }
            else if(id == 2)
            {
                List<Tasks> sortedList = thisEmployeesTasks.OrderBy(Tasks => Tasks.DueDate).ToList();
                return View("Index", sortedList);
            }
            else if (id == 3)
            {
                List<Tasks> sortedList = thisEmployeesTasks.OrderBy(Tasks => Tasks.Complete).ToList();
                return View("Index", sortedList);
            }
            return RedirectToAction("Index");
        }
        public IActionResult FindTasks(string word)
        {
            string id2 = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Tasks> thisEmployeesTasks = _context.Tasks.Where(x => x.EmployeeId == id2).ToList();

            if(word != null)
            {
                List<Tasks> wordTasks = thisEmployeesTasks.Where(Tasks => Tasks.TaskName.Contains(word)).ToList();
                return View("Index", wordTasks);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}