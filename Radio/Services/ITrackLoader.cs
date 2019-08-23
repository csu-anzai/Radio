namespace Radio.Services
{
    using System.Collections.Generic;

    using Radio.Models;

    public interface ITrackLoader
    {
        IEnumerable<Track> LoadAllTracks();
    }
}