using Microsoft.AspNetCore.Mvc;
using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security;
using Capstone.DAO.Interfaces;
using System;
using Capstone.DAO;
using Capstone.Service;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class CommonController : ControllerBase
    {
        protected readonly IArtistsDao _artistsDao;
        protected readonly IBarcodesDao _barcodesDao;
        protected readonly IFormatsDao _formatsDao;
        protected readonly IFriendsDao _friendsDao;
        protected readonly IGenresDao _genresDao;
        protected readonly IImagesDao _imagesDao;
        protected readonly ILabelsDao _labelsDao;
        protected readonly IRecordBuilderDao _recordBuilderDao;
        protected readonly IRecordsArtistsDao _recordsArtistsDao;
        protected readonly IRecordsExtraArtistsDao _recordsExtraArtistsDao;
        protected readonly IRecordsFormatsDao _recordsFormatsDao;
        protected readonly IRecordsGenresDao _recordsGenresDao;
        protected readonly IRecordsLabelsDao _recordsLabelsDao;
        protected readonly ITracksDao _tracksDao;
        protected readonly IUserDao _userDao;
        protected readonly IRecordService _recordService;
        protected readonly ISearchDao _searchDao;

        public CommonController(IArtistsDao artistsDao, IBarcodesDao barcodesDao, IFormatsDao formatsDao, IFriendsDao friendsDao, IGenresDao genresDao,
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
    }
}
