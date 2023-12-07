using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IImagesDao
    {
        public Image GetImageInfo(Image image);
        public bool AddImage(Image image);
        public List<Image> GetImagesByDiscogsIdAndUsername(int discogId, string username);
    }
}
