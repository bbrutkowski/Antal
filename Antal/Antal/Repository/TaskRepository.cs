using Antal.Context;
using Antal.Models;
using Antal.Models.DTO;
using Antal.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Antal.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DatabaseContext _context;

        public TaskRepository(DatabaseContext databaseContext) => _context = databaseContext;

        public async Task<List<TodoTask>> GetTasksAsync(DateTime date, CancellationToken token)
        {
            return await _context.TodoItems
                .AsNoTracking()
                .Where(x => x.DueDate.Date == date.Date)
                .ToListAsync(token);
        }

        public async Task<bool> AddTaskAsync(TaskDto task, CancellationToken token)
        {
            try
            {
                var todoTask = new TodoTask
                {
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.Date,
                    IsCompleted = false
                };

                await _context.TodoItems.AddAsync(todoTask, token);
                var changes = await _context.SaveChangesAsync(token);

                return changes > 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTaskAsync(int id, CancellationToken token)
        {
            try
            {
                var task = await _context.TodoItems.FindAsync(id, token);
                if (task is null) return false;

                _context.TodoItems.Remove(task);
                var changes = await _context.SaveChangesAsync(token);

                return changes > 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<TodoTask> GetTaskByIdAsync(int id, CancellationToken token)
        {
            return await _context.TodoItems.FindAsync(id, token) ?? new();
        }

        public async Task<bool> UpdateTaskAsync(TodoTask task, CancellationToken token)
        {
            try
            {
                var existingTask = await GetTaskByIdAsync(task.Id, token);
                if (existingTask is null) return false;

                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;

                var changes = await _context.SaveChangesAsync(token);
                return changes > 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> MarkAsCompletedAsync(int id, CancellationToken token)
        {
            var task = await GetTaskByIdAsync(id, token);

            task.IsCompleted = true;

            var changes = await _context.SaveChangesAsync(token);
            return changes > 0 ? true : false;
        }

        public async Task<List<TodoTask>> GetUpcomingTasksAsync(CancellationToken token)
        {
            var upcomingDate = DateTime.Now.AddDays(1).Date; 

            return await _context.TodoItems
                .Where(task => task.DueDate.Date == upcomingDate)
                .ToListAsync(token);
        }
    }
}
