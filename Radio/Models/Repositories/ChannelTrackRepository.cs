namespace Radio.Models.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Radio.Models.Database;

    public class ChannelTrackRepository : IChannelTrackRepository
    {
        private readonly AppDbContext _appDbContext;

        public ChannelTrackRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private IQueryable<Channel> Channels => _appDbContext.Channels
                                                             .Include(channel => channel.ChannelTracks)
                                                             .ThenInclude(channelTrack => channelTrack.Track);

        public ChannelTrack CurrentChannelTrackFor(string channelId)
        {
            return ChannelById(channelId).ChannelTracks.Single(channelTrack => channelTrack.Order == 0);
        }

        public ChannelTrack CurrentChannelTrackFor(Channel channel)
        {
            return channel.ChannelTracks.Single(channelTrack => channelTrack.Order == 0);
        }

        public IEnumerable<ChannelTrack> ChannelTrackQueueFor(string channelId)
        {
            return ChannelById(channelId).ChannelTracks.Where(channelTrack => channelTrack.Order > 0).OrderBy(channelTrack => channelTrack.Order);
        }

        public async Task MoveToNextTrack(Channel channel)
        {
            ICollection<ChannelTrack> channelTracks = channel.ChannelTracks;
            ChannelTrack currentChannelTrack = channelTracks.Single(channelTrack => channelTrack.Order == 0);

            foreach (ChannelTrack channelTrack in channelTracks)
            {
                --channelTrack.Order;
            }
            currentChannelTrack.Order = (uint)(channelTracks.Count - 1);

            _appDbContext.Channels.Update(channel);
            await _appDbContext.SaveChangesAsync();
        }

        private Channel ChannelById(string channelId)
        {
            return Channels.Single(channel => channel.Id == channelId);
        }
    }
}