namespace Radio.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Radio.Models.Database;

    public class TrackRepository : ITrackRepository
    {
        private readonly AppDbContext _appDbContext;

        public TrackRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Track CurrentTrack => Tracks.Single(track => track.Order == 0);

        public IQueryable<Track> TrackQueue => Tracks.Where(track => track.Order > 0).OrderBy(track => track.Order);

        private IQueryable<Track> Tracks => _appDbContext.Tracks;

        public async Task AddTrack(Track track)
        {
            track.Order = Tracks.Count();
            _appDbContext.Tracks.Add(track);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task MoveToNextTrack()
        {
            Track currentTrack = CurrentTrack;
            currentTrack.Order = Tracks.Count() - 1;
            _appDbContext.Tracks.Update(currentTrack);

            IQueryable<Track> trackQueue = TrackQueue;
            foreach (Track track in trackQueue)
            {
                --track.Order;
            }
            _appDbContext.Tracks.UpdateRange(trackQueue);

            await _appDbContext.SaveChangesAsync();
        }
    }
}