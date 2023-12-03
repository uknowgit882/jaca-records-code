using System.Collections.Generic;
using Capstone.Models;
namespace Capstone.DAO
{
    public interface ITracksDao
    {
        bool AddTrack(Track track);
    }
}
