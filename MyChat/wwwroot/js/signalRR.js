const chatroom = document.getElementById('chatroom');
const connection = new signalR.HubConnectionBuilder()
    .withUrl('/chat')
    .build();

connection.on('Send', function (userName, messageText, avatarPath) {
    addMessageToChat(userName, messageText, avatarPath);
    SendMesageToServer(userName, messageText);
});

document.getElementById("sendBtn").addEventListener("click", function (e) {
    let input = document.getElementById("message")
    let message = input.value;
    if (message == '') {
        return;
    }
    input.value = '';
    connection.invoke("Send", currentUserName, message);
});

connection.start()
    .catch(error => {
        console.error(error.message);
    });

function addMessageToChat(name, text, avatar) {
    var currentdate = new Date();
    let dateString = currentdate.toLocaleString('ru-RU', { hour: 'numeric', minute: 'numeric', hour12: false });
    //перечисление элементов сообщения
    let row = document.createElement('div');
    let img = document.createElement('img');
    let messageBlock = document.createElement('div');
    let divUserName = document.createElement('div');
    let spanForUserName = document.createElement('span');
    let spanForText = document.createElement('span');
    let spanWhen = document.createElement('span');
    let aImg = document.createElement('a');

    messageBlock.className = 'message-block-chat';//классы, которые не зависят от userName == currentUserName
    divUserName.className = 'user-name-chat';
    spanForText.className = 'message-text-chat';
    spanWhen.className = 'message-when-chat';

    spanForText.innerHTML = text;
    spanWhen.innerHTML = dateString;

    messageBlock.appendChild(divUserName);
    divUserName.appendChild(spanForUserName);
    messageBlock.appendChild(spanForText);

    if (name == currentUserName) {
        row.className = 'row-right-chat';
        row.appendChild(spanWhen);
        row.appendChild(messageBlock);
        spanForUserName.innerHTML = 'Вы';
    }
    else {
        row.className = 'row-chat';
        aImg.href = 'https://localhost:44354/Home/Profile?name=' + name;;
        aImg.appendChild(img);
        row.appendChild(aImg);
        row.appendChild(messageBlock);
        row.appendChild(spanWhen);
        spanForUserName.innerHTML = name;
        img.src = avatar;
        img.className = 'image-chat';
    }
    var firstChild = chatroom.firstChild;
    chatroom.insertBefore(row, firstChild);
}
function SendMesageToServer(name, text) {
    var formData = new FormData();
    formData.append('sender', name);
    formData.append('text', text);
    var request = new XMLHttpRequest();
    request.open('POST', '@Url.Action("Create","Home")');
    request.send(formData);
}
