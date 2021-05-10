using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class UserPost
    {
        [Required]
        [MaxLength(10)]
        public string s_UserCode { get; set; }

        [Required]
        [MaxLength(25)]
        public string s_UserName { get; set; }

        [MaxLength(100)]
        public string s_Email { get; set; }

        [Required]
        public string s_Password { get; set; }
    }
}