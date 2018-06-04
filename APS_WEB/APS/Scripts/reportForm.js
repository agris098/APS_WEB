function sendReportError() {
    var uri = "/api/admin/addreporterror";
    var data = {
        Description: $('#ReportText').val()
    };
    $.ajax({
        method: "POST",
        url: uri,
        data: data,
        success: function () {
            $("#reportErrorModal").modal('hide');
        },
        error: function () { }
    });
}