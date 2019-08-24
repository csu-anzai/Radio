namespace Radio.Services
{
    using System;
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
            using (Stream stream = _fileProvider.GetFileInfo("wwwroot/Tracks.json").CreateReadStream())
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    Track[] tracks = JsonConvert.DeserializeObject<Track[]>(streamReader.ReadToEnd());
                    Shuffle(tracks);
                    return tracks;
                }
            }
        }

        private static void Shuffle<T>(T[] array)
        {
            Random random = new Random();

            for (int index = 0; index < array.Length; index++)
            {
                int swapIndex = index + random.Next(array.Length - index);

                T swapItem = array[swapIndex];
                array[swapIndex] = array[index];
                array[index] = swapItem;
            }
        }
    }
}