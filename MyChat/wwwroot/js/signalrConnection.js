const chatroom = document.getElementById('chatroom');
const connection = new signalR.HubConnectionBuilder()
    .withUrl('/chat')
    .build();

connection.on('Send', function (userName, messageText, avatarPath) {
    addMessageFirst(createMessageRow(userName, messageText, avatarPath));
    messagesOnPage++; 
});

document.getElementById("sendBtn").addEventListener("click", function (e) {
    let input = document.getElementById("message");

    let messageText = input.value;

    if (message.trim() === '') {
        return;
    }
    input.value = '';
    let message = new Message(currentUserName,messageText,new Date())
    connection.invoke("Send", message);
    SendMesageToServer(message);
});

connection.start()
    .catch(error => {
        console.error(error.message);
    });


