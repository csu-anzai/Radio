namespace Radio.Services
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Timers;

    using Microsoft.AspNetCore.SignalR;

    using Radio.Hubs;
    using Radio.Models;

    public class TrackService : ITrackService
    {
        private readonly ITrackQueue _trackQueue;

        private readonly IHubContext<RadioHub> _radioHub;

        private readonly Timer _timer;

        private readonly Stopwatch _stopwatch;

        public TrackService(ITrackQueue trackQueue, IHubContext<RadioHub> radioHub)
        {
            _trackQueue = trackQueue;
            _radioHub = radioHub;
            _timer = new Timer();
            _timer.Elapsed += async (sender, e) => await NextTrack();
            _stopwatch = new Stopwatch();

            NextTrack();
        }

        public TrackStatus CurrentTrackStatus => new TrackStatus(CurrentTrack.Id, (int)_stopwatch.Elapsed.TotalSeconds);

        public Track CurrentTrack { get; private set; }

        private async Task NextTrack()
        {
            CurrentTrack = _trackQueue.PopNext();
            await _radioHub.Clients.All.SendAsync("UpdateTrack", CurrentTrack.Id);

            _timer.Stop();
            _timer.Interval = CurrentTrack.Length.TotalMilliseconds;
            _timer.Start();
            _stopwatch.Restart();
        }
    }
}