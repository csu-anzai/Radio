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
        private readonly IChannelRepository _channelRepository;

        private readonly IChannelTrackRepository _channelTrackRepository;

        public TrackController(IChannelRepository channelRepository, IChannelTrackRepository channelTrackRepository)
        {
            _channelRepository = channelRepository;
            _channelTrackRepository = channelTrackRepository;
        }

        [HttpGet("Current/{channelId}")]
        public ActionResult<TrackTitle> Current(string channelId)
        {
            if (!ChannelExists(channelId))
            {
                return ChannelNotFound(channelId);
            }

            return _channelTrackRepository.CurrentChannelTrackFor(channelId)
                                          .Track
                                          .ToTrackTitle();
        }

        [HttpGet("Queue/{channelId}")]
        public ActionResult<IEnumerable<TrackTitle>> Queue(string channelId)
        {
            if (!ChannelExists(channelId))
            {
                return ChannelNotFound(channelId);
            }

            return Ok(_channelTrackRepository.ChannelTrackQueueFor(channelId)
                                             .Select(channelTrack => channelTrack.Track.ToTrackTitle())
            );
        }

        private bool ChannelExists(string channelId)
        {
            return _channelRepository.ChannelExists(channelId);
        }

        private ActionResult ChannelNotFound(string channelId)
        {
            return NotFound($"Channel ID '{channelId}' does not exist.");
        }
    }
}