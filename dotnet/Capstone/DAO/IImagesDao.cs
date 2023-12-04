using Capstone.Models;

namespace Capstone.DAO
{
    public interface IImagesDao
    {
        public Image GetImageInfo(Image image);
        public bool AddImage(Image image);
    }
}
