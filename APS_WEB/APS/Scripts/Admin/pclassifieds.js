$(document).ready(function () {
    updateTables();
});

function updateTables() {
    var uri = "http://localhost:56616/api/admin/pclassifieds";
    $.ajax({
        url: uri,
        method: 'GET',
        success: function (data) {
            var approved = data.Approved,
                notApproved = data.NotApproved;

            $('#approved-table').DataTable({
                "data": approved,
                "columns": [
                    { "data": "SectionId" },
                    { "data": "S_description" }
                ]
            });
            $('#not-approved-table').DataTable({
                "data": notApproved,
                "columns": [
                    { "data": "SectionId" },
                    { "data": "S_description" }
                ]
            });

        },
        error: function () {
            alert("error")
        }
    });
}
