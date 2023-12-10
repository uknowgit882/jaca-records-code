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
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Linq;

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
        public ActionResult<List<OutboundCollectionWithFullRecords>> GetAllCollectionsByRoleForUser()
        {
            string username = User.Identity.Name;

            username = "user";

            try
            {
                // find the user's role
                string userRole = _userDao.GetUserRole(username);

                List<OutboundCollectionWithFullRecords> output = new List<OutboundCollectionWithFullRecords>();

                List<Collection> collections = new List<Collection>();

                // get the collections by user role
                // free should only get <25 back
                collections = _collectionsDao.GetAllCollectionsByRole(username, (userRole == FreeAccountName ? NotPremium : IsPremium));

                if(collections == null)
                {
                    // short circuit and stop here if nothign was found
                    return NotFound();
                }

                // otherwise, get all the records for these collections
                foreach(Collection collection in collections)
                {
                    // create a new OutboundCollection object for this collection
                    OutboundCollectionWithFullRecords collectionWithRecords = new OutboundCollectionWithFullRecords();

                    collectionWithRecords.Name = collection.Name;

                    // have the collection id. Need a list of records for them
                    List<RecordCollection> recordsInCollection = _recordsCollectionsDao.GetAllRecordsInCollectionByCollectionId(collection.Collection_Id);

                    // then for each of those records, build the full record and attach it to the outbound object
                    foreach (RecordCollection record in recordsInCollection)
                    {
                        collectionWithRecords.Records.Add(BuildFullRecord(record.Discogs_Id));
                    }

                    // then when you get here, add that collectionWithRecords to the output
                    output.Add(collectionWithRecords);
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
                ErrorLog.WriteLog("Trying to get all collections for this user", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public ActionResult<Collection> CreateCollection(IncomingCollectionRequest newCollection)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {

                // find the user's role
                string userRole = _userDao.GetUserRole(username);

                // check if they're a free user. If so, check if they have a collection already, if so, reject. If not, add
                if (userRole == FreeAccountName)
                {
                    int collectionCount = _collectionsDao.GetFreeUserCollectionCountByUsername(username);

                    if (collectionCount >= FreeUserCollectionLimit)
                    {
                        return BadRequest($"As a free user, you cannot exceed the collection limit of {FreeUserCollectionLimit}. Please upgrade or remove an existing collection");
                    }
                }

                int collectionId = _collectionsDao.AddCollection(username, newCollection.Name);

                // get the full collection
                Collection output = _collectionsDao.GetNamedCollection(username, newCollection.Name, (userRole == FreeAccountName ? NotPremium : IsPremium));

                if (output != null)
                {
                    // if successful, send back the new collection. Don't need to send back the records, it's new so won't have any
                    return Created(CreationPathReRoute, output);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add a collection", $"For {username}, {newCollection.Name}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{name}")]
        public ActionResult<OutboundCollectionWithFullRecords> GetSpecificCollection(string name)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                OutboundCollectionWithFullRecords output = new OutboundCollectionWithFullRecords();

                // get the full collection
                Collection collection = _collectionsDao.GetNamedCollection(username, name);

                output.Name = collection.Name;

                // get the records in this collection
                List<RecordCollection> recordsInCollection = new List<RecordCollection>();

                foreach(RecordCollection recordInCollection in recordsInCollection)
                {
                    // add the full record to the output
                    output.Records.Add(BuildFullRecord(recordInCollection.Discogs_Id));
                }

                if (output.Records.Count != 0)
                {
                    // if successful, send back the  collection and its records
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

        [HttpDelete("{name}")]
        public ActionResult<Collection> DeleteSpecificCollection(string name)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                // to delete, you need to delete the any time the record appears in records_collections first for this collection
                // need the collection ID for that
                Collection collectionToDelete = _collectionsDao.GetNamedCollection(username, name);

                if (collectionToDelete == null)
                {
                    return NotFound();
                }
                _recordsCollectionsDao.DeleteAllRecordsInCollectionByCollectionId(collectionToDelete.Collection_Id);

                // then delete the collection itself
                bool output = _collectionsDao.DeleteCollection(name, username);

                if (output)
                {
                    // if successful, send back the new collection
                    return Ok($"{name} has been deleted");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to delete a collection", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{name}/name")]
        public ActionResult<Collection> ChangeCollectionName(string name, IncomingCollectionRequest request)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                // check if the resource exists
                Collection collectionToUpdate = _collectionsDao.GetNamedCollection(username, name);

                if (collectionToUpdate == null)
                {
                    return NotFound();
                }

                // then update the collection
                bool output = _collectionsDao.UpdateCollectionTitle(name, username, request.Name);

                if (output)
                {
                    // if successful, send back the updated collection
                    return Ok(_collectionsDao.GetNamedCollection(username, name));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to update a collection's name", $"For {username}, {name}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{name}/privacy")]
        public ActionResult<Collection> ChangeCollectionPrivacy(string name, IncomingCollectionRequest request)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                // check if the resource exists
                Collection collectionToUpdate = _collectionsDao.GetNamedCollection(username, name);

                if (collectionToUpdate == null)
                {
                    return NotFound();
                }

                // then update the collection
                bool output = _collectionsDao.ChangeCollectionPrivacy(name, username, request.Is_Private);

                if (output)
                {
                    // if successful, send back the updated collection
                    return Ok(_collectionsDao.GetNamedCollection(username, name));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to update a collection's privacy", $"For {username}, {name}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("{name}/record")]
        public ActionResult<RecordClient> AddRecordToCollection(string name, IncomingRecord recordToAdd)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                // check if the resource exists
                Collection collectionToUpdate = _collectionsDao.GetNamedCollection(username, name);
                // check if the record is in the collection
                RecordTableData record = _recordBuilderDao.GetRecordByDiscogsId(recordToAdd.Discogs_Id);

                if (collectionToUpdate == null)
                {
                    return NotFound("This collection does not exist");
                }
                else if (record == null)
                {
                    return NotFound("This record has not yet been added to a library successfully");
                }

                bool recordAdded = _recordsCollectionsDao.AddRecordCollections(recordToAdd.Discogs_Id, collectionToUpdate.Collection_Id);

                if (recordAdded)
                {
                    // if successful, send back the updated collection
                    return Ok(BuildFullRecord(recordToAdd.Discogs_Id));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to add a record to a collection", $"For {username}, {recordToAdd.Discogs_Id}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{name}/record/{id}")]
        public ActionResult<Collection> RemoveRecordFromCollection(string name, int id)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                // need the collection ID for that
                Collection collection = _collectionsDao.GetNamedCollection(username, name);

                RecordTableData record = _recordBuilderDao.GetRecordByDiscogsId(id);

                if (collection == null)
                {
                    return NotFound("This collection does not exist");
                }
                else if (record == null)
                {
                    return NotFound("This record has not yet been added to a library successfully");
                }

                // if all good, remove the record
                bool output = _recordsCollectionsDao.DeleteRecordCollectionByDiscogsIdAndCollectionId(id, collection.Collection_Id);

                if (output)
                {
                    // if successful, send back the new collection
                    return Ok($"{record.Title} has removed from {name}");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to view a record in a collection", $"For {username}, record {id}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{name}/record/{id}")]
        public ActionResult<RecordClient> GetRecordInCollection(string name, int id)
        {
            string username = User.Identity.Name;
            username = "user";

            try
            {
                // build the record client from the database. Don't really care about the user...
                RecordClient output = BuildFullRecord(id);

                if (output != null)
                {
                    // if successful, send back the new collection
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to do return a full record", $"For {username}, record {id}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }




    }
}
