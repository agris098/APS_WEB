
/// Croppie ------------------------------
var vanilla;
function createVanilla(src) {
    var el = $("#vanilla-demo")[0];
    vanilla = new Croppie(el, {
        url: src,
        orientation: 1,
        viewport: { width: 250, height: 300 },
        boundary: { width: 350, height: 400 },
        showZoomer: false,
        enableOrientation: true
    });
    $('.vanilla-result').unbind("click").on('click', function (ev) {
        vanilla.result({
            type: 'base64'
        }).then(function (base64) {
            $('#user_img2').attr("src", base64);
            var imageString = base64.substr(base64.indexOf(',') + 1);
            $('#lg_image').val(imageString);
        });
    });
    $('.vanilla-rotate').unbind('click').on('click', function (ev) {
        vanilla.rotate(parseInt($(this).data('deg')));
    });
}
var imageOnModal;
function onFileSelected(event) {
    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    var imgtag = document.getElementById("user_img2");
    imgtag.title = selectedFile.name;

    reader.onload = function (event) {
        imageOnModal = event.target.result;
        $("#myModal").modal('show');
    };
    reader.readAsDataURL(selectedFile);
}

$(document).on("hidden.bs.modal", "#myModal", function () {
    vanilla.destroy();
});
$(document).on("shown.bs.modal", "#myModal", function () {
    createVanilla(imageOnModal);
});