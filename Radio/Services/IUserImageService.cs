namespace Radio.Services
{
    using System.Threading.Tasks;

    public interface IUserImageService
    {
        Task StoreImageForUserId(string id);

        string UserImageFilenameForUserId(string id);
    }
}