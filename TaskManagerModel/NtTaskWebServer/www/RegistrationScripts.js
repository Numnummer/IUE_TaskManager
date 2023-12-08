function registrate() {
    var loginData = {
        UserName: document.getElementById("Name").value,
        Email: document.getElementById("Email").value,
        Login: document.getElementById("Login").value,
        Password: document.getElementById("Password").value
    };

    if (!IsValid(loginData)) {
        openErrWindow("Не валидные данные");
        return;
    }

    $.ajax({
        type: 'POST',
        url: 'Registration',
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
    var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    var isEmailValid = emailPattern.test(loginData.Email);
    var isLoginValid = loginData.Login != "";
    var isPasswordValid = loginData.Password.length >= 4;
    return isUserNameValid && isEmailValid && isLoginValid && isPasswordValid;
}