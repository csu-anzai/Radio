namespace Radio.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Radio.Hubs.Radio;
    using Radio.Models;
    using Radio.Models.Repositories;

    public class TrackService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IRadioHubProxy _radioHubProxy;

        private readonly TrackStatusService _trackStatusService;

        private readonly ConcurrentDictionary<string, TrackTracker> _trackTrackerForChannel = new ConcurrentDictionary<string, TrackTracker>();

        public TrackService(IServiceProvider serviceProvider, IRadioHubProxy radioHubProxy, TrackStatusService trackStatusService)
        {
            _serviceProvider = serviceProvider;
            _radioHubProxy = radioHubProxy;
            _trackStatusService = trackStatusService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IChannelRepository channelRepository = scope.ServiceProvider.GetService<IChannelRepository>();

                foreach (Channel channel in channelRepository.AllChannels)
                {
                    _trackTrackerForChannel[channel.Id] = new TrackTracker(_serviceProvider, _radioHubProxy, channel);
                }
            }

            foreach (TrackTracker trackTracker in _trackTrackerForChannel.Values)
            {
                await trackTracker.Start();
            }

            _trackStatusService.RequestTrackStatus += (sender, e) => e.TrackStatus = _trackTrackerForChannel[e.ChannelId].CurrentTrackStatus;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (TrackTracker trackTracker in _trackTrackerForChannel.Values)
            {
                await trackTracker.Stop();
            }
        }
    }
}