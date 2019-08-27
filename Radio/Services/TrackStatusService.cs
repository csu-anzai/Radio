namespace Radio.Services
{
    using System;

    using Radio.Models;

    public class TrackStatusEventArgs : EventArgs
    {
        public TrackStatusEventArgs(string channelId)
        {
            ChannelId = channelId;
        }

        public string ChannelId { get; }

        public TrackStatus TrackStatus { get; set; }
    }

    public class TrackStatusService : ITrackStatusService
    {
        public event EventHandler<TrackStatusEventArgs> RequestTrackStatus;

        public TrackStatus CurrentTrackStatusFor(string channelId)
        {
            TrackStatusEventArgs trackStatusEventArgs = new TrackStatusEventArgs(channelId);
            RequestTrackStatus.Invoke(this, trackStatusEventArgs);

            return trackStatusEventArgs.TrackStatus;
        }
    }
}