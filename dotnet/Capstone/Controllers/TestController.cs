using Capstone.DAO;
using Capstone.DAO.Interfaces;
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
        private readonly ISearchDao _searchDao;

        public TestController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, IFormatsDao formatsDao, IFriendsDao friendsDao, IGenresDao genresDao,
            IImagesDao imagesDao, ILabelsDao labelsDao, IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao, IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao)
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
            _searchDao = searchDao;
        }

        [HttpGet("AddRecordToDb/{discogsId}")]
        public ActionResult<RecordClient> AddRecordToDbById(int discogsId)
        {
            // this might be atrocious for performance but I am not sure how else to do this
            RecordClient output = null;

            try
            {
                // get the record from the client
                RecordClient clientSuppliedRecord = _recordService.GetRecord(discogsId);

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
                RecordTableData existingRecord = _recordBuilderDao.GetRecordByDiscogsId(discogsId);

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

                    // build out the preceding primary tables
                    // if count is zero, no need for the sql dao for this particular table
                    if (clientSuppliedRecord.Genres.Count != 0)
                    {
                        foreach (string genre in clientSuppliedRecord.Genres)
                        {
                            _genresDao.AddGenre(genre);
                            Genre genreReturned = _genresDao.GetGenre(genre);
                            _recordsGenresDao.AddRecordGenre(clientSuppliedRecord.Id, genreReturned.Genre_Id);
                        }
                    }
                    if (clientSuppliedRecord.Styles.Count != 0)
                    {
                        // we're just lumping genres and styles (sub genre it seems) into one
                        // so repeate the method call
                        foreach (string style in clientSuppliedRecord.Styles)
                        {
                            _genresDao.AddGenre(style);
                            Genre genreReturned = _genresDao.GetGenre(style);
                            _recordsGenresDao.AddRecordGenre(clientSuppliedRecord.Id, genreReturned.Genre_Id);
                        }
                    }
                    if (clientSuppliedRecord.Labels.Count != 0)
                    {
                        foreach (Label item in clientSuppliedRecord.Labels)
                        {
                            _labelsDao.AddLabel(item);
                            Label returnedLabel = _labelsDao.GetLabel(item);
                            _recordsLabelsDao.AddRecordLabel(clientSuppliedRecord.Id, returnedLabel.Label_Id);
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
                                    Format returnedFormat = _formatsDao.GetFormat(description);
                                    _recordsFormatsDao.AddRecordFormat(clientSuppliedRecord.Id, returnedFormat.Format_Id);
                                }
                            }
                        }
                    }
                    if (clientSuppliedRecord.Artists.Count != 0)
                    {
                        foreach (Artist artist in clientSuppliedRecord.Artists)
                        {
                            _artistsDao.AddArtist(artist);
                            Artist returnedArtist = _artistsDao.GetArtist(artist);
                            _recordsArtistsDao.AddRecordArtist(clientSuppliedRecord.Id, returnedArtist.Artist_Id);
                        }
                    }
                    if (clientSuppliedRecord.ExtraArtists.Count != 0)
                    {
                        // same thing for extra artists
                        foreach (Artist extraArtist in clientSuppliedRecord.ExtraArtists)
                        {
                            _artistsDao.AddArtist(extraArtist);
                            Artist returnedArtist = _artistsDao.GetArtist(extraArtist);
                            _recordsExtraArtistsDao.AddRecordExtraArtist(clientSuppliedRecord.Id, returnedArtist.Artist_Id);
                        }
                    }


                    // then do the other downstream builds
                    if (clientSuppliedRecord.Identifiers.Count != 0)
                    {
                        foreach (Identifier identifier in clientSuppliedRecord.Identifiers)
                        {
                            identifier.Discogs_Id = clientSuppliedRecord.Id;
                            _barcodesDao.AddIdentifier(identifier);
                        }
                    }
                    if (clientSuppliedRecord.Images.Count != 0)
                    {
                        foreach (Image image in clientSuppliedRecord.Images)
                        {
                            image.Discogs_Id = clientSuppliedRecord.Id;
                            _imagesDao.AddImage(image);
                        }
                    }
                    if (clientSuppliedRecord.Tracklist.Count != 0)
                    {
                        foreach (Track track in clientSuppliedRecord.Tracklist)
                        {
                            track.Discogs_Id = clientSuppliedRecord.Id;
                            _tracksDao.AddTrack(track);
                        }
                    }
                    // if you get here, assume that everything went well
                    // activate the record
                    // if you have an existing record, activate that
                    // if you have a new record, activate that
                    if (existingRecord != null)
                    {
                        _recordBuilderDao.ActivateRecord(existingRecord.Discogs_Id);
                    }
                    else
                    {
                        _recordBuilderDao.ActivateRecord(newRecord.Discogs_Id);
                    }

                    return Created("https://localhost:44315/", newRecord);
                }
                else if (clientSuppliedRecord.Date_Changed != existingRecord.Discogs_Date_Changed)
                {
                    //do an update
                    // update record
                    _recordBuilderDao.UpdateRecord(clientSuppliedRecord);

                    return Ok("Updated record");
                }
                else
                {
                    // TODO update this
                    return Ok("This record already exists in our database");
                }

            }
            catch (Exception e)
            {

                return BadRequest($"Something went wrong adding your record, id {discogsId}. Please contact an admin");
            }
        }




        [HttpGet("search")]
        public ActionResult<SearchResult> Search(string q, string artist, string title, string genre, string year, string country, string label, string barcode)
        {
            SearchRequest searchRequest = _recordService.GenerateRequestObject(q,artist,title,genre,year,country,label,barcode);
            int pageNumber = 1;
            SearchResult output = null;
            if (searchRequest.TypeOfSearch == "All")
            {
                try
                {
                    output = _recordService.SearchForRecordsDiscogs(searchRequest, pageNumber);
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

            }
            else if (searchRequest.TypeOfSearch == "Collections")
            {

            }
            return output;
        }

        [HttpGet("searchDatabase")]
        public ActionResult<List<RecordTableData>> SearchLibrary(string q, string artist, string title, string genre, string year, string country, string label, string barcode)
        {
            SearchRequest searchRequest = _recordService.GenerateRequestObject(q, artist, title, genre, year, country, label, barcode);

            List<RecordTableData> output = new List<RecordTableData>();
            List<int> recordIds = new List<int>();

            if (!string.IsNullOrEmpty(searchRequest.Query))
            {
                try
                {
                    recordIds = _searchDao.WildcardSearchDatabaseForRecords(searchRequest.Query);
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

            } else
            {
                try
                {
                    recordIds = _searchDao.WildcardAdvancedSearchDatabaseForRecords(searchRequest);
                    //RecordTableData recordToAddToResultsList = null;
                    //foreach (int recordId in recordIds)
                    //{

                    //}
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
            return output;
        }

    }
}
