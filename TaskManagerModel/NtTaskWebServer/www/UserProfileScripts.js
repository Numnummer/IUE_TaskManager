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
    document.getElementById("operationResult").innerHTML = "";
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
    document.getElementById("users").innerHTML += "<li class=\"userList\">" + "<button type=\"button\" onclick=\"SendOrder(this)\">" + user + "</button>" + "</li>";
    console.log(document.getElementById("users").innerHTML); /*onclick=\"SendOrder(this)\"*/
}

function ProcessOrder(order) {
    document.getElementById("orders").innerHTML += "<li class=\"userList\">" + "<button type=\"button\" onclick=\"AcceptOrder(this)\">" + order + "</button>" + "</li>";
}

function ProcessFriend(friend) {
    document.getElementById("friendsList").innerHTML += "<li class=\"userList\">" + friend + "</li>";
}

function SendOrder(button) {
    console.log(button.innerHTML);
    $.ajax({
        type: 'POST',
        url: 'UserProfile/Order',
        data: JSON.stringify(button.innerHTML),
        success: function (response) {
            if (response == "add") {
                document.getElementById("operationResult").innerHTML = "Заявка успешно отправлена";
            }
            else {
                document.getElementById("operationResult").innerHTML = "Не удалось отправить заявку";
            }
        },
        error: function (xhr, status, error) {
            document.getElementById("operationResult").innerHTML = error;
            console.log('Failed: ' + error);
        }
    });
}

window.addEventListener('load', UpdateAllFriends);

function UpdateAllFriends() {
    $.ajax({
        type: 'GET',
        url: 'UserProfile/Orders',
        success: function (response) {
            var json = JSON.stringify(response);
            var orders = JSON.parse(json);
            document.getElementById("orders").innerHTML = "";
            orders.forEach(order => ProcessOrder(order));
        },
        error: function (xhr, status, error) {
            //document.getElementById("operationResult").innerHTML = error;
            console.log('Failed: ' + error);
        }
    });

    $.ajax({
        type: 'GET',
        url: 'UserProfile/Friends',
        success: function (response) {
            var json = JSON.stringify(response);
            var friends = JSON.parse(json);
            document.getElementById("friendsList").innerHTML = "";
            friends.forEach(friend => ProcessFriend(friend));
        },
        error: function (xhr, status, error) {
            document.getElementById("operationResult").innerHTML = error;
            console.log('Failed: ' + error);
        }
    });
}