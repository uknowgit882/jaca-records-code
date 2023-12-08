using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.DAO.Interfaces
{
    public interface IImagesDao
    {
        public Image GetImageInfoExact(Image image);
        public List<Image> GetAllImagesByDiscogsId(int discogsId);
        public int GetImageCountByUsername(string username);
        public int GetImageCount();
        //public List<Image> GetImagesByDiscogsIdAndUsername(int discogId, string username); // might not need
        public bool AddImage(Image image);
        public int DeleteImage(int imageId);
    }
}
