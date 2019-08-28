namespace Radio.Models.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IChannelTrackRepository
    {
        ChannelTrack CurrentChannelTrackFor(string channelId);

        ChannelTrack CurrentChannelTrackFor(Channel channel);

        IEnumerable<ChannelTrack> ChannelTrackQueueFor(string channelId);

        Task MoveToNextTrack(Channel channel);
    }
}