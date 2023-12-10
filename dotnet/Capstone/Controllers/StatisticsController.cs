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
using Microsoft.AspNetCore.Authorization;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : CommonController
    {
        public StatisticsController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, ICollectionsDao collectionsDao, IFormatsDao formatsDao,
            IFriendsDao friendsDao, IGenresDao genresDao, IImagesDao imagesDao, ILabelsDao labelsDao, ILibrariesDao librariesDao,
            IRecordBuilderDao recordBuilderDao, IRecordsArtistsDao recordsArtistsDao, IRecordsCollectionsDao recordsCollectionsDao, IRecordsExtraArtistsDao recordsExtraArtistsDao,
            IRecordsFormatsDao recordsFormatsDao, IRecordsGenresDao recordsGenresDao, IRecordsLabelsDao recordsLabelsDao,
            IRecordService recordService, ITracksDao tracksDao, IUserDao userDao, ISearchDao searchDao)
            : base(artistsDao, barcodesDao, collectionsDao, formatsDao, friendsDao, genresDao, imagesDao, labelsDao, librariesDao,
                  recordBuilderDao, recordsArtistsDao, recordsCollectionsDao, recordsExtraArtistsDao, recordsFormatsDao, recordsGenresDao, recordsLabelsDao,
                  recordService, tracksDao, userDao, searchDao)
        {
        }

        [HttpGet("user")]
        [Authorize]
        public ActionResult<Statistics> GetUserStats()
        {
            string username = User.Identity.Name;
            string usersRole = _userDao.GetUserRole(username);

            bool isPremium = usersRole == FreeAccountName ? NotPremium : IsPremium;

            try
            {
                Statistics output = new Statistics();

                output.TotalArtists = (_artistsDao.GetArtistCountByUsername(username, isPremium) + _artistsDao.GetExtraArtistCountByUsername(username, isPremium));
                output.TotalCollections = _collectionsDao.GetCollectionCountByUsername(username, isPremium);
                output.TotalFormats = _formatsDao.GetFormatCountByUsername(username, isPremium);
                output.TotalGenres = _genresDao.GetGenreCountByUsername(username, isPremium);
                output.TotalImages = _imagesDao.GetImageCountByUsername(username, isPremium);
                output.TotalLabels = _labelsDao.GetLabelCountByUsername(username, isPremium);
                output.TotalRecordsInCollections = _collectionsDao.CountOfRecordsInAllCollectionsByUsername(username, isPremium);
                output.TotalTracks = _tracksDao.GetTrackCountByUsername(username, isPremium);
                output.NumRecordsByArtist = _artistsDao.GetArtistAndRecordCountByUsername(username, isPremium);
                output.NumRecordsByFormat = _formatsDao.GetFormatAndRecordCountByUsername(username, isPremium);
                output.NumRecordsByGenre = _genresDao.GetGenreAndRecordCountByUsername(username, isPremium);
                output.NumRecordsByLabel = _labelsDao.GetLabelAndRecordCountByUsername(username, isPremium);

                    return Ok(output);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Trying to do stuff", $"For {username}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("aggregate")]
        public ActionResult<StatisticsAggregate> GetAggregateUserStats()
        {
            try
            {
                StatisticsAggregate output = new StatisticsAggregate();

                output.TotalArtists = _artistsDao.GetArtistCount();
                output.TotalCollections = _collectionsDao.GetCollectionCount();
                output.TotalFormats = _formatsDao.GetFormatCount();
                output.TotalGenres = _genresDao.GetGenreCount();
                output.TotalImages = _imagesDao.GetImageCount();
                output.TotalLabels = _labelsDao.GetLabelCount();
                output.TotalTracks = _tracksDao.GetTrackCount();
                output.TotalRecordsInCollections = _collectionsDao.CountOfRecordsInAllCollections();

                output.NumRecordsByArtist = _artistsDao.GetArtistAndRecordCount();
                output.NumRecordsByFormat = _formatsDao.GetFormatAndRecordCount();
                output.NumRecordsByGenre = _genresDao.GetGenreAndRecordCount();
                output.NumRecordsByLabel = _labelsDao.GetLabelAndRecordCount();

                output.TotalUsers = _userDao.GetUserCount();
                output.TotalRecords = _recordBuilderDao.GetRecordCount();
                output.NumRecordsByYear = _recordBuilderDao.GetYearAndRecordCount();
                output.NumRecordsByCountry = _recordBuilderDao.GetCountryAndRecordCount();

                return Ok(output);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Getting aggregate stats", $"", MethodBase.GetCurrentMethod().Name, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
