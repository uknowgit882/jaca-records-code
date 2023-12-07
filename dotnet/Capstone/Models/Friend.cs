using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Capstone.Models
{
    /// <summary>
    /// Holds user and their friends
    /// </summary>
    public class UserAndFriends
    {
        [JsonIgnore]
        public int User_Id { get; set; }
        public string Username { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        [JsonIgnore]
        public string Email_Address { get; set; }
        public List<Friend> Friends { get; set; } = new List<Friend>();
        public int Friend_Count
        {
            get
            {
                return Friends.Count;
            }
        }
        public override string ToString()
        {
            return Username + " and their " + Friends.Count + " friend(s)";
        }
    }
    /// <summary>
    /// Holds friend details retrieved from the database
    /// </summary>
    public class Friend
    {
        [JsonIgnore]
        public int Friends_Id { get; set; }
        public string Friends_Username { get; set; }
        public string Friends_First_Name { get; set; }
        public string Friends_Last_Name { get; set; }
        [JsonIgnore]
        public string Friends_Email_Address { get; set; }
        [JsonIgnore]
        public bool Friend_Is_Active { get; set; }

        public override string ToString()
        {
            return Friends_Username;
        }
    }
}
