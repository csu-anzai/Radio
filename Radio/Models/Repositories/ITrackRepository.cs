namespace Radio.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITrackRepository
    {
        Track CurrentTrack { get; }

        IQueryable<Track> TrackQueue { get; }

        Task AddTrack(Track track);

        Task MoveToNextTrack();
    }
}