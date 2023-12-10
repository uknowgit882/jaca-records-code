using System.Collections.Generic;
using Capstone.Models;
namespace Capstone.DAO
{
    public interface ITracksDao
    {
        public Track GetTrack(Track track);
        public List<Track> GetTracksByDiscogsId(int discogId);
        public int GetTrackCountByUsername(string username, bool isPremium);
        public int GetTrackCount();
        public bool AddTrack(Track track);
        public Track UpdateTrack(Track updatedTrack);
    }
}
