using System.Collections.Generic;
using Capstone.Models;
namespace Capstone.DAO
{
    public interface ITracksDao
    {
        public bool GetTrack(Track track);
        public bool AddTrack(Track track);
    }
}
