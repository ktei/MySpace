using System;
using System.Linq;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Services.Photo;

namespace LiteApp.MySpace.ViewModels
{
    public class PhotoPagedDataSource : IPagedDataSource<PhotoViewModel>
    {

        public PhotoPagedDataSource(string albumId)
        {
            AlbumId = albumId;
        }

        public string AlbumId { get; private set; }

        public void FetchData(int pageIndex, int pageSize, Action<PagedDataResponse<PhotoViewModel>> responseCallback)
        {
            PhotoServiceClient client = new PhotoServiceClient();
            client.GetPagedPhotosCompleted += (sender, e) =>
            {
                PagedDataResponse<PhotoViewModel> response = new PagedDataResponse<PhotoViewModel>();
                if (e.Error != null)
                {
                    response.Error = e.Error;
                }
                else
                {
                    response.TotalItemCount = e.Result.TotalItemCount;
                    response.Items = new System.Collections.Generic.List<PhotoViewModel>();
                    response.Items.AddRange(e.Result.Entities.Select(x => MapToPhotoViewModel(x)));
                }
                responseCallback(response);
            };
            client.GetPagedPhotosAsync(pageIndex, pageSize, AlbumId);
        }

        PhotoViewModel MapToPhotoViewModel(LiteApp.MySpace.Services.Photo.Photo photo)
        {
            return new PhotoViewModel()
            {
                Id = photo.Id,
                CreatedOn = photo.CreatedOn,
                Description = photo.Description,
                ThumbURI = photo.ThumbURI,
                PhotoURI = photo.PhotoURI
            };
        }
    }
}
