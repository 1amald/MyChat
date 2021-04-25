function addMessageFirst(row) {
    
    var firstChild = chatroom.firstChild;
    chatroom.insertBefore(row, firstChild);
}

function addMessageLast(row) {
    chatroom.appendChild(row);
}

function createMessageRow(name,text,avatar) {
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
    return row;
}

function SendMesageToServer(message) {
    var formData = new FormData();
    formData.append('message', message);
    var request = new XMLHttpRequest();
    request.open('POST', 'https://localhost:44354/Home/Create');
    request.send(formData);
}

document.addEventListener('keyup', function (event) {
    if (event.key == 'Enter') {
        if (!event.shiftKey) {
            document.querySelector('#sendBtn').click();
            let input = document.getElementById("message");
            input.value = '';
        }
    }
});

function GetMoreMessages() {
    var formdata = new FormData();
    formdata.append('skipCount', messagesOnPage);
    formdata.append('takeCount', 30);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', 'https://localhost:44354/Home/GetMoreMessages', true);
    xhr.send(formdata);

    xhr.onreadystatechange = function () {
        if (xhr.status != 200) { // анализируем HTTP-статус ответа, если статус не 200, то произошла ошибка
            alert(`Ошибка ${xhr.status}: ${xhr.statusText}`); // Например, 404: Not Found
        } else { // если всё прошло гладко, выводим результат
            alert(xhr.responseText);
            var messages = JSON.parse(xhr.responseText);
            messages.forEach(function (message) {
                addMessageLast(createMessageRow(message.senderName, message.text, message.avatarPath));
            });
            messagesOnPage += messages.lenght;
        }
    };
}

class Message {
    constructor(username, text, when) {
        this.userName = username;
        this.text = text;
        this.when = when;
    }
}