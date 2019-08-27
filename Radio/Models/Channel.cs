namespace Radio.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Channel
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public ushort Discriminator { get; set; }

        public ICollection<ChannelTrack> ChannelTracks { get; set; }
    }
}