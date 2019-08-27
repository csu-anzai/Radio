namespace Radio.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Track
    {
        public Track()
        {
        }

        public Track(string id, string title, TimeSpan length)
        {
            Id = id;
            Title = title;
            Length = length;
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public TimeSpan Length { get; set; }

        public int Order { get; set; }

        public TrackTitle ToTrackTitle()
        {
            return new TrackTitle(Title);
        }
    }
}