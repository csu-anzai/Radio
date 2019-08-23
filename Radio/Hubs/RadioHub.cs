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

            trackService.TrackUpdated += async (sender, e) => await UpdateTrack(e.NewTrackId);
        }

        public async Task Played()
        {
            Track currentTrack = _trackService.CurrentTrack;
            await Clients.Caller.SendAsync("SyncTimeStamp", currentTrack.TimeStampSeconds);
        }

        public async Task Connected()
        {
            Track currentTrack = _trackService.CurrentTrack;
            await Clients.Caller.SendAsync("SyncVideo", currentTrack.Id, currentTrack.TimeStampSeconds);
        }

        private async Task UpdateTrack(string newTrackId)
        {
            await Clients.All.SendAsync("UpdateTrack", newTrackId);
        }
    }
}