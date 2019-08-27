namespace Radio.Services
{
    using System;

    using Radio.Models;

    public class TrackStatusEventArgs : EventArgs
    {
        public TrackStatus TrackStatus { get; set; }
    }

    public class TrackStatusService : ITrackStatusService
    {
        public TrackStatus CurrentTrackStatus
        {
            get
            {
                TrackStatusEventArgs trackStatusEventArgs = new TrackStatusEventArgs();
                RequestTrackStatus?.Invoke(this, trackStatusEventArgs);

                return trackStatusEventArgs.TrackStatus;
            }
        }

        public event EventHandler<TrackStatusEventArgs> RequestTrackStatus;
    }
}