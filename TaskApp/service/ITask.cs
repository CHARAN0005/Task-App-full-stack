using Microsoft.AspNetCore.Mvc;
using TaskModels;
namespace TaskService;

public interface ITaskService
{
    public Task<IEnumerable<TaskModel>> getAll();
    public Task<TaskModel> getBy(int id);
    public Task<TaskModel> create(TaskCreate dto);
    public Task<TaskModel> update(TaskUpdate dto);
    public Task<bool> delete(int id);

}