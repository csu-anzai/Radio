namespace Radio.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Radio.Hubs;
    using Radio.Models;
    using Radio.Models.Repositories;

    using Timer = System.Timers.Timer;

    public class TrackService : ITrackService, IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IHubContext<RadioHub, IRadioClient> _radioHub;

        private readonly Timer _timer;

        private readonly Stopwatch _stopwatch;

        private Track _currentTrack;

        public TrackService(IHubContext<RadioHub, IRadioClient> radioHub, IServiceProvider serviceProvider, TrackStatusService trackStatusService)
        {
            _serviceProvider = serviceProvider;
            _radioHub = radioHub;
            _timer = new Timer();
            _stopwatch = new Stopwatch();

            trackStatusService.RequestTrackStatus += (sender, e) => e.TrackStatus = CurrentTrackStatus;
        }

        public TrackStatus CurrentTrackStatus => new TrackStatus(_currentTrack.Id, (int)_stopwatch.Elapsed.TotalSeconds);

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Elapsed += async (sender, e) => await NextTrack();
            await NextTrack();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            _stopwatch.Stop();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        private async Task NextTrack()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var trackRepository = scope.ServiceProvider.GetService<ITrackRepository>();

                await trackRepository.MoveToNextTrack();
                _currentTrack = trackRepository.CurrentTrack;
            }

            await _radioHub.Clients.All.UpdateTrack(_currentTrack.Id);

            _timer.Stop();
            _timer.Interval = _currentTrack.Length.TotalMilliseconds;
            _timer.Start();
            _stopwatch.Restart();
        }
    }
}