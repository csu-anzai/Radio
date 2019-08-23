function onYouTubeIframeAPIReady() {
    new YT.Player("player",
        {
            width: "100%",
            height: "100%",
            videoId: "SsQM3c4KPW4",
            playerVars: {
                autoplay: 1,
                controls: 0,
                disablekb: 1,
                fs: 0,
                iv_load_policy: 3
            }
        });

    onYouTubeIframeAPIReady = undefined;
}