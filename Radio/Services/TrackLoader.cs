namespace Radio.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Extensions.FileProviders;

    using Newtonsoft.Json;

    using Radio.Models;

    public class TrackLoader : ITrackLoader
    {
        private readonly IFileProvider _fileProvider;

        public TrackLoader(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public IEnumerable<Track> LoadAllTracks()
        {
            using Stream stream = _fileProvider.GetFileInfo("Tracks.json").CreateReadStream();
            using StreamReader streamReader = new StreamReader(stream);

            return JsonConvert.DeserializeObject<Track[]>(streamReader.ReadToEnd());
        }
    }
}