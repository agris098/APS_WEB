$(document).ready(function () {
    updateUserTable();
});

function updateUserTable() {
    var uri = "http://localhost:56616/api/admin/users";
    $.ajax({
        url: uri,
        method: 'GET',
        success: function (data) {
            console.log(data);
            $('#example').DataTable({
                "data": data,
                "columns": [
                    { "data": "Email" },
                    { "data": "PhoneNumber" }
                ]
            });

        },
        error: function () {
            alert("error")
        }
    });
}
