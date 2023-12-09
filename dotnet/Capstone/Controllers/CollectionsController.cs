using Microsoft.AspNetCore.Mvc;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.DAO.Interfaces;
using System;
using Capstone.DAO;
using Capstone.Service;
using Capstone.Utils;
using System.Reflection;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CollectionsController : CommonController
    {
        public CollectionsController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, ICollectionsDao collectionsDao, IFormatsDao formatsDao,
            IFriendsDao friendsDao, IGenresDao genresDao, IImagesDao imagesDao, ILabelsDao labelsDao, ILibrariesDao librariesDao,
            IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsCollectionsDao recordsCollectionsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao,
            IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao)
            : base(artistsDao, barcodesDao, collectionsDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, librariesDao,
                  recordBuilderDao, recordsArtistsDao, recordsCollectionsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao,
                  recordService, tracksDao, userDao, searchDao)
        {
        }

        /// <summary>
        /// Gets all the records for a user. Intended for a user to see all their collections in one place, regardless of public/private
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Collection>> GetAllCollectionsByRoleForUser()
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                // find the username
                string userRole = _userDao.GetUserRole(username);

                List<Collection> output = new List<Collection>();

                // get the collections by user role
                // free should only get <25 back
                if (userRole == "free")
                {
                    output = _collectionsDao.GetAllCollectionsByRole(username, false);
                }
                else
                {
                    output = _collectionsDao.GetAllCollectionsByRole(username, true);
                }

                if (output.Count != 0)
                {
                    return Ok(output);

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to do return all collection information", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        
        /// <summary>
        /// Gets a named collection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("named/{name}")]
        public ActionResult<Collection> GetNamedCollection(string name)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                string userRole = _userDao.GetUserRole(username);

                Collection output = null;


                if (userRole == "free")
                {
                    output = _collectionsDao.GetNamedCollection(username, name, false);
                }
                else
                {
                    output = _collectionsDao.GetNamedCollection(username, name, true);
                }

                if (output != null)
                {
                    return Ok(output);

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to do return all collection information", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets records for a user based on the privacy option selected. Can be used to display only public collections
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("filterprivacy")]
        public ActionResult<List<Collection>> GetAllCollectionsByRoleAndPrivacyForUser(IncomingCollectionRequest request)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                string userRole = _userDao.GetUserRole(username);

                List<Collection> output = new List<Collection>();

                if (userRole == "free")
                {
                    output = _collectionsDao.GetPubOrPrivAllCollections(username, false, request.Is_Private);
                }
                else
                {
                    output = _collectionsDao.GetPubOrPrivAllCollections(username, true, request.Is_Private);
                }

                if (output.Count != 0)
                {
                    return Ok(output);

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to do return all collection information", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a named collection, toggle on public/private
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("filterprivacy/named")]
        public ActionResult<Collection> GetNamedCollectionPubOrPriv(IncomingCollectionRequest request)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                string userRole = _userDao.GetUserRole(username);

                Collection output = null;


                if (userRole == "free")
                {
                    output = _collectionsDao.GetPubOrPrivCollection(username, request.Name, false, request.Is_Private);
                }
                else
                {
                    output = _collectionsDao.GetPubOrPrivCollection(username, request.Name, true, request.Is_Private);
                }

                if (output != null)
                {
                    return Ok(output);

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to do return all collection information", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
