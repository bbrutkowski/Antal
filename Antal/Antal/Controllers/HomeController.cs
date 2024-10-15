using Antal.BusinessLogic.Interface;
using Antal.Models;
using Antal.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Antal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskService _todoTaskService;

        public HomeController(ITaskService todoTaskService) => _todoTaskService = todoTaskService;

        public async Task<IActionResult> Index(DateTime? date, CancellationToken token)
        {
            if (!date.HasValue) date = DateTime.Now;

            var tasks = await _todoTaskService.GetTasksAsync(date.Value, token);
            var upcomingTasks = await _todoTaskService.GetUpcomingTasksAsync(token);

            ViewBag.SelectedDate = date.Value.ToString("yyyy-MM-dd");
            ViewBag.UpcomingTasks = upcomingTasks; 

            return View(tasks);
        }

        public IActionResult AddTask(DateTime? date)
        {
            var selectedDate = date ?? DateTime.Now;

            var task = new TaskDto { Date = selectedDate };

            return View(task); 
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskDto task, CancellationToken token)
        {
            if (!ModelState.IsValid) return View(task);

            var result = await _todoTaskService.AddTaskAsync(task, token);
            if (result) return RedirectToAction("Index", new { date = task.Date });

            ModelState.AddModelError("", "An error occurred while saving task");
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            var task = await _todoTaskService.GetTaskByIdAsync(id, token);
            if (task is null) return NotFound();

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken token)
        {
            var result = await _todoTaskService.DeleteTaskAsync(id, token);
            if (!result) TempData["Error"] = "Failed to delete the task.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id, CancellationToken token)
        {
            var task = await _todoTaskService.GetTaskByIdAsync(id, token);
            if (task is null) return NotFound();

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoTask task, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Model state is not valid.";
                return View(task);
            }

            var result = await _todoTaskService.UpdateTaskAsync(task, token);
            if (!result)
            {
                TempData["Error"] = "Failed to update the task.";
                return View(task);
            }

            return RedirectToAction(nameof(Index), new { date = DateTime.Now });
        }

        [HttpGet]
        public async Task<IActionResult> MarkAsCompleted(int id, CancellationToken token)
        {
            var task = await _todoTaskService.GetTaskByIdAsync(id, token);
            if (task is null) return NotFound();

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteConfirmed(int id, CancellationToken token)
        {
            var result = await _todoTaskService.MarkAsCompletedAsync(id, token);
            if (!result) TempData["Error"] = "Failed to mark task as completed.";

            return RedirectToAction(nameof(Index));
        }
    }
}
