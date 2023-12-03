using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.Security.Models;

namespace Capstone.DAO
{
    public class GenreSqlDao : IGenresDao
    {
        private readonly string connectionString;

        public GenreSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        bool AddGenre(string genre)
        {
            throw new NotImplementedException();
        }
    }
}
