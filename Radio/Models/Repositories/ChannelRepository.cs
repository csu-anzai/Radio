namespace Radio.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Radio.Models.Database;

    public class ChannelRepository : IChannelRepository
    {
        private readonly AppDbContext _appDbContext;

        public ChannelRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private IQueryable<Channel> Channels => _appDbContext.Channels;

        public async Task AddChannel(Channel channel)
        {
            _appDbContext.Channels.Add(channel);
            await _appDbContext.SaveChangesAsync();
        }

        public Channel GetChannelOrDefault(string id)
        {
            return Channels.SingleOrDefault(channel => channel.Id == id);
        }

        public Channel GetChannel(string name, ushort discriminator = 0)
        {
            return Channels.Single(channel => channel.Name == name && channel.Discriminator == discriminator);
        }
    }
}