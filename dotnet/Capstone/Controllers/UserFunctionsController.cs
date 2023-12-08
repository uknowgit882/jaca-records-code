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
    public class UserFunctionsController : CommonController
    {
        public UserFunctionsController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, ICollectionsDao collectionsDao, IFormatsDao formatsDao,
            IFriendsDao friendsDao, IGenresDao genresDao, IImagesDao imagesDao, ILabelsDao labelsDao, ILibrariesDao librariesDao,
            IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao,
            IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao)
            : base(artistsDao, barcodesDao, collectionsDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, librariesDao,
                  recordBuilderDao, recordsArtistsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao,
                  recordService, tracksDao, userDao, searchDao)
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
                return BadRequest("Something went wrong deactivating this user");
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
                // have to update is_premium in places
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
                return BadRequest("Something went wrong upgrading this user");
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
                // demote user has impacts
                // will reduce record count to the last 25 records added
                // will reduce collection to the last one created

                // get the most recent 25 records from library
                // mark them is premium false
                // add a collection "free" from collections
                // set it to is premium false
                // add the 25 to that collection to records_collections
                // set it to is premium false
                // check user role for all view library/collections 
                // in get library or get collections endpoints, do an if on the role
                // if role free, use the method that only returns is_premium false
                //List<int> freeRecords = _librariesDao(username)


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
                return BadRequest("Something went wrong downgrading this user");
            }
        }
    }
}
