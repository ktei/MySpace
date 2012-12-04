using System;
using System.Linq;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Services.Photo;

namespace LiteApp.MySpace.ViewModels
{
    public class AlbumPagedDataSource : IPagedDataSource<AlbumViewModel>
    {
        public int PageSize { get; set; }

        public void FetchData(int pageIndex, Action<PagedDataResponse<AlbumViewModel>> responseCallback)
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
            client.GetPagedAlbumsAsync(pageIndex, PageSize);
        }

        static int idx;
        AlbumViewModel MapToAlbumViewModel(Album album)
        {
            if (idx == 0)
            {
                idx++;
                return new AlbumViewModel()
                {
                    Id = album.Id,
                    Name = album.Name,
                    Description = album.Description,
                    CoverUri = "http://dl.dropbox.com/u/63866164/MySpace/Photos/me.png"
                };

            }
            return new AlbumViewModel()
            {
                Id = album.Id,
                Name = album.Name,
                Description = album.Description,
                CoverUri = "http://dl.dropbox.com/u/63866164/MySpace/Photos/thunder.png"
            };

        }
    }
}
