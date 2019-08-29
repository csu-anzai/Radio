namespace Radio.Services.FileProviders
{
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Primitives;

    public class WebRootFileProvider : IWebRootFileProvider
    {
        private readonly IFileProvider _webRootFileProvider;

        public WebRootFileProvider(IFileProvider webRootFileProvider)
        {
            _webRootFileProvider = webRootFileProvider;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return _webRootFileProvider.GetFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _webRootFileProvider.GetDirectoryContents(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return _webRootFileProvider.Watch(filter);
        }
    }
}