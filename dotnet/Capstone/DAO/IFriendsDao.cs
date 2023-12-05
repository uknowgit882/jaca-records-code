using Capstone.Models;

namespace Capstone.DAO
{
    public interface IFriendsDao
    {
        public UserAndFriends GetUsersFriendsById(int id);
        public UserAndFriends GetUsersFriendsByUsername(string username);
        public UserAndFriends AddFriend(int usersId, int friendsId);
        public UserAndFriends DropFriend(int usersId, int friendsId);
    }
}
