namespace Radio.Hubs.Radio
{
    using System.Threading.Tasks;

    public interface IRadioClient
    {
        Task ChannelIdInfo(string channelId);

        Task SyncVideo(string trackId, int timeStampSeconds);

        Task UpdateTrack(string trackId);
    }
}