namespace Radio.Services
{
    using Radio.Models;

    public interface ITrackStatusService
    {
        TrackStatus CurrentTrackStatusFor(string channelId);
    }
}