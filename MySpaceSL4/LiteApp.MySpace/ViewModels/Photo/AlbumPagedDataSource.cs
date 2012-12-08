using System;
using System.Linq;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Services.Photo;

namespace LiteApp.MySpace.ViewModels
{
    public class AlbumPagedDataSource : IPagedDataSource<AlbumViewModel>
    {
        public void FetchData(int pageIndex, int pageSize, Action<PagedDataResponse<AlbumViewModel>> responseCallback)
        {
            PhotoServiceClient client = new PhotoServiceClient();
            client.GetPagedAlbumsCompleted += (sender, e) =>
                {
                    PagedDataResponse<AlbumViewModel> response = new PagedDataResponse<AlbumViewModel>();
                    response.TotalItemCount = e.Result.TotalItemCount;
                    response.Items = new System.Collections.Generic.List<AlbumViewModel>();
                    response.Items.AddRange(e.Result.Entities.Select(x => MapToAlbumViewModel(x)));
                    responseCallback(response);
                };
            client.GetPagedAlbumsAsync(pageIndex, pageSize);
        }

        AlbumViewModel MapToAlbumViewModel(Album album)
        {
            return new AlbumViewModel()
            {
                Id = album.Id,
                Name = album.Name,
                Description = album.Description,
                Covers = AlbumViewModel.GetCovers(album.CoverURIs)
            };
        }
    }
}
