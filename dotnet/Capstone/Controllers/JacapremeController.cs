using Microsoft.AspNetCore.Mvc;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.DAO.Interfaces;
using System;
using Capstone.DAO;
using Capstone.Service;
using Microsoft.AspNetCore.Authorization;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "jacapreme")]
    [ApiController]
    public class JacapremeController : CommonController
    {
        public JacapremeController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, ICollectionsDao collectionsDao, IFormatsDao formatsDao,
            IFriendsDao friendsDao, IGenresDao genresDao, IImagesDao imagesDao, ILabelsDao labelsDao, ILibrariesDao librariesDao,
            IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsCollectionsDao recordsCollectionsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao,
            IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao)
            : base(artistsDao, barcodesDao, collectionsDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, librariesDao,
                  recordBuilderDao, recordsArtistsDao, recordsCollectionsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao,
                  recordService, tracksDao, userDao, searchDao)
        {
        }
        
        // only admins should be able to reactivate
        [HttpPut("reactivate/{username}")]
        public ActionResult<string> ReactivateUser(string username)
        {
            // don't need authentication that it matches the user to action
            // we're the admins, it won't
            if (string.IsNullOrEmpty(username))
            {
                // validation of the input
                // if fails, stop
                return BadRequest("You must enter a valid username");
            }

            try
            {
                // check if the user is inactive
                User user = _userDao.GetUserByUsername(username);

                if (user.IsActive)
                {
                    return BadRequest($"{username} is already active");
                }

                // reactivate collections, library, records_collections
                _librariesDao.DeReactivateLibrary(username, IsActive);
                _recordsCollectionsDao.DeReactivateRecordsInCollection(username, IsActive);
                _collectionsDao.DeReactivateCollection(username, IsActive);
                
                // then reactivate the user
                bool output = _userDao.ReactivateUser(username);
                if (output)
                {
                    return Ok($"{username} was reactivated");

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong reactivating this user");
            }
        }
        [HttpPut("upgradetoadmin/{username}")]
        public ActionResult<string> UpgradeUserToAdmin(string username)
        {
            // don't need authentication that it matches the user to action
            // we're the admins, it won't
            if (string.IsNullOrEmpty(username))
            {
                // validation of the input
                // if fails, stop
                return BadRequest("You must enter a valid username");
            }

            try
            {
                // check if the user is already an admin
                User user = _userDao.GetUserByUsername(username);

                if (user.Role == AdminAccountName)
                {
                    return BadRequest($"{username} is already one of us");
                }

                bool output = _userDao.UpgradeAdmin(username);
                if (output)
                {
                    return Ok($"{username} was upgraded to the Jacapreme crew. Welcome!");

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong upgrading this user to an admin");
            }
        }
        [HttpPut("downgradefromadmin/{username}")]
        public ActionResult<string> DowngradeAdmin(string username)
        {
            // don't need authentication that it matches the user to action
            // we're the admins, it won't
            if (string.IsNullOrEmpty(username))
            {
                // validation of the input
                // if fails, stop
                return BadRequest("You must enter a valid username");
            }

            try
            {
                // check if the user is already an admin
                User user = _userDao.GetUserByUsername(username);

                if (user.Role == PremiumAccountName || user.Role == FreeAccountName)
                {
                    return BadRequest($"{username} isn't one of us");
                }

                bool output = _userDao.DowngradeAdmin(username);
                if (output)
                {
                    return Ok($"{username} was kicked out of the Jacapreme crew. Good riddance!");

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong upgrading this user to an admin");
            }
        }
    }
}
