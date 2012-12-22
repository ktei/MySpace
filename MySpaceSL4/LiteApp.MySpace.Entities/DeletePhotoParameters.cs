
namespace LiteApp.MySpace.Entities
{
    public class DeletePhotoParameters
    {
        public string PhotoId { get; set; }

        /// <summary>
        /// File name shared by thumb and photo
        /// </summary>
        public string FileName { get; set; }
    }
}
