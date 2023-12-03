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

window.addEventListener('load', UpdateAllTasks);
function UpdateAllTasks() {
    var card = "";
    $.get('Dashboard/Tasks', async function (response) {
        await fetch('Dashboard/TaskHtml')
            .then(response => response.text())
            .then(html => {
                card = html;
            })
            .catch(error => {
                console.error('Error:', error);
            });

        var json = JSON.stringify(response);
        var tasks = JSON.parse(json);
        console.log("before");
        tasks.sort(function (task1, task2) {
            return task1.Priority - task2.Priority;
        });
        console.log("after");
        //TODO: Оптимизировать удаление
        ClearTasks();
        tasks.forEach(task => ProcessTask(task, card, "add"));

    }).fail(function (error) {
        console.error('Ошибка:', error);
        // Обрабатывайте ошибку...
    });
}

function ProcessTask(task, card) {
    let currentCard = card.replace("name", "name" + task.Id)
        .replace("startDate", "startDate" + task.Id)
        .replace("deadline", "deadline" + task.Id)
        .replace("cardId", task.Id);

    switch (task.Status) {
        case 0:
            ShowTaskCard(currentCard, "NotStarted", task);
            break;
        case 1:
            ShowTaskCard(currentCard, "InProcess", task);
            break;
        case 2:
            ShowTaskCard(currentCard, "Done", task);
            break;
        case 3:
            ShowTaskCard(currentCard, "Expired", task);
            break;
        default:
            // code to execute when expression doesn't match any case
            break;
    }
}

function ShowTaskCard(currentCard, column, task) {
    document.getElementById(column).innerHTML += currentCard;
    document.getElementById("name" + task.Id).innerHTML = task.Name;
    document.getElementById("deadline" + task.Id).innerHTML = task.Deadline;
}

function ClearTasks() {
    document.getElementById("NotStarted").innerHTML = "<div class=\"columnHeader\">Не начаты<button class=\"AddButton\" onclick=\"openForm()\">+</button></div > ";
    document.getElementById("InProcess").innerHTML = "<div class=\"columnHeader\">В процессе</div > ";
    document.getElementById("Done").innerHTML = "<div class=\"columnHeader\">Завершены</div > ";
    document.getElementById("Expired").innerHTML = "<div class=\"columnHeader\">Просрочены</div > ";
}
