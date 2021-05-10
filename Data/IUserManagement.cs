using UserManagement.Models;
using System.Collections.Generic;

namespace UserManagement.Data
{
    public interface IUserManagement
    {
        List<User> GetAllUsers();

        User GetUserById(int id);

        bool SaveChanges();

        void CreateUser(User user);

        void DeleteUser(User user);

        void UpdateUser(User user);
    }
}