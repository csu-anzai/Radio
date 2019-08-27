namespace Radio.Models.Repositories
{
    using System.Threading.Tasks;

    public interface IChannelRepository
    {
        Task AddChannel(Channel channel);

        Channel GetChannelOrDefault(string id);

        Channel GetChannel(string name, ushort discriminator);
    }
}