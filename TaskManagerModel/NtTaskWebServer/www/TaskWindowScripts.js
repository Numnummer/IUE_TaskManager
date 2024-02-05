var windowDiv = document.getElementById('window');
var fieldDiv = document.getElementById('field');
var isDragging = false;
var startX, startY, scrollLeft, scrollTop;

windowDiv.addEventListener('mousedown', function (e) {
    isDragging = true;
    startX = e.clientX;
    startY = e.clientY;
    scrollLeft = windowDiv.scrollLeft;
    scrollTop = windowDiv.scrollTop;
});

windowDiv.addEventListener('mousemove', function (e) {
    if (!isDragging) return;
    var x = scrollLeft + startX - e.clientX;
    var y = scrollTop + startY - e.clientY;
    windowDiv.scrollTo(x, y);
});

windowDiv.addEventListener('mouseup', function () {
    isDragging = false;
});

function closeTaskWindowEditor() {
    document.getElementById("taskWindowEditor").style.display = "none";
}