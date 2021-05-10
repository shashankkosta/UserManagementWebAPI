using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class UserLogin
    {
        [Required]
        public string s_UserCode { get; set; }

        [Required]
        public string s_Password { get; set; }
    }
}