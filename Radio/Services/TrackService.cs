namespace Radio.Services
{
    using System;

    using Radio.EventArgs;
    using Radio.Models;

    public class TrackService : ITrackService
    {
        public event EventHandler<TrackUpdatedEventArgs> TrackUpdated;

        public Track CurrentTrack { get; }
    }
}