using Capstone.DAO;
using Capstone.Models;
using Capstone.Service;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public readonly IRecordService recordService = new RecordService();
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

        public TestController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, IFormatsDao formatsDao, IFriendsDao friendsDao, IGenresDao genresDao,
            IImagesDao imagesDao, ILabelsDao labelsDao, IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao, ITracksDao tracksDao, IUserDao userDao)
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
                RecordClient clientSuppliedRecord = recordService.GetRecord(recordId);

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
                RecordTableData existingRecord = _recordBuilderDao.GetRecordByDiscogsId(output.Id);

                if (existingRecord == null)
                {
                    // do the add if it doesn't exist

                    // build out the preceding primary tables
                    // if count is zero, no need for the sql dao for this particular table
                    if (clientSuppliedRecord.Genres.Count != 0)
                    {

                    }
                    if (clientSuppliedRecord.Labels.Count != 0)
                    {
                        foreach (Label item in clientSuppliedRecord.Labels)
                        {
                            _labelsDao.AddLabel(item);
                        }
                    }
                    if (clientSuppliedRecord.Formats.Count != 0)
                    {
                        foreach (Format item in clientSuppliedRecord.Formats)
                        {
                            // want only the vinyl formats...
                            if (item.Name.ToLower() == "vinyl")
                            {
                                // then go through the descriptions array and add it to the format table
                                foreach (string description in item.Descriptions)
                                {
                                    _formatsDao.AddFormat(description);
                                }
                            }
                        }
                    }
                    if (clientSuppliedRecord.Artists.Count != 0)
                    {

                    }
                    // once those primary tables have been built, build the record itself
                    RecordTableData newRecord = _recordBuilderDao.AddRecord(clientSuppliedRecord);

                    // then do the other downstream builds
                    if (clientSuppliedRecord.Identifiers.Count != 0)
                    {

                    }
                    if (clientSuppliedRecord.Images.Count != 0)
                    {

                    }
                    if (clientSuppliedRecord.Tracklist.Count != 0)
                    {

                    }

                    // then do the join associations
                    if (clientSuppliedRecord.Artists.Count != 0)
                    {

                    }
                    if (clientSuppliedRecord.ExtraArtists.Count != 0)
                    {

                    }
                    if (clientSuppliedRecord.Formats.Count != 0)
                    {

                    }
                    if (clientSuppliedRecord.Genres.Count != 0)
                    {

                    }
                    if (clientSuppliedRecord.Styles.Count != 0)
                    {
                        // we're just lumping genres and styles (sub genre it seems) into one
                        // so repeate the method call

                    }
                    if (clientSuppliedRecord.Labels.Count != 0)
                    {

                    }
                }
                else if (clientSuppliedRecord.Date_Changed != existingRecord.Discogs_Date_Changed)
                {
                    //do an update
                    return Ok();
                }
                else
                {
                    // TODO update this
                    return Ok("This record already exists in our database");
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


        /*
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
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong removing your friend. Perhaps you are already broke up?");
            }
        }
        */


        [HttpGet("search")]
        public ActionResult<RecordClient> Search(string q, string artist, string title, string genre, string year, string country, string label)
        {
            try
            {
                RecordClient output = null;
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
    }
}
