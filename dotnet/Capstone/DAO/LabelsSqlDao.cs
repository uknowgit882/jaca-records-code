using Capstone.Models;
using System;

namespace Capstone.DAO
{
    public class LabelsSqlDao : ILabelsDao
    {
        private readonly string connectionString;
        public LabelsSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public bool AddLabel()
        {
            throw new NotImplementedException();
        }
    }
}
