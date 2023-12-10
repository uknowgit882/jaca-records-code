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
    public class TestController : CommonController
    {

        public TestController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, ICollectionsDao collectionsDao, IFormatsDao formatsDao,
            IFriendsDao friendsDao, IGenresDao genresDao, IImagesDao imagesDao, ILabelsDao labelsDao, ILibrariesDao librariesDao,
            IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsCollectionsDao recordsCollectionsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao,
            IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao)
            : base(artistsDao, barcodesDao, collectionsDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, librariesDao,
                  recordBuilderDao, recordsArtistsDao, recordsCollectionsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao,
                  recordService, tracksDao, userDao, searchDao)
        {
        }

       

        [HttpGet("searchDatabase")]
        public ActionResult<List<RecordClient>> SearchLibrary(string q, string artist, string title, string genre, string year, string country, string label, string barcode, int pageNumber = 1)
        {
            string username = User.Identity.Name;
            username = "user";
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
                    return NotFound();
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

                if (output != null)
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

        [HttpGet("search")]
        public ActionResult<List<SearchResult>> SearchAll(string q, string artist, string title, string genre, string year, string country, string label, string barcode, int pageNumber = 1)
        {
            List<SearchResult> allResults = new List<SearchResult>();
            // need the username to search the library
            //TODO
            string username = User.Identity.Name;
            username = "user";
            if (username == null)
            {
                return BadRequest("You must be logged in to search a library");
            }

            SearchRequest searchRequest = _recordService.GenerateRequestObject(q, artist, title, genre, year, country, label, barcode);
            SearchResult discogsResult = null;
            SearchResult libraryResult = null;
            List<RecordClient> recordsInLibrary = null;
            SearchResult collectionsResult = null;

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
                    //allResults.Add(discogsResult);
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




            return allResults;
        }

    }
}
