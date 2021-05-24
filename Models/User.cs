using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class User
    {   
        // public User(int n_UserID, string s_UserCode, string s_UserName, string s_Email)
        // {
        //     this.n_UserID = n_UserID;
        //     this.s_UserCode = s_UserCode;
        //     this.s_UserName = s_UserName;
        //     this.s_Email = s_Email;
        // }

        [Key]
        public int n_UserID { get; set; }

        [Required]
        [MaxLength(10)]
        public string s_UserCode { get; set; }

        [Required]
        [MaxLength(25)]
        public string s_UserName { get; set; }

        [MaxLength(100)]
        public string s_Email { get; set; }

        [Required]
        public int n_UserType { get; set; }

        [Required]
        public DateTime d_CreatedDateTime { get; set; }

        [Required]
        public string s_Password { get; set; }

        public string s_Token { get; set; }
    }
}