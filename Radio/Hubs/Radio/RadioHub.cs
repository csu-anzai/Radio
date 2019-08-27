namespace Radio.Hubs.Radio
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using global::Radio.Models;
    using global::Radio.Models.Repositories;
    using global::Radio.Services;

    using Microsoft.AspNetCore.SignalR;

    public class RadioHub : Hub<IRadioClient>
    {
        private static readonly ConcurrentDictionary<string, string> ClientChannelIds = new ConcurrentDictionary<string, string>();

        private readonly IChannelRepository _channelRepository;

        private readonly ITrackStatusService _trackStatusService;

        public RadioHub(IChannelRepository channelRepository, ITrackStatusService trackStatusService)
        {
            _channelRepository = channelRepository;
            _trackStatusService = trackStatusService;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ClientChannelIds.TryRemove(Context.ConnectionId, out var _);

            return Task.CompletedTask;
        }

        public async Task Negotiate(string channelName, ushort channelDiscriminator)
        {
            if (channelName == "null") // SignalR client-side bug which doesn't permit null values
            {
                channelName = null;
            }

            Channel channel = _channelRepository.GetChannel(channelName, channelDiscriminator);

            ClientChannelIds[Context.ConnectionId] = channel.Id;
            await Groups.AddToGroupAsync(Context.ConnectionId, channel.Id);

            await Clients.Caller.ChannelIdInfo(channel.Id);
            await SyncClientToCurrentVideo();
        }

        public async Task Played()
        {
            await SyncClientToCurrentVideo();
        }

        private async Task SyncClientToCurrentVideo()
        {
            TrackStatus currentTrackStatus = _trackStatusService.CurrentTrackStatusFor(ClientChannelIds[Context.ConnectionId]);
            await Clients.Caller.SyncVideo(currentTrackStatus.CurrentTrackId, currentTrackStatus.TimeStampSeconds);
        }
    }
}