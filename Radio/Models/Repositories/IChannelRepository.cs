namespace Radio.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IChannelRepository
    {
        IQueryable<Channel> AllChannels { get; }

        Task AddChannel(Channel channel);

        Channel GetChannelOrDefault(string id);

        Channel GetChannel(string name, ushort discriminator);
    }
}