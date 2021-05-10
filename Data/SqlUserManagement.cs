using System;
using System.Collections.Generic;
using UserManagement.Models;
using System.Linq;

namespace UserManagement.Data
{
    public class SqlUserManagement : IUserManagement
    {
        UserManagementDbContext _context;

        public SqlUserManagement(UserManagementDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _context.tb_Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.tb_Users.FirstOrDefault(x => x.n_UserID == id);
        }

        public void CreateUser(User user)
        {
            _context.tb_Users.Add(user);
        }

        public bool SaveChanges()
        {
            if (_context.SaveChanges() > 0)
                return true;
            
            return false;
        }

        public void DeleteUser(User user)
        {
            _context.tb_Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            _context.tb_Users.Update(user);
        }
    }
}