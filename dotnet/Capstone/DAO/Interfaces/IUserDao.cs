using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.DAO.Interfaces
{
    public interface IUserDao
    {
        public IList<User> GetUsers();
        public User GetUserById(int id);
        public User GetUserByUsername(string username);
        public string GetUserRole(string username); // don't expose in end point
        public bool GetUserStatus(string username); // don't expose in end point
        public int GetUserCount();
        public User CreateUser(RegisterUser userParam);
        public bool DeactivateUser(string username);
        public bool ReactivateUser(string username);
        public bool UpgradeUser(string username);
        public bool DowngradeUser(string username);
        public bool UpgradeAdmin(string username);
        public bool DowngradeAdmin(string username);
        public bool UpdateLastLogin(string username); // don't expose in end point

    }
}
