namespace Radio.Services
{
    using System.Collections.Generic;

    using Radio.Models;

    public class TrackQueue : ITrackQueue
    {
        private readonly List<Track> _tracks;

        public TrackQueue(ITrackLoader trackLoader)
        {
            _tracks = new List<Track>(trackLoader.LoadAllTracks());
        }

        public IEnumerable<Track> AllTracks => _tracks;

        public Track PopNext()
        {
            Track track = _tracks[0];
            _tracks.RemoveAt(0);
            _tracks.Add(track);
            return track;
        }

        public void InsertHead(Track track)
        {
            _tracks.Insert(0, track);
        }

        public void InsertTail(Track track)
        {
            _tracks.Add(track);
        }
    }
}