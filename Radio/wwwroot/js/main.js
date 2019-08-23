function onYouTubeIframeAPIReady() {
    const player = new YT.Player("player",
        {
            width: "100%",
            height: "100%",
            playerVars: {
                controls: 1,
                disablekb: 1,
                fs: 0,
                iv_load_policy: 3
            },
            events: {
                onReady,
                onStateChange
            }
        });
    let playing = false;

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/radio")
        .build();

    connection.on("UpdateTrack", function (trackId) {
        player.loadVideoById(trackId);
    });

    connection.on("SyncTimeStamp", function (timeStampSeconds) {
        player.seekTo(timeStampSeconds);
    });

    connection.on("SyncVideo", function (trackId, timeStampSeconds) {
        player.loadVideoById(trackId, timeStampSeconds);
    });

    function onReady() {
        connection.start()
            .then(function() {
                connection.invoke("Connected");
            });
    }

    function onStateChange(event) {
        if (event.data === YT.PlayerState.PLAYING) {
            if (!playing) {
                connection.invoke("Played");
            }

            playing = true;
        }

        if (event.data === YT.PlayerState.PAUSED) {
            playing = false;
        }
    }

    onYouTubeIframeAPIReady = undefined;
}