$(document).ready(function () {
    updateUserTable();
});

function updateUserTable() {
    var uri = "/api/admin/users";
    $.ajax({
        url: uri,
        method: 'GET',
        success: function (data) {
            $('#usersTable').DataTable({
                "data": data,
                "columns": [
                    { "data": "Id" },
                    { "data": "FullName" },
                    { "data": "Email" },
                    { "data": "EmailConfirmed" },
                    { "data": "PhoneNumber" },
                    { "data": "Roles" },
                    { "data": "Blocked" },
                    { "defaultContent": "<i class='fa fa-edit edit-profile'></i><i class='fa fa-eye show-profile'></i>" }
                ],
                "columnDefs": [
                    {
                        "targets": [7,4,2,0],
                        "visible": true,
                        "orderable": false
                    }
                ]
            });
        },
        error: function () {
            alert("error")
        }
    });
}
$(document).on('click', '.show-profile', function () {
    var id = $(this).closest('tr').find('td:first').html();
    location.href = "/profile/" + id;
});
$(document).on('click', '.edit-profile', function () {
    var id = $(this).closest('tr').find('td:first').html();
    var modal = $('#userEditModal');
    //  modal.
    var roles = $(this).closest('tr').find('td:nth-child(6n)').html();
    var b = $(this).closest('tr').find('td:nth-child(7n)').html();
    var AdminRole = modal.find('input[value=Admin]');
    var SupportRole = modal.find('input[value=Support]');
    var blockButton = modal.find('input[value=Blocked]');
    AdminRole.prop('checked', false)
    SupportRole.prop('checked', false);
    blockButton.prop('checked', false);
    if (roles.indexOf('Admin') !== -1) {
        AdminRole.prop('checked', true)
    }
    if (roles.indexOf('Support') !== -1) {
        SupportRole.prop('checked', true)
    }
    if (b.indexOf('true') !== -1) {
        blockButton.prop('checked', true);
    }
    modal.find('#Id').val(id);
    modal.modal('show');
});
function editUser() {
    var modal = $('#userEditModal');
   
    var uri = "/api/admin/edituser";
    var roles = modal.find('input[name=Roles]:checked').map(function () { return this.value; }).get().join(',');
    var blocked = modal.find('input[value=Blocked]:checked').length > 0 ? true : false;
    var data = {
        Id: modal.find('#Id').val(),
        Roles: roles,
        Blocked: blocked
    };
    $.ajax({
        url: uri,
        method: 'POST',
        data: data,
        success: function (data) {
            modal.modal('hide');
            var parent = $("td:contains(" + modal.find('#Id').val()+")").parent();
            parent.find('td:nth-child(6n)').html(roles);
            parent.find('td:nth-child(7n)').html(String(blocked));
        },
        error: function () {
            alert("error")
        }
    });
}
