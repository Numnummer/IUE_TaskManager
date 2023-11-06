function enter() {
    var loginData = {        
        UserName: document.getElementById("UserName").value,
        Password: document.getElementById("Password").value
      };

    if(!IsValid(loginData)){
        return;
    } 
      
    $.ajax({
    type: 'POST',
    url: 'LogIn',
    data: JSON.stringify(loginData),
    success: function(response) {
        if(response=="User accepted"){
            window.location="Dashboard";
        }
        else{
            document.getElementById("errorLabel").innerHTML=response;
        }
        console.log(response);
    },
    error: function(xhr, status, error) {
        // Handle error response
        console.log('Enter failed: ' + error);
    }
    });
}

function IsValid(loginData){
    var isUserNameValid=loginData.UserName!="";      
    var isPasswordValid=loginData.Password.length>=4;
    return isUserNameValid && isPasswordValid;
}