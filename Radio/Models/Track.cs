namespace Radio.Models
{
    using System;

    public class Track
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public TimeSpan Length { get; set; }
    }
}