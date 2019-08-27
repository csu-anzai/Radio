namespace Radio.Models.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Radio.Models.Database;

    public class TrackRepository : ITrackRepository
    {
        private readonly AppDbContext _appDbContext;

        public TrackRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private IQueryable<Track> Tracks => _appDbContext.Tracks;

        public IQueryable<ChannelTrack> AllChannelTracksFor(Channel channel)
        {
            return Tracks.Include(track => track.ChannelTracks)
                         .ThenInclude(channelTrack => channelTrack.Track)
                         .Select(track => track.ChannelTracks.Single(channelTrack => channelTrack.ChannelId == channel.Id));
        }

        public ChannelTrack CurrentChannelTrackFor(Channel channel)
        {
            return AllChannelTracksFor(channel).Single(channelTrack => channelTrack.Order == 0);
        }

        public IQueryable<ChannelTrack> ChannelTrackQueueFor(Channel channel)
        {
            return AllChannelTracksFor(channel).Where(channelTrack => channelTrack.Order > 0).OrderBy(channelTrack => channelTrack.Order);
        }

        public async Task AddTrack(Track track, Channel channel)
        {
            if (track.ChannelTracks == null)
            {
                track.ChannelTracks = new List<ChannelTrack>();
            }

            track.ChannelTracks.Add(new ChannelTrack
            {
                ChannelId = channel.Id,
                TrackId = track.Id,
                Order = (uint)AllChannelTracksFor(channel).Count()
            });

            _appDbContext.Tracks.Add(track);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task MoveToNextChannelTrack(Channel channel)
        {
            ChannelTrack currentChannelTrack = CurrentChannelTrackFor(channel);
            currentChannelTrack.Order = (uint)(AllChannelTracksFor(channel).Count() - 1);

            IQueryable<ChannelTrack> channelTrackQueue = ChannelTrackQueueFor(channel);
            foreach (ChannelTrack channelTrack in channelTrackQueue)
            {
                --channelTrack.Order;
            }

            _appDbContext.Channels.Update(channel);
            await _appDbContext.SaveChangesAsync();
        }
    }
}