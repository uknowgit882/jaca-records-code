using Capstone.DAO;
using Capstone.DAO.Interfaces;
using Capstone.Models;
using Capstone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpPut("deactivate")]
        public ActionResult<string> DeactivateUser()
        {
            string username = User.Identity.Name;

            try
            {
                User user = _userDao.GetUserByUsername(username);

                if (!user.IsActive)
                {
                    return BadRequest($"{username} is already inactive");
                }
                // deactivate library, collections, record collections
                _librariesDao.DeReactivateLibrary(username, NotActive);
                _recordsCollectionsDao.DeReactivateRecordsInCollection(username, NotActive);
                _collectionsDao.DeReactivateCollection(username, NotActive);

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

        [HttpPut("upgrade")]
        public ActionResult<string> UpgradeUser()
        {
            string username = User.Identity.Name;

            try
            {
                // get user's current role
                string role = _userDao.GetUserRole(username);

                if (role == PremiumAccountName)
                {
                    return BadRequest($"{username} is already a premium user");
                }

                // have to update is_premium in places
                // upgrade all records in libraries and collections to ispremium true
                _librariesDao.ChangeAllRecordIsPremium(username, IsPremium);
                _recordsCollectionsDao.ChangeAllRecordCollectionIsPremium(username, IsPremium);

                // restore all collections to is premium true
                // get all the collections that are false
                List<Collection> collections = _collectionsDao.GetAllCollections(username);

                // then make is premium is true
                foreach (Collection collection in collections)
                {
                    _collectionsDao.ChangeCollectionIsPremium(collection.Name, username, IsPremium);
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
        [HttpPut("downgrade")]
        public ActionResult<string> DowngradeUser()
        {
            string username = User.Identity.Name;

            try
            {
                // get user's current role
                string role = _userDao.GetUserRole(username);

                if (role == FreeAccountName)
                {
                    return BadRequest($"{username} is already a free user");
                }

                // demote user has impacts
                // will reduce record count to the last 25 records added
                // will reduce collection to the last one created

                // get the most recent 25 records from library
                List<Library> freeRecords = _librariesDao.Get25MostRecentDiscogsIdsInLibrary(username);

                // mark just them as is premium false
                foreach (Library record in freeRecords)
                {
                    _librariesDao.ChangeRecordIsPremium(record.Discog_Id, username, NotPremium);
                    // and also in records collections
                    _recordsCollectionsDao.ChangeSingleRecordCollectionIsPremium(record.Discog_Id, username, NotPremium);
                }

                // check if user has a free collection
                Collection existingFreeCollection = _collectionsDao.GetNamedCollection(username, "Free Collection");
                if (existingFreeCollection == null)
                {
                    // add a collection "free" from collections
                    _collectionsDao.AddCollection(username, "Free Collection",NotPrivate, NotPremium);
                } else
                {
                    _collectionsDao.ChangeCollectionIsPremium(existingFreeCollection.Name, username, NotPremium);
                }

                // set it to is premium false
                //_collectionsDao.ChangeCollectionIsPremium("Free Collection", username, NotPremium);

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
