$(document).ready(function () {
    getPublicedCount();
    getWorkerList();
    getWorkerInfo();
    setInterval(function () { getPublicedCount(); getWorkerInfo(); }, 5000);

});
function getPublicedCount() {
    console.log('UpdateCountData');
    var uri = "/api/admin/pclassifiedscount";
    $.ajax({
        url: uri,
        method: 'GET',
        success: function (data) {
            bindPublicedCountData(data);
        },
        error: function () {
            alert("error")
        }
    });
}
function bindPublicedCountData(data) {
    var notAssigned = $("#notAssigned"),
        assigned = $("#assigned"),
        approved = $("#approved"),
        notApproved = $("#notApproved");

    notApproved.html(data.NotApproved);
    approved.html(data.Approved);
    assigned.html(data.Assigned);
    notAssigned.html(data.NotAssigned);
}

function getWorkerList() {
    console.log('getWorkerList');
    var uri = "/api/admin/workerlist";
    $.ajax({
        url: uri,
        method: 'GET',
        success: function (data) {
            bindWorkerData(data);
        },
        error: function () {
            alert("error")
        }
    });
}
function getWorkerInfo() {
    console.log('getWorkerInfo');
    var uri = "/api/admin/workerinfo";
    $.ajax({
        url: uri,
        method: 'GET',
        success: function (data) {
            bindWorkerInfo(data);
        },
        error: function () {
            alert("error")
        }
    });
}
function bindWorkerData(data) {
    var listContainer = $("#workerList");

    listContainer.empty();
    $.each(data, function () {
        var option = $("<option></option>");
        option.html(this.FullName);
        option.val(this.Id);
        listContainer.append(option);
    });
}
function assingClassifieds() {
    var uri = "/api/admin/assign";
    var data = {
        Id: $('#workerList').val(),
        Count: $('#assingCount').val()
    }
    $.ajax({
        url: uri,
        method: 'POST',
        data: data,
        success: function (count) {
            $('.addedCountContainer').show();
            $('#addedCount').html(count);
            $('#assignModal .btn-primary').addClass('disabled');
        },
        error: function () {
            alert("error")
        }
    });
}
function bindWorkerInfo(data) {
    var table = $("#summary-workers");
    table.empty();
    $.each(data, function () {
        var tr = $("<tr id=" + this.Id + "><td>" + this.FullName + "</td><td>" + this.NotApproved + "</td></tr>");
        table.append(tr);
    });
}
$('#assignModal').on('show.bs.modal', function () {
    $('.addedCountContainer').hide();
    $('#assignModal .btn-primary').removeClass('disabled');
    $('#assingCount').val(30);
});
$(document).on('focus', '#assignModal input, #assignModal select', function () {
    $('.addedCountContainer').hide();
    $('#assignModal .btn-primary').removeClass('disabled');
});
////////////////////////   Classified Logic Here