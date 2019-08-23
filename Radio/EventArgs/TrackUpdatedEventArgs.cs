namespace Radio.EventArgs
{
    using System;

    public class TrackUpdatedEventArgs : EventArgs
    {
        public TrackUpdatedEventArgs(string newTrackId)
        {
            NewTrackId = newTrackId;
        }

        public string NewTrackId { get; }
    }
}