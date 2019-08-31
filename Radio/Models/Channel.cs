namespace Radio.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Radio.Models.User;

    public class Channel
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public ushort Discriminator { get; set; }

        [Required]
        [ForeignKey(nameof(Owner))]
        public string OwnerId { get; set; }

        public AppUser Owner { get; set; }

        public ICollection<ChannelTrack> ChannelTracks { get; set; }
    }
}