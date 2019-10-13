/// <reference path="../lib/jquery/dist/jquery.min.js" />

function getUserById(id) {
    $.getJSON("http://localhost/LuftbornWebApi/api/User/GetUserById", { id: id })
        .done((result) => {
            console.log(result);

            let userData = "<h1 class='display-4'>" + result.name + "</h1><pre>" +
                result.id + "<br >" +
                result.name + "<br />" +
                result.userName + "<br >" +
                result.email + "<br >" +
                result.address + "<br >" +
                result.phoneNumber + "<br >" +
                result.createdAt + "<br >" +
                result.modifiedAt +
                "</pre>";

            $("#userModal").html(userData);
        });
}

function deleteUser(id) {
    if (confirm("Are you sure?")) {
        $.ajax({
            url: "User/Delete",
            type: "DELETE",
            data: { id: id },
            success: () => {
                location.href = location.href;
            },
            failure: () => {
                alert("Error deleting user.");
            }
        });
    }
}