
namespace LiteApp.MySpace.ViewModels.Message
{
    public class UpdateAlbumCoversMessage
    {
        public UpdateAlbumCoversMessage(string albumId, string newCoverURIs)
        {
            AlbumId = albumId;
            NewCoverURIs = newCoverURIs;
        }

        public string AlbumId { get; private set; }
        public string NewCoverURIs { get; private set; }
    }
}
