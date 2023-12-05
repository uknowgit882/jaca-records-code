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
        private readonly IFormatsDao _formatsDao;
        private readonly IFriendsDao _friendsDao;
        private readonly IGenresDao _genresDao;
        private readonly ILabelsDao _labelsDao;
        private readonly ITracksDao _tracksDao;
        private readonly IUserDao _userDao;
        public readonly ISearchService searchService = new SearchService();
        public TestController(IArtistsDao artistsDao, IFormatsDao formatsDao, IFriendsDao friendsDao, IGenresDao genresDao, ILabelsDao labelsDao, ITracksDao tracksDao, IUserDao userDao)
        {
            _artistsDao = artistsDao;
            _formatsDao = formatsDao;
            _friendsDao = friendsDao;
            _genresDao = genresDao;
            _labelsDao = labelsDao;
            _tracksDao = tracksDao;
            _userDao = userDao;
        }

        [HttpGet("GetRecord/{recordId}")]
        public ActionResult<RecordClient> GetRecordById(int recordId)
        {
            try
            {
                RecordClient output = null;
                output = recordService.GetRecord(recordId);
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
            try
            {
                SearchResult output = null;
                output = searchService.SearchForRecord(searchRequest);
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
