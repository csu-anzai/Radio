namespace Radio.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Timers;

    using Microsoft.Extensions.DependencyInjection;

    using Radio.Hubs.Radio;
    using Radio.Models;
    using Radio.Models.Repositories;

    public class TrackTracker
    {
        private readonly Timer _timer = new Timer();

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly IServiceProvider _serviceProvider;

        private readonly IRadioHubProxy _radioHubProxy;

        private readonly Channel _currentChannel;

        private Track _currentTrack;

        public TrackTracker(IServiceProvider serviceProvider, IRadioHubProxy radioHubProxy, Channel currentChannel)
        {
            _serviceProvider = serviceProvider;
            _radioHubProxy = radioHubProxy;
            _currentChannel = currentChannel;
        }

        public TrackStatus CurrentTrackStatus => new TrackStatus(_currentTrack.Id, (int)_stopwatch.Elapsed.TotalSeconds);

        public async Task Start()
        {
            _timer.Elapsed += async (sender, e) => await NextTrack();
            await NextTrack();
        }

        public Task Stop()
        {
            _timer.Stop();
            _stopwatch.Stop();

            return Task.CompletedTask;
        }

        private async Task NextTrack()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IChannelTrackRepository channelTrackRepository = scope.ServiceProvider.GetService<IChannelTrackRepository>();

                await channelTrackRepository.MoveToNextTrack(_currentChannel);
                _currentTrack = channelTrackRepository.CurrentChannelTrackFor(_currentChannel).Track;

                await _radioHubProxy.UpdateTrack(_currentChannel, _currentTrack);
            }

            _timer.Stop();
            _timer.Interval = _currentTrack.Length.TotalMilliseconds;
            _timer.Start();
            _stopwatch.Restart();
        }
    }
}