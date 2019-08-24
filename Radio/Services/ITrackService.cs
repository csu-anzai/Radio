namespace Radio.Services
{
    using Radio.Models;

    public interface ITrackService
    {
        TrackStatus CurrentTrackStatus { get; }

        Track CurrentTrack { get; }
    }
}