﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.Security.Models;

namespace Capstone.DAO
{
    public class TracksSqlDao : ITracksDao
    {
        private readonly string connectionString;

        public TracksSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Tracks AddTrack(int artistId, string title, string position, string duration)
        {
            throw new NotImplementedException();
        }
    }
}
