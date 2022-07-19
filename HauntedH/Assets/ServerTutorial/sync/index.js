var io = require('socket.io')(process.env.PORT || 8080);

var Player = require("./Classes/player.js");
console.log("Server has started");
var players =[];
var sockets =[];
io.on('connection',function(socket){
    console.log("connection made!");

    var player = new Player();
    var thisPlayerID = player.id;

    players[thisPlayerID] = player;
    sockets[thisPlayerID] = socket;

    socket.emit('register',{id : thisPlayerID});
    socket.emit('spawn',player);
    socket.broadcast.emit('spawn',player);

    for(var playerID in players){
        if(playerID != thisPlayerID){
            socket.emit('spawn',players[playerID]);
        }
    }
    socket.on("disconnect",function(){
        console.log("A player has disconnected");
        delete players[thisPlayerID];
        delete sockets[thisPlayerID];
    });
});