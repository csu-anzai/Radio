﻿namespace Radio.Services
{
    using System;

    using Radio.EventArgs;
    using Radio.Models;

    public interface ITrackService
    {
        event EventHandler<TrackUpdatedEventArgs> TrackUpdated;

        TrackStatus CurrentTrackStatus { get; }
    }
}