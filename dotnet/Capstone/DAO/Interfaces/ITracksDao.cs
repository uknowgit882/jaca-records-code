using System.Collections.Generic;
using Capstone.Models;
namespace Capstone.DAO
{
    public interface ITracksDao
    {
        public Track GetTrack(Track track);
        public bool AddTrack(Track track);
        public Track UpdateTrack(Track updatedTrack);
        public List<Track> GetTracksByDiscogsIdAndUsername(int discogId, string username);
    }
}
