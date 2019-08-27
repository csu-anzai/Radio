namespace Radio.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ChannelTrack
    {
        [ForeignKey(nameof(Channel))]
        public string ChannelId { get; set; }

        public Channel Channel { get; set; }

        [ForeignKey(nameof(Track))]
        public string TrackId { get; set; }

        public Track Track { get; set; }

        public uint Order { get; set; }
    }
}