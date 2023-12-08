using Microsoft.AspNetCore.Mvc;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.DAO.Interfaces;
using System;
using Capstone.DAO;
using Capstone.Service;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AseelTestController : CommonController
    {
        public AseelTestController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, IFormatsDao formatsDao, IFriendsDao friendsDao, IGenresDao genresDao, IImagesDao imagesDao, ILabelsDao labelsDao, IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao, IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao, IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao) : base(artistsDao, barcodesDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, recordBuilderDao, recordsArtistsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao, recordService, tracksDao, userDao, searchDao)
        {
        }
        // Sample, please edit as needed
        [HttpGet("library")]
        public ActionResult<List<RecordTableData>> GetUserLibrary()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Please provide a valid username");
            }
            username = "aseelt"; // TODO remove after test


            try
            {
                
                bool output = _userDao.DeactivateUser(username);
                if (output)
                {
                    return Ok($"{username} was deactivated");

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
