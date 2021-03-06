﻿$(document).ready(function () {
    getClassifiedInfo();
});
function bindWorkItemData(data) {
    var c = data.Classified,
        s = data.Section,
        noTasks = $(".noTasks");
        cId = $(".classified-overwatch"),
        pictureContainer = $(".classified-pictures-container"),
        price = $('.S_price'),
        description = $('.S_description'),
        video = $('.video');
    if (data.Classified) {
        cId.show();
        noTasks.hide();
        cId.data('wId', c.Id);
        price.html(c.S_price);
        description.html(c.S_description);
       // video.html(c.S_video);
        pictureContainer.empty();
        if (c.S_pictures) {
            $.each(c.S_pictures, function (i, v) {
                var li = $("<li></li>"),
                    img = $("<img/>").attr("src", "data:image/jpg;base64," + v);
                li.append(img);
                pictureContainer.append(li);
            });
            $('#slider').ubislider({
                arrowsToggle: true,
                type: 'ecommerce',
                autoSlideOnLastClick: true,
                modalOnClick: true,
                position: 'vertical'
            });
        }
    } else {
        cId.hide();
        noTasks.show();
    }

}

function getClassifiedInfo() {
    var uri = "/api/admin/workitem";
    $.ajax({
        url: uri,
        method: 'GET',
        data: {
            Id: $(".classified-overwatch").data('id')
        },
        success: function (data) {
            bindWorkItemData(data);
        },
        error: function () {
            alert("error")
        }
    });
}
function rejectWorkItem() {
    var uri = "/api/admin/rejectworkitem";
    $.ajax({
        url: uri,
        method: 'PUT',
        data: {
            Id: $(".classified-overwatch").data('wId')
        },
        success: function () {
            getClassifiedInfo();
        },
        error: function () {
            alert("error")
        }
    });
}
function approveWorkItem() {
    var uri = "/api/admin/approveworkitem";
    $.ajax({
        url: uri,
        method: 'PUT',
        data: {
            Id: $(".classified-overwatch").data('wId')
        },
        success: function () {
            getClassifiedInfo();
        },
        error: function () {
            alert("error")
        }
    });
}
