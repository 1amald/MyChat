var basic;
$('#success').hide();
$('#danger').hide();

document.getElementById('sendStatus').addEventListener('click', function () {
    SendStatus();
});
document.getElementById('sendPassword').addEventListener('click', function () {
    SendPassword();
});
$('.actionSelect input').on('change', function () { showCrop(); readFile(this); });

function showCrop() {
    basic = $('#main-cropper').croppie({
        viewport: { width: 400, height: 400 },
        boundary: { width: 450, height: 450 },
        showZoomer: false
    });
}
function readFile(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#main-cropper').croppie('bind', {
                url: e.target.result
            });
        }

        reader.readAsDataURL(input.files[0]);
    }
}
function ShowOk() {
    $('#success').show('fade');
}
function ShowError(text) {
    if (text != undefined) {
        document.getElementById('spanDanger').innerHTML = text;
    }
    else {
        document.getElementById('spanDanger').innerHTML = 'Изменения не сохранены.';
    }
    $('#danger').show('fade');
}

$('.close').on('click', function () {
    $('.my-class').hide('fade');
});

$('.actionUpload').on('click', function () {
    basic.croppie('result', 'blob').then(async function (blob) {
        var formData = new FormData();
        formData.append('filename', 'test.jpg');
        formData.append('blob', blob);

        fetch('https://localhost:44354/Account/SetAvatar', {
            method: 'post',
            body: formData
        }).then(function (response) {
            if (response.ok) {
                ShowOk();
            }
            else {
                ShowError('Что-то пошло не так :с');
            }
        });
    });
});

function SendStatus() {
    let statusElement = document.getElementById('inputStatus');
    let statusText = statusElement.value;
    if (statusText.lenght > 300) {
        ShowError();
    }

    fd = new FormData();
    fd.append('status', statusText);

    fetch('https://localhost:44354/Account/SetStatus', {
        method: 'post',
        body: fd
    }).then(function (response) {
        if (response.ok) {
            ShowOk();
        }
        else {
            ShowError('Что-то пошло не так :с');
        }
    });
}

function SendPassword() {
    let currentPassword = document.getElementById('currentPassword').value;
    let newPassword = document.getElementById('newPassword').value;
    let confirmPassword = document.getElementById('confirmPassword').value;

    if (confirmPassword != newPassword) {
        ShowError('Пароли не совпадают.');
        return;
    }
    if (newPassword.lenght < 4) {
        ShowError('Пароль должен быть не короче 4 символов.');
        return;
    }
    let fd = new FormData();
    fd.append('currentPassword', currentPassword);
    fd.append('newPassword', newPassword);
    fd.append('confirmPassword', confirmPassword);

    fetch('https://localhost:44354/Account/ChangePassword', {
        method: 'post',
        body: fd
    }).then(function (response) {
        if (response.ok) {
            ShowOk();
        }
        else {
            let text = response.text;
            ShowError(text);
        }
    });

}
