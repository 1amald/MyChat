const connection = new signalR.HubConnectionBuilder()
    .withUrl('/chat')
    .build();

connection.on('Send', function (message) {
    addMessageFirst(createMessageRow(message));
    messagesOnPage++; 
});

submit.addEventListener("click", OnSubmitClick);

connection.start()
    .catch(error => {
        console.error(error.message);
    });


