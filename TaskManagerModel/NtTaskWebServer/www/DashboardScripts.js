function ToProfile(){
    $.get('UserProfile/UserData', function(response) {
    console.log('Успешный ответ:', response);
    window.myObject=JSON.parse(response);
    window.location="UserProfile";       
    }).fail(function(error) {
        console.error('Ошибка:', error);
        // Обрабатывайте ошибку...
    });    
}