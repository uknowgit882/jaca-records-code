using Capstone.DAO;
using Capstone.Models;
using Capstone.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IArtistsDao _artistsDao;
        private readonly IBarcodesDao _barcodesDao;
        private readonly IFormatsDao _formatsDao;
        private readonly IFriendsDao _friendsDao;
        private readonly IGenresDao _genresDao;
        private readonly IImagesDao _imagesDao;
        private readonly ILabelsDao _labelsDao;
        private readonly IRecordBuilderDao _recordBuilderDao;
        private readonly IRecordsArtistsDao _recordsArtistsDao;
        private readonly IRecordsExtraArtistsDao _recordsExtraArtistsDao;
        private readonly IRecordsFormatsDao _recordsFormatsDao;
        private readonly IRecordsGenresDao _recordsGenresDao;
        private readonly IRecordsLabelsDao _recordsLabelsDao;
        private readonly ITracksDao _tracksDao;
        private readonly IUserDao _userDao;
        public readonly IRecordService _recordService;

        public TestController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, IFormatsDao formatsDao, IFriendsDao friendsDao, IGenresDao genresDao,
            IImagesDao imagesDao, ILabelsDao labelsDao, IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao, IRecordService recordService, ITracksDao tracksDao, IUserDao userDao)
        {
            _artistsDao = artistsDao;
            _barcodesDao = barcodesDao;
            _formatsDao = formatsDao;
            _friendsDao = friendsDao;
            _genresDao = genresDao;
            _imagesDao = imagesDao;
            _labelsDao = labelsDao;
            _recordBuilderDao = recordBuilderDao;
            _recordsArtistsDao = recordsArtistsDao;
            _recordsExtraArtistsDao = recordsExtraArtistsDao;
            _recordsFormatsDao = recordsFormatsDao;
            _recordsGenresDao = recordsGenresDao;
            _recordsLabelsDao = recordsLabelsDao;
            _recordService = recordService;
            _tracksDao = tracksDao;
            _userDao = userDao;
        }

        [HttpGet("AddRecordToDb/{recordId}")]
        public ActionResult<RecordClient> AddRecordToDbById(int recordId)
        {
            // this might be atrocious for performance but I am not sure how else to do this
            RecordClient output = null;


            try
            {
                // get the record from the client
                RecordClient clientSuppliedRecord = _recordService.GetRecord(recordId);

                // need to make sure the client supplied record is at least on vinyl once
                int vinylCount = 0;
                foreach (Format item in clientSuppliedRecord.Formats)
                {
                    // go through each of the formats and count
                    if (item.Name.ToLower() == "vinyl")
                    {
                        vinylCount++;
                    }
                }
                // if there isn't at least one vinyl format, return a bad request 
                if (vinylCount < 1)
                {
                    return BadRequest("This record does not contain any vinyl. We're a vinyl only shop. Try again.");
                }

                // check if it's in the record table first
                // if yes, just need to check if we should update the database
                //      check if the date_changes is different in discogs from the last time we pulled
                //      if yes, update
                // if not in our database, add to it

                // need to refactor this so that if a downstream table load fails, it'll still go through the new created pathway
                // maybe extra column that says "fully loaded 1/0" and that only gets updated once all the things parse?
                RecordTableData existingRecord = _recordBuilderDao.GetRecordByDiscogsId(recordId);

                // checks if you have an existing record
                // if you don't, add
                // if you do, check if it's active. If not active, assumed something in here failed
                if (existingRecord == null || existingRecord.Is_Active == false)
                {
                    // do the add if it doesn't exist
                    // can build this out first
                    RecordTableData newRecord = null;
                    if (existingRecord == null)
                    {
                        newRecord = _recordBuilderDao.AddRecord(clientSuppliedRecord);
                    }

        [HttpPost("Artist/")]
        public ActionResult<bool> AddArtist(Artist newArtist)
        {
            try
            {
                bool output = false;
                output = _artistsDao.AddArtist(newArtist);
                // redirects when created (placeholder), or gives a 200 and message
                return output ? Created("https://localhost:44315/", output) : Ok("This artist already exists");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("Format/")]
        public ActionResult<bool> AddFormat(string newFormat)
        {
            try
            {
                bool output = false;
                output = _formatsDao.AddFormat(newFormat);
                return output ? Created("https://localhost:44315/", output) : Ok("This format already exists");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("Friend/ById/{id}")]
        public ActionResult<UserAndFriends> GetUsersFriendsById(int id)
        {
            try
            {
                UserAndFriends output = null;
                output = _friendsDao.GetUsersFriendsById(id);
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
        [HttpGet("Friend/ByUsername/{username}")]
        public ActionResult<UserAndFriends> GetUsersFriendsByUsername(string username)
        {
            try
            {
                UserAndFriends output = null;
                output = _friendsDao.GetUsersFriendsByUsername(username);
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
        [HttpPost("Friend/{usersId}/Add/{friendsId}")]
        public ActionResult<UserAndFriends> AddFriend(int usersId, int friendsId)
        {
            try
            {
                UserAndFriends output = null;
                output = _friendsDao.AddFriend(usersId, friendsId);
                if (output != null)
                {
                    return Created("https://localhost:44315/", output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong adding your friend. Perhaps you are already friends?");
            }
        }
        [HttpDelete("Friend/{usersId}/Delete/{friendsId}")]
        public ActionResult<UserAndFriends> DropFriend(int usersId, int friendsId)
        {
            try
            {
                UserAndFriends output = null;
                output = _friendsDao.DropFriend(usersId, friendsId);
                if (output != null)
                {
                    //do an update
                    return Ok();
                }
                else
                {
                    // TODO update this
                    return Ok("This record already exists in our database");
                }

            }
            catch (Exception)
            {

                return BadRequest(e.Message);
            }
        }




        [HttpGet("search")]
        public ActionResult<SearchResult> Search(string q, string artist, string title, string genre, string year, string country, string label)
        {
            SearchRequest searchRequest = new SearchRequest();
            searchRequest.Query = q;
            searchRequest.Artist = artist;
            searchRequest.Title = title;
            searchRequest.Genre = genre;
            searchRequest.Year = year;
            searchRequest.Country = country;
            searchRequest.Label = label;
            searchRequest.Barcode = "";
            searchRequest.TypeOfSearch = "All";

            SearchResult output = null;
            if (searchRequest.TypeOfSearch == "All")
            {
                try
                {
                    output = _recordService.SearchForRecordsDiscogs(searchRequest);
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
            else if (searchRequest.TypeOfSearch == "Library")
            {

            } else if (searchRequest.TypeOfSearch == "Collections")
            {

            }
            return output;
        }

        //[HttpGet("results")]
        //public ActionResult<bool> DisplaySearchResults(SearchResult resultsOfSearch)
        //{

        //    return false;
        //}
    }
}
