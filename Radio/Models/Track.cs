﻿namespace Radio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Track
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public TimeSpan Length { get; set; }

        public ICollection<ChannelTrack> ChannelTracks { get; set; }

        public TrackTitle ToTrackTitle()
        {
            return new TrackTitle(Title);
        }
    }
}