namespace Radio.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Timers;

    using Microsoft.AspNetCore.SignalR;

    using Radio.EventArgs;
    using Radio.Hubs;
    using Radio.Models;

    public class TrackService : ITrackService
    {
        private readonly ITrackQueue _trackQueue;

        private readonly IHubContext<RadioHub> _radioHub;

        private readonly Timer _timer;

        private readonly Stopwatch _stopwatch;

        private Track _currentTrack;

        public TrackService(ITrackQueue trackQueue, IHubContext<RadioHub> radioHub)
        {
            _trackQueue = trackQueue;
            _radioHub = radioHub;
            _timer = new Timer();
            _timer.Elapsed += async (sender, e) => await NextTrack();
            _stopwatch = new Stopwatch();

            NextTrack();
        }

        public event EventHandler<TrackUpdatedEventArgs> TrackUpdated;

        public TrackStatus CurrentTrackStatus => new TrackStatus(_currentTrack.Id, (int)_stopwatch.Elapsed.TotalSeconds);

        private async Task NextTrack()
        {
            _currentTrack = _trackQueue.PopNext();
            await _radioHub.Clients.All.SendAsync("UpdateTrack", _currentTrack.Id);

            _timer.Stop();
            _timer.Interval = _currentTrack.Length.TotalMilliseconds;
            _timer.Start();
            _stopwatch.Restart();
        }
    }
}