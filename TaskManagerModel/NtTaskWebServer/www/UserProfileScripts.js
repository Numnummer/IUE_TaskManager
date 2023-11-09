function ToBoard(){
    window.location="Dashboard";
}

document.addEventListener('DOMContentLoaded', function() {    
    //console.log(localStorage.getItem("UserData"));
    var userData=JSON.parse(localStorage.getItem("UserData"));
    console.log(userData);
    console.log(userData.UserName);
    document.getElementById("Name").innerText=userData.UserName;
    document.getElementById("Email").innerText=userData.Email;
  });