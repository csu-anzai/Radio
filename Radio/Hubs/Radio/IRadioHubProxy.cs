namespace Radio.Hubs.Radio
{
    using System.Threading.Tasks;

    using global::Radio.Models;

    public interface IRadioHubProxy
    {
        Task UpdateTrack(Channel channel, Track track);
    }
}