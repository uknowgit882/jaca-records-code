using Capstone.DAO;
using Capstone.DAO.Interfaces;
using Capstone.Models;
using Capstone.Service;
using Capstone.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : CommonController
    {
        public SearchController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, ICollectionsDao collectionsDao, IFormatsDao formatsDao,
            IFriendsDao friendsDao, IGenresDao genresDao, IImagesDao imagesDao, ILabelsDao labelsDao, ILibrariesDao librariesDao,
            IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsCollectionsDao recordsCollectionsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao,
            IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao)
            : base(artistsDao, barcodesDao, collectionsDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, librariesDao,
                recordBuilderDao, recordsArtistsDao, recordsCollectionsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao,
                recordService, tracksDao, userDao, searchDao)
        {
        }

        // Searches the Discogs API for records to add to user library
        [HttpGet("search")]
        [Authorize]
        public ActionResult<SearchResult> SearchDiscogs(string q, string artist, string title, string genre, string year, string country, string label, string barcode, int pageNumber = 1)
        {
            SearchResult discogsResult = null;
            // need the username to search the library
            string username = User.Identity.Name;
            if (username == null)
            {
                return BadRequest("You must be logged in to search a library");
            }

            SearchRequest searchRequest = _recordService.GenerateRequestObject(q, artist, title, genre, year, country, label, barcode);

            if (string.IsNullOrEmpty(searchRequest.Query) && string.IsNullOrEmpty(searchRequest.Artist) && string.IsNullOrEmpty(searchRequest.Title) && string.IsNullOrEmpty(searchRequest.Genre) && string.IsNullOrEmpty(searchRequest.Year) && string.IsNullOrEmpty(searchRequest.Country) && string.IsNullOrEmpty(searchRequest.Label) && string.IsNullOrEmpty(searchRequest.Barcode))
            {
                return BadRequest("Please enter a valid search");
            }

            try
            {
                discogsResult = _recordService.SearchForRecordsDiscogs(searchRequest, pageNumber);

                if (discogsResult != null)
                {
                    return Ok(discogsResult);
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

        // Searches for records in user library
        [HttpGet("searchLibrary")]
        [Authorize]
        public ActionResult<List<RecordClient>> SearchLibrary(string q, string artist, string title, string genre, string year, string country, string label, string barcode)
        {
            string username = User.Identity.Name;
            if (username == null)
            {
                return BadRequest("You must be logged in to search a library");
            }

            SearchRequest searchRequest = _recordService.GenerateRequestObject(q, artist, title, genre, year, country, label, barcode);

            List<RecordClient> output = new List<RecordClient>();

            try
            {
                List<int> recordIds = new List<int>();
                if (string.IsNullOrEmpty(searchRequest.Artist) && string.IsNullOrEmpty(searchRequest.Title) && string.IsNullOrEmpty(searchRequest.Genre) && string.IsNullOrEmpty(searchRequest.Year) && string.IsNullOrEmpty(searchRequest.Country) && string.IsNullOrEmpty(searchRequest.Label) && string.IsNullOrEmpty(searchRequest.Barcode))
                {
                    recordIds = _searchDao.WildcardSearchDatabaseForRecords(searchRequest.Query, username);
                }
                else
                {
                    recordIds = _searchDao.WildcardAdvancedSearchDatabaseForRecords(searchRequest, username);
                }

                if (recordIds.Count == 0)
                {
                    RecordClient dummyOut = new RecordClient();
                    return Ok(dummyOut);
                }

                foreach (int discogId in recordIds)
                {
                    // this is much neater
                    // refactored and put in the parent CommonController class as a helper method
                    RecordClient newFullRecord = BuildFullRecord(discogId);

                    if (newFullRecord != null)
                    {
                        output.Add(newFullRecord);
                    }
                }

                //PaginationFilter validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                //var pagedData = output.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize);

                if (output != null)
                {
                    return Ok(output);
                    //return Ok(new PagedResponse<List<RecordClient>>((List<RecordClient>)pagedData, validFilter.PageNumber, validFilter.PageSize));
                    //return Ok(new PagedResponse<List<Customer>>(pagedData, validFilter.PageNumber, validFilter.PageSize));
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

        // Searches for records in user collections
        [HttpGet("searchCollections")]
        [Authorize]
        public ActionResult<List<OutboundCollectionWithFullRecords>> SearchCollections(string q, string artist, string title, string genre, string year, string country, string label, string barcode)
        {
            string username = User.Identity.Name;
            if (username == null)
            {
                return BadRequest("You must be logged in to search a library");
            }

            SearchRequest searchRequest = _recordService.GenerateRequestObject(q, artist, title, genre, year, country, label, barcode);

            List<OutboundCollectionWithFullRecords> output = new List<OutboundCollectionWithFullRecords>();

            try
            {
                string userRole = _userDao.GetUserRole(username);

                List<int> recordIds = new List<int>();
                if (string.IsNullOrEmpty(searchRequest.Artist) && string.IsNullOrEmpty(searchRequest.Title) && string.IsNullOrEmpty(searchRequest.Genre) && string.IsNullOrEmpty(searchRequest.Year) && string.IsNullOrEmpty(searchRequest.Country) && string.IsNullOrEmpty(searchRequest.Label) && string.IsNullOrEmpty(searchRequest.Barcode))
                {
                    recordIds = _searchDao.WildcardSearchCollectionsForRecords(searchRequest.Query, username);
                }
                else
                {
                    recordIds = _searchDao.WildcardAdvancedSearchCollectionsForRecords(searchRequest, username);
                }

                if (recordIds.Count == 0)
                {
                    List<OutboundCollectionWithFullRecords> dummyOut = new List<OutboundCollectionWithFullRecords>();
                    return Ok(dummyOut);
                }

                if (recordIds.Count != 0)
                {
                    // for each record found
                    foreach (int recordId in recordIds)
                    {
                        // get me a list of all collections that this record appears in for this user
                        List<int> collectionsIds = _recordsCollectionsDao.GetAllCollectionsForThisDiscogsIdByUsername(recordId, username);

                        // foreach collection found
                        foreach (int collectionId in collectionsIds)
                        {
                            // get me the whole collection
                            Collection returnedCollection = _collectionsDao.GetCollectionByCollectionId(collectionId, (userRole == FreeAccountName ? NotPremium : IsPremium));

                            // for the collection, associate the name and privacy status
                            OutboundCollectionWithFullRecords outputRow = new OutboundCollectionWithFullRecords();
                            outputRow.Name = returnedCollection.Name;
                            outputRow.Is_Private = returnedCollection.IsPrivate;

                            // find me all the records in this collection
                            List<int> foundCollectionRecordsIds = _recordsCollectionsDao.GetAllRecordsInCollectionByCollectionId(returnedCollection.Collection_Id);

                            // build the full record for the discogsid
                            foreach (int foundRecordId in foundCollectionRecordsIds)
                            {
                                outputRow.Records.Add(BuildFullRecord(foundRecordId));
                            }
                            output.Add(outputRow);
                        }
                    }
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }

}

