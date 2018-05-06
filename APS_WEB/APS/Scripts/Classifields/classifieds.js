
function UpdateTableRows(selector, data) {
    var tableBody = $("#" + selector + " tbody");
    tableBody.empty();
    var option = "";
    switch (selector) {
        case "draft": option = "public";
            break;
        case "public": option = "unlist";
            break;
        case "expired": option = "unlist";
            break;
        case "rejected": option = "";
            break;
        case "marked": option = "";
            break;

    }

    $.each(data, function () {
        
        var imgP = !this.Picture ? "" : "<div class='image'><img src='data: image/jpg;base64," + this.Picture + "' /></div>";
        var price = this.Price ? "<div class='price'>" + this.Price + " €</div>" : "";
        var template = "<tr c-id='" + this.Id + "'><td><div class='info'>" + imgP + "<div class='description'><span>"+this.Description +"<span></div>" + price + 
            "</div></td><td><a class='delete'>Delete</a>   <a class='" + option + "'>" + option+"</a></td></tr>"
        tableBody.append(template);
    });
    $("[status = '" + selector + "']").find("span").text("("+data.length+")");
}
function UpdateTable() {
    var uri = "http://localhost:56616/api/classifields/byuser";
    $.ajax({
        url: uri,
        method: 'GET',
        success: function (data) {
            UpdateTableRows("draft", data[0].classifieds);
            UpdateTableRows("public", data[1].classifieds);
            UpdateTableRows("expired", data[2].classifieds);
            UpdateTableRows("rejected", data[3].classifieds);
            //UpdateTableRows("marked", data[4].classifieds);
        },
        error: function () {
            alert("error")
        }
    });
}
$(document).on('click', "a.delete", function (e) {
    e.stopPropagation();
    var cid = $(this).closest("tr").attr("c-id");
    var uri = "http://localhost:56616/api/classifields/delete/" + cid;
    $.ajax({
        method: 'Delete',
        url: uri,    
        success: function () {
            UpdateTable();
        },
        error: function () {
            alert("error")
        }
    });
});
$(document).on('click', "a.public", function (e) {
    e.stopPropagation();
    $("#publishModal").modal('show');
    
    var cid = $(this).closest("tr").attr("c-id");
    $("#publishModal").modal('show');
    $("#publishModal").find("[data-classified-id]").attr("data-classified-id", cid);
});
function publishClassified(cid) {
    var uri = "http://localhost:56616/api/classifields/updatestatus";
    var weeks = $("#publishModal").find("select").val();
    var data = {
        Id: cid,
        Status: 1,
        Weeks: weeks
    };
    $.ajax({
        method: 'PUT',
        url: uri,
        data: data,
        success: function () {
            $("#publishModal").modal('hide');
            UpdateTable();
        },
        error: function () {
            alert("error")
        }
    });
}
$(document).on('click', "a.unlist", function (e) {
    e.stopPropagation();
    var cid = $(this).closest("tr").attr("c-id");
    var uri = "http://localhost:56616/api/classifields/updatestatus";
    var data = {
        Id: cid,
        Status: 0
    };
    $.ajax({
        method: 'PUT',
        url: uri,
        data: data,
        success: function () {
            UpdateTable();
        },
        error: function () {
            alert("error")
        }
    });
});
$(document).on('click', "tbody tr", function (e) {
    e.stopPropagation();
    var cid = $(this).closest("tr").attr("c-id");
    var path = window.location.href.replace(window.location.pathname, "") + "/classifield/" + cid;

    location.href = path;
});

$(document).ready(function () {
    UpdateTable();
});
$('#publishModal').on('show.bs.modal', function (e) {
    $(this).find('select').val('1');
})