namespace Radio.Models.Database
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Radio.Models.User;

    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Channel> Channels { get; protected set; }

        public virtual DbSet<Track> Tracks { get; protected set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChannelTrack>()
                   .HasKey(channelTrack => new
                   {
                       channelTrack.ChannelId,
                       channelTrack.TrackId
                   });

            builder.Entity<ChannelTrack>()
                   .HasOne(channelTrack => channelTrack.Track)
                   .WithMany(track => track.ChannelTracks)
                   .HasForeignKey(channelTrack => channelTrack.TrackId);

            builder.Entity<ChannelTrack>()
                   .HasOne(channelTrack => channelTrack.Channel)
                   .WithMany(channel => channel.ChannelTracks)
                   .HasForeignKey(channelTrack => channelTrack.ChannelId);

            builder.Entity<Channel>()
                   .HasIndex(channel => new
                   {
                       channel.Name,
                       channel.Discriminator
                   })
                   .IsUnique();
        }
    }
}