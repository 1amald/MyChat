var basic;
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
$('.actionSelect input').on('change', function () { readFile(this); });
$('.actionUpload').on('click', function () {
    basic.croppie('result', 'blob').then(async function (blob) {
        var formData = new FormData();
        formData.append('filename', 'test.jpg');
        formData.append('blob', blob);
        var request = new XMLHttpRequest();
        request.open('POST', url);
        request.send(formData);
    });
});
