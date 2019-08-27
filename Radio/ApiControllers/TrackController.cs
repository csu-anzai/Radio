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

        private readonly ITrackRepository _trackRepository;

        public TrackController(IChannelRepository channelRepository, ITrackRepository trackRepository)
        {
            _channelRepository = channelRepository;
            _trackRepository = trackRepository;
        }

        [HttpGet("Current/{channelId}")]
        public ActionResult<TrackTitle> Current(string channelId)
        {
            Channel channel = _channelRepository.GetChannelOrDefault(channelId);

            if (channel == null)
            {
                return ChannelNotFound(channelId);
            }

            return _trackRepository.CurrentChannelTrackFor(channel).Track.ToTrackTitle();
        }

        [HttpGet("Queue/{channelId}")]
        public ActionResult<IEnumerable<TrackTitle>> Queue(string channelId)
        {
            Channel channel = _channelRepository.GetChannelOrDefault(channelId);

            if (channel == null)
            {
                return ChannelNotFound(channelId);
            }

            return Ok(_trackRepository.ChannelTrackQueueFor(channel)
                                      .Select(channelTrack => channelTrack.Track.ToTrackTitle()));
        }

        private ActionResult ChannelNotFound(string channelId)
        {
            return NotFound($"Channel ID '{channelId}' does not exist.");
        }
    }
}