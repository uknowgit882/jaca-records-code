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
    public class UserFunctionsController : CommonController
    {
        public UserFunctionsController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, IFormatsDao formatsDao, IFriendsDao friendsDao, IGenresDao genresDao, IImagesDao imagesDao, ILabelsDao labelsDao, IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao, IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao, IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao) : base(artistsDao, barcodesDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, recordBuilderDao, recordsArtistsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao, recordService, tracksDao, userDao, searchDao)
        {
        }

        [HttpPut("userfunctions/deactivate/{username}")]
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

        [HttpPut("userfunctions/upgrade/{username}")]
        public ActionResult<string> UpgradeUser(string username)
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
                bool output = _userDao.UpgradeUser(username);
                if (output)
                {
                    return Ok($"{username} was upgraded");

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
        [HttpPut("userfunctions/downgrade/{username}")]
        public ActionResult<string> DowngradeUser(string username)
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
                bool output = _userDao.DowngradeUser(username);
                if (output)
                {
                    return Ok($"{username} was downgraded");

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
