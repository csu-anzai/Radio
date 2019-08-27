namespace Radio.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITrackRepository
    {
        IQueryable<ChannelTrack> AllChannelTracksFor(Channel channel);

        ChannelTrack CurrentChannelTrackFor(Channel channel);

        IQueryable<ChannelTrack> ChannelTrackQueueFor(Channel channel);

        Task AddTrack(Track track, Channel channel);

        Task MoveToNextChannelTrack(Channel channel);
    }
}