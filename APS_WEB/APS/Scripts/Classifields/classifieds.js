
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

    }

    $.each(data, function () {
        var template = "<tr c-id='" + this.Id + "'><td>" + this.SectionId + "</td><td>" + this.S_description +
            "</td><td><a class='delete'>Delete</a>   <a class='" + option + "'>" + option+"</a></td></tr>"
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
    var cid = $(this).closest("tr").attr("c-id");
    var uri = "http://localhost:56616/api/classifields/updatestatus";
    var data = {
        Id: cid,
        Status: 1
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