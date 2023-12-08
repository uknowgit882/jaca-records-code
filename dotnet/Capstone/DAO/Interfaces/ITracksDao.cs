using System.Collections.Generic;
using Capstone.Models;
namespace Capstone.DAO
{
    public interface ITracksDao
    {
        public Track GetTrack(Track track);
        public List<Track> GetTracksByDiscogsId(int discogId);
        public int GetTrackCountByUsername(string username);
        public int GetTrackCount();
        //public List<Track> GetTracksByDiscogsIdAndUsername(int discogId, string username); // might not need this
        public bool AddTrack(Track track);
        public Track UpdateTrack(Track updatedTrack);
    }
}
