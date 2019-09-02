namespace Radio.ApiControllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using Radio.Models;
    using Radio.Models.Repositories;
    using Radio.Models.User;

    [ApiController]
    [Route("Channels/[Action]")]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelRepository _channelRepository;

        public ChannelController(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }

        [HttpGet]
        public IEnumerable<ChannelInfo> All()
        {
            return _channelRepository.ChannelsWithOwners
                                     .Select(channel => new ChannelInfo(channel.Name,
                                                                        channel.Discriminator,
                                                                        channel.Description,
                                                                        new UserThumbnail(channel.Owner)))
                                     .AsEnumerable()
                                     .Reverse();
        }
    }
}