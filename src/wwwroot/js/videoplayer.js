function loadPlayer(id, options) {
    let player;
    let players = videojs.players;
    if (players && Object.keys(players).length) {
        player = players[id];
        player.dispose();
    }
    player = videojs(id, options);
    var videoid = document.getElementsByClassName("vjs-control-bar");
    videoid[0].style.setProperty("display", "none", "important");
}