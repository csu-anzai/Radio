namespace Radio.Models
{
    using Radio.Models.User;

    public class ChannelInfo
    {
        public ChannelInfo(string name, ushort discriminator, string description, UserThumbnail owner)
        {
            Name = name;
            Discriminator = discriminator;
            Description = description;
            Owner = owner;
        }

        public string Name { get; }

        public ushort Discriminator { get; }

        public string Description { get; }

        public UserThumbnail Owner { get; }
    }
}