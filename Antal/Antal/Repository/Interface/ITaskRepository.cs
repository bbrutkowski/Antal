using Antal.Models;
using Antal.Models.DTO;

namespace Antal.Repository.Interface
{
    public interface ITaskRepository
    {
        Task<List<TodoTask>> GetTasksAsync(DateTime date, CancellationToken token);
        Task<bool> AddTaskAsync(TaskDto task, CancellationToken token);
        Task<bool> DeleteTaskAsync(int id, CancellationToken token);
        Task<TodoTask> GetTaskByIdAsync(int id, CancellationToken token);
        Task<bool> UpdateTaskAsync(TodoTask task, CancellationToken token);
        Task<bool> MarkAsCompletedAsync(int id, CancellationToken token);
        Task<List<TodoTask>> GetUpcomingTasksAsync(CancellationToken token);
    }
}
