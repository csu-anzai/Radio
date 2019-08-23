namespace Radio.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Radio.Models;
    using Radio.Services;

    public class RadioHub : Hub
    {
        private readonly ITrackService _trackService;

        public RadioHub(ITrackService trackService)
        {
            _trackService = trackService;
        }

        public async Task Played()
        {
            TrackStatus currentTrackStatus = _trackService.CurrentTrackStatus;
            await Clients.Caller.SendAsync("SyncTimeStamp", currentTrackStatus.TimeStampSeconds);
        }

        public async Task Connected()
        {
            TrackStatus currentTrackStatus = _trackService.CurrentTrackStatus;
            await Clients.Caller.SendAsync("SyncVideo", currentTrackStatus.CurrentTrackId, currentTrackStatus.TimeStampSeconds);
        }
    }
}