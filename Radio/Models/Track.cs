namespace Radio.Models
{
    public class Track
    {
        public Track(string id, int timeStampSeconds)
        {
            Id = id;
            TimeStampSeconds = timeStampSeconds;
        }

        public string Id { get; }

        public int TimeStampSeconds { get; }
    }
}