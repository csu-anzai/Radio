namespace Radio.Models.Repositories
{
    using System.Linq;

    public interface IChannelRepository
    {
        IQueryable<Channel> AllChannels { get; }

        IQueryable<Channel> ChannelsWithOwners { get; }

        bool ChannelExists(string channelId);

        Channel GetChannelOrDefault(string name, ushort discriminator);
    }
}