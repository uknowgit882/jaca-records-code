using Microsoft.AspNetCore.Mvc;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.DAO.Interfaces;
using System;
using Capstone.DAO;
using Capstone.Service;

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
        [HttpPut("userfunctions/{username}")]
        public ActionResult<string> DeactivateUser(string username)
        {
            // check if you have a valid value, and it matches the user who is logged in (for security)
            // so they are actioning their own profile
            if (string.IsNullOrEmpty(username) || User.Identity.Name != username)
            {
                // validation of the input
                // if fails, stop
                return BadRequest("You must enter a valid username");
            }

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
