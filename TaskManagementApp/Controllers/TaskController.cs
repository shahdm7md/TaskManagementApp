using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Models;
using TaskManagementApp.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace TaskManagementApp.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskRepository taskRepo;
        private readonly UserManager<IdentityUser> userManager;
        public TaskController(ITaskRepository taskRepo, UserManager<IdentityUser> userManager)
        {
            this.taskRepo = taskRepo;
            this.userManager = userManager;

        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }
            var tasks = taskRepo.GetTasksByUserId(user.Id);

            return View(tasks);
        }

        [HttpPost]
        public IActionResult ToggleCompletion(int id, bool isCompleted)
        {
            var task = taskRepo.GetById(id);
            if (task != null)
            {
                task.IsCompleted = isCompleted;
                taskRepo.Update(id, task);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTask(UserTask newTask)
        {
            if (ModelState.IsValid)
            {
                if (newTask.DueDate <= DateTime.Now)
                {
                    ModelState.AddModelError("DueDate", "Due date must be in the future.");
                    return View(newTask);
                }
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(); 
                }
                newTask.UserId = user.Id;
                taskRepo.Insert(newTask);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("UserId", "User Id Not Valid");
                return View(newTask);
            }
        }
        public IActionResult Edit(int Id)
        {
            UserTask task = taskRepo.GetById(Id);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, UserTask newTask)
        {
            if (ModelState.IsValid)
            {
                if (newTask.DueDate <= DateTime.Now)
                {
                    ModelState.AddModelError("DueDate", "Due date must be in the future.");
                    return View(newTask);
                }

                taskRepo.Update(Id, newTask); 

                return RedirectToAction("Index"); 
            }
            return View(newTask);
        }

        public IActionResult Details(int Id)
        {
            return View(taskRepo.GetById(Id));
        }
        public IActionResult Delete(int Id)
        {
            taskRepo.Delete(Id);
            return RedirectToAction("Index");
        }

        public IActionResult ConfirmDelete(int Id)
        {

            return View(Id);
        }
    }
}
