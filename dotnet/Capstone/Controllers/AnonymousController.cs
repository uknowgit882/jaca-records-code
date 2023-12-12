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
using Microsoft.AspNetCore.Authorization;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnonymousController : CommonController
    {
        public AnonymousController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, ICollectionsDao collectionsDao, IFormatsDao formatsDao,
            IFriendsDao friendsDao, IGenresDao genresDao, IImagesDao imagesDao, ILabelsDao labelsDao, ILibrariesDao librariesDao,
            IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsCollectionsDao recordsCollectionsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao,
            IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao)
            : base(artistsDao, barcodesDao, collectionsDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, librariesDao,
                  recordBuilderDao, recordsArtistsDao, recordsCollectionsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao,
                  recordService, tracksDao, userDao, searchDao)
        {
        }
        [HttpGet("collections")]
        public ActionResult<List<OutboundCollectionWithFullRecords>> GetPublicCollections()
        { 
             
            try
            {
                List<OutboundCollectionWithFullRecords> output = new List<OutboundCollectionWithFullRecords>();
                // get all public collections
                List<Collection> collections = _collectionsDao.GetPubOrPrivAllCollections(NotPrivate);

                // get all the records for these collections
                foreach (Collection collection in collections)
                {
                    // create a new OutboundCollection object for this collection
                    OutboundCollectionWithFullRecords collectionWithRecords = new OutboundCollectionWithFullRecords();

                    collectionWithRecords.Name = collection.Name;

                    // have the collection id. Need a list of records for them
                    List<int> recordsInCollection = _recordsCollectionsDao.GetAllRecordsInCollectionByCollectionId(collection.Collection_Id);

                    // then for each of those records, build the full record and attach it to the outbound object
                    foreach (int record in recordsInCollection)
                    {
                        collectionWithRecords.Records.Add(BuildFullRecord(record));
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
                ErrorLog.WriteLog("Returning public collections", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
