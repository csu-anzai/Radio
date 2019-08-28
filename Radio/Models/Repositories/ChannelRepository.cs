namespace Radio.Models.Repositories
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using Radio.Models.Database;

    public class ChannelRepository : IChannelRepository
    {
        private readonly AppDbContext _appDbContext;

        public ChannelRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IQueryable<Channel> AllChannels => _appDbContext.Channels
                                                               .Include(channel => channel.ChannelTracks)
                                                               .ThenInclude(channelTrack => channelTrack.Track);

        public bool ChannelExists(string channelId)
        {
            return _appDbContext.Channels.Any(channel => channel.Id == channelId);
        }

        public Channel GetChannelOrDefault(string name, ushort discriminator)
        {
            return _appDbContext.Channels.SingleOrDefault(channel => channel.Name == name && channel.Discriminator == discriminator);
        }
    }
}