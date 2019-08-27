namespace Radio.ApiControllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using Radio.Models;
    using Radio.Models.Repositories;

    [ApiController]
    [Route("[Controller]")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackRepository _trackRepository;

        public TrackController(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        [HttpGet("Current")]
        public TrackTitle Current()
        {
            return _trackRepository.CurrentTrack.ToTrackTitle();
        }

        [HttpGet("Queue")]
        public IEnumerable<TrackTitle> Queue()
        {
            return _trackRepository.TrackQueue.Select(track => track.ToTrackTitle());
        }
    }
}