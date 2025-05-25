using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext context;

        public TaskRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Delete(int Id)
        {
            UserTask task= GetById(Id);
            if (task != null)
            {
                context.Remove(task);
                context.SaveChanges();
            }
            else
                throw new NullReferenceException();
        }

        public List<UserTask> GetAll()
        {
            return context.Tasks.ToList();
        }

        public UserTask GetById(int Id)
        {
            return context.Tasks.FirstOrDefault(t=>t.TaskId==Id);
        }
        public List<UserTask> GetTasksByUserId(string userId)
        {
            return context.Tasks
                          .Where(t => t.UserId == userId)
                          .OrderBy(t => t.IsCompleted) 
                          .ThenBy(t => t.Priority) 
                          .ThenBy(t =>t.DueDate) 
                          .ToList();
        }
        public void Insert(UserTask task)
        {
            context.Add(task);
            context.SaveChanges();
        }

        public void Update(int Id, UserTask task)
        {
            UserTask oldtask = GetById(Id);
            if (oldtask != null)
            {
                oldtask.Title = task.Title;
                oldtask.DueDate = task.DueDate;
                oldtask.Description = task.Description;
                oldtask.IsCompleted = task.IsCompleted;
                oldtask.Priority = task.Priority;

                context.SaveChanges();
            }
            else
            {
                throw new Exception($"Task with Id {Id} not found.");
            }
        }
    }
}
