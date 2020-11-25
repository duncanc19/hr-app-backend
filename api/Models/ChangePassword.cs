using System.ComponentModel.DataAnnotations;

namespace HRApp.API.Models
{
    public class ChangePassword 
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}