namespace Radio.Services
{
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Radio.Services.FileProviders;

    public class UserImageService : IUserImageService
    {
        private readonly IWebRootFileProvider _webRootFileProvider;

        public UserImageService(IWebRootFileProvider webRootFileProvider)
        {
            _webRootFileProvider = webRootFileProvider;
        }

        public async Task StoreImageForUserId(string id)
        {
            string userImagesPath = _webRootFileProvider.GetFileInfo("user-images").PhysicalPath;
            string userImagePath = Path.Combine(userImagesPath, $"{id}.png");

            using (HttpClient httpClient = new HttpClient())
            {
                using (Stream imageStream = await httpClient.GetStreamAsync($"https://api.adorable.io/avatars/100/{id}.png"))
                {
                    using (Stream fileStream = File.Create(userImagePath))
                    {
                        await imageStream.CopyToAsync(fileStream);
                    }
                }
            }
        }

        public string UserImageFilenameForUserId(string id)
        {
            // Currently all images are .png, so no need to search for an extension
            return $"{id}.png";
        }
    }
}