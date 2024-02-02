const codeLength = 6;
function enter() {
    var code = document.getElementById("Code").value;
    if (!isCodeValid(code)) {
        openErrWindow("Код должен содержать 6 цифр");
        return;
    }
    var userName = localStorage.getItem("user_name");
    var requestData = {
        UserName: userName,
        Code: code
    };
    $.ajax({
        type: 'POST',
        url: 'EmailAuth/Code',
        data: JSON.stringify(requestData),
        success: function (response) {
            window.location = "Dashboard";
        },
        error: function (xhr, status, error) {
            openErrWindow(xhr.responseText);
        }
    });
}

function isCodeValid(code) {
    return /^\d+$/.test(code) && code.length == codeLength;
}

window.addEventListener('load', focusOnInput);
function focusOnInput() {
    document.getElementById("Code").focus();
}