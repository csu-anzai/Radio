namespace Radio.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    public class RadioHub : Hub
    {
        public async Task UpdateTrack(string trackId)
        {
            await Clients.All.SendAsync(nameof(UpdateTrack), trackId);
        }
    }
}