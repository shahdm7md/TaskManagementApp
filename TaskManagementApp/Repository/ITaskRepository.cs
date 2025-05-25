using TaskManagementApp.Models;

namespace TaskManagementApp.Repository
{
    public interface ITaskRepository
    {
        public void Insert(UserTask task);
        public List<UserTask> GetAll();
        public UserTask GetById(int Id);
        public List<UserTask> GetTasksByUserId(string userId);
        public void Update(int Id, UserTask task);
        public void Delete(int Id);

        
    }
}
