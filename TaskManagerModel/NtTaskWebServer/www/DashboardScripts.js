function ToProfile() {
    $.get('UserProfile/UserData', function (response) {
        console.log('Успешный ответ:', response);
        localStorage.setItem("UserData", response);
        //window.myObject=JSON.parse(response);
        window.location = "UserProfile";
    }).fail(function (error) {
        console.error('Ошибка:', error);
        // Обрабатывайте ошибку...
    });
}

function ExitFromAccount() {
    $.post('Dashboard/ExitFromAccount', function (response) {
        if (response == "ok") {
            window.location = "StartPage";
        }
    }).fail(function (error) {
        console.error('Ошибка:', error);
        // Обрабатывайте ошибку...
    });
}

window.addEventListener('load', function () {
    // Your code here
    console.log('The page has loaded!');
});
