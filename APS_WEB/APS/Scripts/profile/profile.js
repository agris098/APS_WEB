$(document).ready(function () {
    loadClassifieds();
});

function loadClassifieds() {
    $.ajax({
        method: "GET",
        url: publicedClassifiedUri,
        success: function (data) {
            bindClassifiedData(data);
        },
        error: function () {

        }
    });
}
function bindClassifiedData(data) {
    var container = $("#profile-classifieds"),
        table = $("<table class='table table-responsive'></table>");
    console.log(data);
    $.each(data, function () {
        console.log('data', data, this);
        var imgP = !this.S_price ? "" : "<div class='image'><img src='data: image/jpg;base64," + this.S_mpicture + "' /></div>";
        var price = this.S_price ? "<div class='price'>" + this.S_price + " €</div>" : "";
        var template = "<tr c-id='" + this.IDS + "'><td><div class='info'>" + imgP + "<div class='description'><span>" + this.S_description + "<span></div>" + price +
            "</div></td></tr>"
        table.append(template);
    });
    container.append(table);
}
