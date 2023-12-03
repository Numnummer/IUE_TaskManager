function ToBoard() {
    window.location = "Dashboard";
}

document.addEventListener('DOMContentLoaded', function () {
    //console.log(localStorage.getItem("UserData"));
    var userData = JSON.parse(localStorage.getItem("UserData"));
    console.log(userData);
    console.log(userData.UserName);
    document.getElementById("Name").innerText = userData.UserName;
    document.getElementById("Email").innerText = userData.Email;
});

function AddFriend() {
    openForm();
}

function hideForm() {
    document.getElementById("friendForm").style.display = "none";
}

function openForm() {
    document.getElementById("friendForm").style.display = "block";
}

document.getElementById("UserNameInput").addEventListener('input', function (event) {

    $.ajax({
        type: 'POST',
        url: 'UserProfile/Users',
        data: JSON.stringify(event.target.value),
        success: function (response) {
            var json = JSON.stringify(response);
            var users = JSON.parse(json);
            document.getElementById("users").innerHTML = "";
            users.forEach(user => ProcessUser(user));
        },
        error: function (xhr, status, error) {
            // Handle error response
            console.log('Failed: ' + error);
        }
    });
});

function ProcessUser(user) {
    document.getElementById("users").innerHTML += "<li>" + user + "</li>";
}