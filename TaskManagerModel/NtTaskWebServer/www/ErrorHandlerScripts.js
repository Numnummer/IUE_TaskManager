function hideErrWindow() {
    document.getElementById("errorWindow").style.display = "none";
}

function openErrWindow(errorMessage) {
    document.getElementById("errorWindow").style.display = "block";
    document.getElementById("errorText").innerHTML = errorMessage;
}

function hideHelpWindow() {
    document.getElementById("helpWindow").style.display = "none";
}

function openHelpWindow() {
    document.getElementById("helpWindow").style.display = "block";
}