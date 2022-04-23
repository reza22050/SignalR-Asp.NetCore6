var supportConnection = new signalR.HubConnectionBuilder()
    .withUrl('/supporthub')
    .build();



var chatConnection = new signalR.HubConnectionBuilder()
    .withUrl('/chathub')
    .build();


var activeRoomId = '';

function Init() {
    supportConnection.start();
    chatConnection.start();

    var answerForm = $('#answerForm');
    answerForm.on('submit', function (e) {
        e.preventDefault();

        var text = e.target[0].value;
        e.target[0].value = '';

        sendMessage(text);
    });


}


function sendMessage(text) {

    if (text && text.length) {
        supportConnection.invoke('SendMessage', activeRoomId, text);
    }
}


$(document).ready(function () {
    console.log("ready!");
    Init();
});

supportConnection.on("GetRooms", loadRooms);
supportConnection.on("GetNewMessage", addMessages);

chatConnection.on("GetNewMessage", showMessage);

function addMessages(messages) {
    if (!messages) return;
    messages.forEach(function (m) {
        showMessage(m.sender, m.message, m.time);    

    });

}

function showMessage(sender, message, time) {
    $("#chatMessage").append('<li><div><span class="name"> ' + sender + ' </span><span class="time">' + time + '</span><div><div class="message"> ' + message + ' </div></li>');

}

function loadRooms(rooms) {
    if (!rooms) return;

    var roomIds = Object.keys(rooms);
    if (!roomIds.length) return;
    
    removeAllChildren(roomListEl);

    roomIds.forEach(function (id) {
        var roomInfo = rooms[id];
        if (!roomInfo) return;

        // add button for list
        return $("#roomList").append("<a class='list-group-item list-group-item-action d-flex justify-content-between align-items-center' data-id='" + roomInfo  + "' href= '#'>" + roomInfo + "</a>");

    });

}

var roomListEl = document.getElementById('roomList');
var roomMessageEl = document.getElementById('chatMessage');

function removeAllChildren(node) {
    if (!node) return;

    while (node.lastChild) {
        node.removeChild(node.lastChild);
    }
}

function setActiveRoomButton(el) {

    var allButtons = roomListEl.querySelectorAll('a.list-group-item');

    allButtons.forEach(function (btn) {
        btn.classList.remove('active');
    });
    el.classList.add('active');
}



function switchActiveRoomTo(id) {
    if (id === activeRoomId) return;

    removeAllChildren(roomMessageEl)

    if (activeRoomId) {
        chatConnection.invoke('LeaveRoom', activeRoomId);
    }

    activeRoomId = id;

    chatConnection.invoke('JoinRoom', activeRoomId);
    supportConnection.invoke('LoadMessage', activeRoomId);
}

roomListEl.addEventListener('click', function (e) {
    roomMessageEl.style.display = 'block';
    setActiveRoomButton(e.target);
    var roomId = e.target.getAttribute('data-id');
    switchActiveRoomTo(roomId);
});