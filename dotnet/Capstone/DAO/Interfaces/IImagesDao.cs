using Capstone.Models;

namespace Capstone.DAO.Interfaces
{
    public interface IImagesDao
    {
        public Image GetImageInfo(Image image);
        public bool AddImage(Image image);
    }
}
