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
            IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsCollectionsDao recordsCollectionsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao,
            IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao)
            : base(artistsDao, barcodesDao, collectionsDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, librariesDao,
                  recordBuilderDao, recordsArtistsDao, recordsCollectionsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao,
                  recordService, tracksDao, userDao, searchDao)
        {
        }

        [HttpPut("deactivate/{username}")]
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
                // deactivate library, collections, record collections
                _librariesDao.DeReactivateLibrary(username, false);
                _collectionsDao.DeReactivateCollection(username, false);
                _recordsCollectionsDao.DeReactivateRecordsInCollection(username, false);

                // then deactivate the user
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

        [HttpPut("upgrade/{username}")]
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
                // upgrade all records in libraries and collections to ispremium true
                _librariesDao.ChangeAllRecordIsPremium(username, true);
                _recordsCollectionsDao.ChangeAllRecordCollectionIsPremium(username, true);

                // restore all collections to is premium true
                // get all the collections that are false
                List<Collection> collections = _collectionsDao.GetAllCollections(username);

                // then make is premium is true
                foreach(Collection collection in collections)
                {
                    _collectionsDao.ChangeCollectionIsPremium(collection.Name, username, true);
                }

                // then upgrade
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
        [HttpPut("downgrade/{username}")]
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
                List<Library> freeRecords = _librariesDao.Get25MostRecentDiscogsIdsInLibrary(username);
                
                // mark just them as is premium false
                foreach(Library record in freeRecords)
                {
                    _librariesDao.ChangeRecordIsPremium(record.Discog_Id, username, false);
                    // and also in records collections
                    _recordsCollectionsDao.ChangeSingleRecordCollectionIsPremium(record.Discog_Id, username, false);
                }

                // add a collection "free" from collections
                _collectionsDao.AddCollection(username, "Free Collection");

                // set it to is premium false
                _collectionsDao.ChangeCollectionIsPremium("Free Collection", username, false);

                // then downgrade the user
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
