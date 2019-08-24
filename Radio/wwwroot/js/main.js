function onYouTubeIframeAPIReady() {
    onYouTubeIframeAPIReady = undefined;

    let player;
    let playing = false;
    let currentTrackId;

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/radio")
        .build();

    connection.on("UpdateTrack",
        function(trackId) {
            if (playing) {
                player.loadVideoById(trackId);
                currentTrackId = trackId;
                updateTrackList();
            }
        });

    connection.on("SyncVideo",
        function(trackId, timeStampSeconds) {
            if (currentTrackId === trackId) {
                player.seekTo(timeStampSeconds);
            } else {
                player.loadVideoById(trackId, timeStampSeconds);
                currentTrackId = trackId;
            }

            updateTrackList();
        });

    player = new YT.Player("player",
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
        } else if (event.data === YT.PlayerState.PAUSED) {
            playing = false;
        }
    }

    const trackListDiv = $("div.track-list");
    const trackList = trackListDiv.find("ul");

    function updateTrackList() {
        function getCurrentTrack(callback) {
            $.get("/Track/Current", callback);
        }

        function getTrackQueue(callback) {
            $.get("/Track/Queue", callback);
        }

        getCurrentTrack(function(currentTrack) {
            getTrackQueue(function(trackQueue) {
                trackList.empty();

                function addNewTrackItem(classes, title) {
                    trackList.append($(`<li class="${classes}">${title}</li>`));
                }

                addNewTrackItem("list-group-item active rounded-0", currentTrack.title);

                for (const track of trackQueue) {
                    addNewTrackItem("list-group-item", track.title);
                }
            });
        });
    }

    (function() {
        const main = $("main");
        const trackListToggleListItem = $("#track-list-toggle");
        const trackListToggleLink = trackListToggleListItem.find("a");

        let active = false;

        function toggleTrackListVisibility() {
            if (active) {
                main.removeClass("video-and-track-list");
                main.addClass("video-only");
                trackListToggleListItem.removeClass("active");
                trackListDiv.css("display", "none");
            } else {
                main.addClass("video-and-track-list");
                main.removeClass("video-only");
                trackListToggleListItem.addClass("active");
                trackListDiv.css("display", "");
            }

            active = !active;
        }

        toggleTrackListVisibility();

        trackListToggleLink.click(function(event) {
            toggleTrackListVisibility();
            event.preventDefault();
        });
    })();
}