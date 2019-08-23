namespace Radio.Services
{
    using System.Collections.Generic;

    using Radio.Models;

    public interface ITrackQueue
    {
        IEnumerable<Track> AllTracks { get; }

        Track PopNext();

        void InsertHead(Track track);

        void InsertTail(Track track);
    }
}