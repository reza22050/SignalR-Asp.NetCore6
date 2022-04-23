var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

connection.start();

connection.invoke('SendNewMessage', "Site visitor", "This message is sent from client");

$(document).ready(function () {
    Init();
});

function Init() {

    var NewMessageForm = $('#NewMessageForm');

    NewMessageForm.on("submit", function (e) {
        e.preventDefault();
        var message = e.target[0].value;
        e.target[0].value = '';
        sendMessage(message);
    });
}

// Send message to server
function sendMessage(text) {
    connection.invoke('SendNewMessage', 'Site client', text);
};

// Get message from server
connection.on("GetNewMessage", getMessage);
function getMessage(sender, message, time) {
    $("#Messages").append(" <div class='media media-chat'> <img class='avatar' src='https://img.icons8.com/color/36/000000/administrator-male.png' alt='...'><div class='media-body'><p>" + message + "</p><p class='meta'><span class='meta'> " + sender + "</span></p><p class='meta'><time datetime='2022'>" + time + "</time></p></div></div >");
};


