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
                x => x.s_UserCode == userLogin.s_UserCode && x.s_Password == userLogin.s_Password
            );

            return user;
        }
    }
}