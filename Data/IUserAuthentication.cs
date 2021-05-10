using UserManagement.Models;

namespace UserManagement.Data
{
    public interface IUserAuthentication
    {
        User AuthenticateUser(UserLogin userLogin);
    }
}