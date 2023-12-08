function openForm() {
    document.getElementById("taskForm").style.display = "block";
}

function addTask() {
    var taskData = {
        Name: document.getElementById("TaskName").value,
        Deadline: document.getElementById("Deadline").value,
        Priority: document.getElementById("Priority").value
    };

    if (IsTaskDataValid(taskData)) {
        $.ajax({
            type: 'POST',
            url: 'Dashboard/CreateTask',
            data: JSON.stringify(taskData),
            success: function (response) {
                if (response == "ok") {
                    UpdateAllTasks();
                    hideForm();
                }
            },
            error: function (xhr, status, error) {
                hideForm();
                openErrWindow(xhr.responseText);
            }
        });
    }
    else {
        hideForm();
        openErrWindow("Не валидная таска");
    }
}

function hideForm() {
    document.getElementById("taskForm").style.display = "none";
}

function IsTaskDataValid(taskData) {
    var dateRegex = /^(19|20)\d{2}-(0\d|1[0-2])-(0\d|1\d|2\d|3[0-1])T([01]\d|2[0-3]):[0-5]\d$/;

    if (dateRegex.test(taskData.Deadline)) {
        return taskData.Name.length > 0 && taskData.Priority >= 0;
    }
    else {
        return false;
    }
}

async function GetTaskCard() {
    await fetch('Dashboard/TaskHtml')
        .then(response => response.text())
        .then(html => {
            return html;
        })
        .catch(error => {
            console.error('Error:', error);
        });
    return "";
}

function RemoveTaskCard(button) {
    var id = button.parentNode.id;
    $.ajax({
        type: 'POST',
        url: 'Dashboard/RemoveTask',
        data: JSON.stringify(id),
        success: function (response) {
            if (response == "removed") {
                UpdateAllTasks();
            }
            else {
                openForm(response);
            }
        },
        error: function (xhr, status, error) {
            openForm(xhr.responseText);
        }
    });
}

function DecreaseTaskStatus(button) {
    var id = button.parentNode.parentNode.id;
    $.ajax({
        type: 'POST',
        url: 'Dashboard/DecreaseTaskStatus',
        data: JSON.stringify(id),
        success: function (response) {
            if (response == "ok") {
                UpdateAllTasks();
            }
        },
        error: function (xhr, status, error) {
            // Handle error response
            console.log('Failed: ' + error);
        }
    });
}

function IncreaseTaskStatus(button) {
    var id = button.parentNode.parentNode.id;
    $.ajax({
        type: 'POST',
        url: 'Dashboard/IncreaseTaskStatus',
        data: JSON.stringify(id),
        success: function (response) {
            if (response == "ok") {
                UpdateAllTasks();
            }
        },
        error: function (xhr, status, error) {
            // Handle error response
            console.log('Failed: ' + error);
        }
    });
}