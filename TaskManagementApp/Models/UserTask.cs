using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TaskManagementApp.Models
{
    public class UserTask
    {
        [Key] 
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters.")]
        public string Title { get; set; }

        public string ? Description { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; } = DateTime.Now; 

        [Required]
        [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5.")]
        public int Priority { get; set; }


        [Required(ErrorMessage = "Is Completed is required.")]

        public bool IsCompleted { get; set; } = false;

        // العلاقة مع المستخدم (كل مهمة مرتبطة بمستخدم)
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; } // تغيير هنا إلى IdentityUser
    }
}
