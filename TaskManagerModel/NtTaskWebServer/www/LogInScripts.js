function enter() {
    var loginData = {
        UserName: document.getElementById("UserName").value,
        Password: document.getElementById("Password").value
    };

    if (!IsValid(loginData)) {
        openErrWindow("Не валидные данные");
        return;
    }

    $.ajax({
        type: 'POST',
        url: 'LogIn',
        data: JSON.stringify(loginData),
        success: function (response) {
            if (response == "User accepted") {
                window.location = "Dashboard";
            }
            else {
                document.getElementById("errorLabel").innerHTML = response;
            }
        },
        error: function (xhr, status, error) {
            openErrWindow(xhr.responseText);
        }
    });
}

function IsValid(loginData) {
    var isUserNameValid = loginData.UserName != "";
    var isPasswordValid = loginData.Password.length >= 4;
    return isUserNameValid && isPasswordValid;
}