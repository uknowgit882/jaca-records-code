using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.DAO
{
    public interface ILabelsDao
    {
        bool AddLabel(Label label);
    }
}
