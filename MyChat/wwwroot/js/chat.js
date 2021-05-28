let messagesOnPage = 0;
let fullDialog = false;
GetMoreMessages();



function addMessageFirst(row) {
    
    var firstChild = chatroom.firstChild;
    chatroom.insertBefore(row, firstChild);
    messagesOnPage++;
}

function addMessageLast(row) {
    chatroom.appendChild(row);
    messagesOnPage++;
}

function createMessageRow(message) {
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

    spanForText.innerHTML = message.text;
    spanWhen.innerHTML = message.shortDate;

    messageBlock.appendChild(divUserName);
    divUserName.appendChild(spanForUserName);
    messageBlock.appendChild(spanForText);

    if (message.userName == currentUserName) {
        row.className = 'row-right-chat';
        row.appendChild(spanWhen);
        row.appendChild(messageBlock);
        spanForUserName.innerHTML = 'You';
    }
    else {
        row.className = 'row-chat';
        aImg.href = 'https://localhost:44354/Home/Profile?name=' + message.userName;;
        aImg.appendChild(img);
        row.appendChild(aImg);
        row.appendChild(messageBlock);
        row.appendChild(spanWhen);
        spanForUserName.innerHTML = message.userName;
        img.src = message.avatarPath;
        img.className = 'image-chat';
    }
    return row;
}

function GetMoreMessages() {
    let formdata = new FormData();
    formdata.append('skipCount', messagesOnPage);
    formdata.append('takeCount', 15);
    fetch("https://localhost:44354/Home/GetMessages", {
        method: 'post',
        body: formdata
    }).then(response => response.text())
        .then(result => parseAndAdd(result));
}

function parseAndAdd(json) {
    let array = JSON.parse(json);
    array.forEach(function (message) {
        addMessageLast(createMessageRow(message));
    });
}

function OnSubmitClick() {
    let messageText = input.value;
    if (messageText.trim() === '') {
        return;
    }
    input.value = '';
    connection.invoke("Send", messageText);
}

let chatWindow = document.getElementById("chatroom");

chatWindow.addEventListener('scroll', function () {
    if (chatWindow.scrollHeight + chatWindow.scrollTop - chatWindow.clientHeight < 1) {
        GetMoreMessages();
    }
});

document.addEventListener('keyup', function (event) {
    if (event.key == 'Enter') {
        if (!event.shiftKey) {
            document.querySelector('#sendBtn').click();
            let input = document.getElementById("message");
            input.value = '';
        }
    }
});

