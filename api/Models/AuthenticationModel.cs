using System.ComponentModel.DataAnnotations;

namespace HRApp.API.Models
{
    public class AuthenticationModel
    {
        [Required]
        public string Username {get; set;}

        [Required]
        public string Password { get; set; }

    }    

}