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
    $.get('Dashboard/Tasks', function (response) {

        var json = JSON.stringify(response);
        var tasks = JSON.parse(json);
        tasks.forEach(task => ProcessTask(task));

    }).fail(function (error) {
        console.error('Ошибка:', error);
        // Обрабатывайте ошибку...
    });
});

function ProcessTask(task) {
    switch (task.Status) {
        case 0:
            document.getElementById("NotStarted").innerHTML;
            break;
        case 1:
            // code to execute when expression matches value2
            break;
        case 2:
            // code to execute when expression matches value2
            break;
        case 3:
            // code to execute when expression matches value2
            break;
        default:
            // code to execute when expression doesn't match any case
            break;
    }
    console.log(task);
}
