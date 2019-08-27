namespace Radio.Hubs
{
    using System.Threading.Tasks;

    public interface IRadioClient
    {
        Task SyncVideo(string trackId, int timeStampSeconds);

        Task UpdateTrack(string trackId);
    }
}