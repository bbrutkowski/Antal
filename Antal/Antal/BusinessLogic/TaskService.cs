using Antal.BusinessLogic.Interface;
using Antal.Models;
using Antal.Models.DTO;
using Antal.Repository.Interface;

namespace Antal.BusinessLogic
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository) => _taskRepository = taskRepository;

        public async Task<List<TodoTask>> GetTasksAsync(DateTime date, CancellationToken token) => await _taskRepository.GetTasksAsync(date, token);

        public async Task<bool> AddTaskAsync(TaskDto task, CancellationToken token) => await _taskRepository.AddTaskAsync(task, token);

        public async Task<bool> DeleteTaskAsync(int id, CancellationToken token) => await _taskRepository.DeleteTaskAsync(id, token);

        public async Task<TodoTask> GetTaskByIdAsync(int id, CancellationToken token) => await _taskRepository.GetTaskByIdAsync(id, token);

        public async Task<bool> UpdateTaskAsync(TodoTask task, CancellationToken token) => await _taskRepository.UpdateTaskAsync(task, token);

        public async Task<bool> MarkAsCompletedAsync(int id, CancellationToken token) => await _taskRepository.MarkAsCompletedAsync(id, token);

        public async Task<List<TodoTask>> GetUpcomingTasksAsync(CancellationToken token) => await _taskRepository.GetUpcomingTasksAsync(token);
    }
}
