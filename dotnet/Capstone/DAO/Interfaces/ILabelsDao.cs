using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.DAO.Interfaces
{
    public interface ILabelsDao
    {
        public Label GetLabel(Label label);
        public bool AddLabel(Label label);
        public List<Label> GetLabelsByDiscogsId(int discogId);
        public int GetLabelCountByUsername(string username);
        public int GetLabelCount();
        public Dictionary<string, int> GetLabelAndRecordCountByUsername(string username);
        public Dictionary<string, int> GetGenreAndRecordCount();

        //public List<Label> GetLabelsByDiscogsIdAndUsername(int discogId, string username); // might not need
        public Label UpdateLabel(Label updatedLabel);
    }
}
