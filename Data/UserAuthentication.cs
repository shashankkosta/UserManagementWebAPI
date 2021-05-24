using System.Linq;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class UserAuthentication : IUserAuthentication
    {
        private readonly UserManagementDbContext _context;

        public UserAuthentication(UserManagementDbContext context)
        {
            _context = context;
        }

        public User AuthenticateUser(UserLogin userLogin)
        {
            var user = _context.tb_Users.FirstOrDefault(
                x => x.s_UserCode.ToUpper() == userLogin.s_UserCode.ToUpper() 
                && x.s_Password.ToUpper() == userLogin.s_Password.ToUpper()
            );

            return user;
        }

        public User ValidateToken(string token)
        {
            var user = _context.tb_Users.FirstOrDefault(x => x.s_Token == token);
            return user;
        }
    }
}