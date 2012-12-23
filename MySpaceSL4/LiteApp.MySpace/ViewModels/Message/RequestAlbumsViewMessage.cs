
namespace LiteApp.MySpace.ViewModels.Message
{
    public class RequestAlbumsViewMessage
    {
        public RequestAlbumsViewMessage(AlbumViewModel requester)
        {
            Requester = requester;
        }

        public AlbumViewModel Requester { get; private set; }
    }
}
