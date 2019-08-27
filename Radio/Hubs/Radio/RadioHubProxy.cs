namespace Radio.Hubs.Radio
{
    using System.Threading.Tasks;

    using global::Radio.Models;

    using Microsoft.AspNetCore.SignalR;

    public class RadioHubProxy : IRadioHubProxy
    {
        private readonly IHubContext<RadioHub, IRadioClient> _radioHub;

        public RadioHubProxy(IHubContext<RadioHub, IRadioClient> radioHub)
        {
            _radioHub = radioHub;
        }

        public Task UpdateTrack(Channel channel, Track track)
        {
            return _radioHub.Clients.Group(channel.Id).UpdateTrack(track.Id);
        }
    }
}