using UserManagement.Models;
using UserManagement.Data;
using System.Collections.Generic;

namespace UserManagement.Data
{
    public class MockUserManagement : IUserManagement
    {
        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            //     new User(1, "UserA", "User A Name", "usera@gmail.com"),
            //     new User(2, "UserB", "User B Name", "userb@gmail.com"),
            //     new User(3, "UserC", "User C Name", "userc@gmail.com"),
            //     new User(4, "UserD", "User D Name", "userd@gmail.com")
            // };
            return users;
        }

        public User GetUserById(int id)
        {
            return null;
            // return new User(2, "UserB", "User B Name", "userb@gmail.com");
        }

        public void CreateUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveChanges() => throw new System.NotImplementedException();

        public void DeleteUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public bool ValidateCredentials(string userName, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}