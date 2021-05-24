using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class UserGet
    {
        public int n_UserID { get; set; }
        public string s_UserCode { get; set; }
        public string s_UserName { get; set; }
        public string s_Email { get; set; }
        public string s_Token { get; set; }
    }
}