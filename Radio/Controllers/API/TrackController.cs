namespace Radio.Controllers.API
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using Radio.Models;
    using Radio.Services;

    [Route("[Controller]")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackQueue _trackQueue;

        private readonly ITrackService _trackService;

        public TrackController(ITrackQueue trackQueue, ITrackService trackService)
        {
            _trackQueue = trackQueue;
            _trackService = trackService;
        }

        [HttpGet("Current")]
        public Track Current()
        {
            return _trackService.CurrentTrack;
        }

        [HttpGet("Queue")]
        public IEnumerable<Track> Queue()
        {
            return _trackQueue.AllTracks;
        }
    }
}