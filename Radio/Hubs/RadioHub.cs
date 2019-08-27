namespace Radio.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Radio.Models;
    using Radio.Services;

    public class RadioHub : Hub
    {
        private readonly ITrackStatusService _trackStatusService;

        public RadioHub(ITrackStatusService trackStatusService)
        {
            _trackStatusService = trackStatusService;
        }

        public override async Task OnConnectedAsync()
        {
            await SyncClientToCurrentVideo();
        }

        public async Task Played()
        {
            await SyncClientToCurrentVideo();
        }

        private async Task SyncClientToCurrentVideo()
        {
            TrackStatus currentTrackStatus = _trackStatusService.CurrentTrackStatus;
            await Clients.Caller.SendAsync("SyncVideo", currentTrackStatus.CurrentTrackId, currentTrackStatus.TimeStampSeconds);
        }
    }
}