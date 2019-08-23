namespace Radio.Models
{
    public class TrackStatus
    {
        public TrackStatus(string currentTrackId, int timeStampSeconds)
        {
            CurrentTrackId = currentTrackId;
            TimeStampSeconds = timeStampSeconds;
        }

        public string CurrentTrackId { get; }

        public int TimeStampSeconds { get; }
    }
}