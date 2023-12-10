using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.DAO.Interfaces
{
    public interface ILabelsDao
    {
        public Label GetLabel(Label label);
        public bool AddLabel(Label label);
        public List<Label> GetLabelsByDiscogsId(int discogId);
        public int GetLabelCountByUsername(string username, bool isPremium);
        public int GetLabelCount();
        public Dictionary<string, int> GetLabelAndRecordCountByUsername(string username, bool isPremium);
        public Dictionary<string, int> GetLabelAndRecordCount();

        public Label UpdateLabel(Label updatedLabel);
    }
}
