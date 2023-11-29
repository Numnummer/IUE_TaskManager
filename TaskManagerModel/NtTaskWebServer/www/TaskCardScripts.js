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
                document.getElementById("NotStarted").innerHTML += response;
                hideForm();
            },
            error: function (xhr, status, error) {
                // Handle error response
                console.log('Failed: ' + error);
            }
        });
    }
    else {
        console.log("Invalid data");
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