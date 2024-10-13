// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code. 

let element = document.getElementById("id");

element.addEventListener("keyup", () => {
    //Send request To the Backend
    let xhr = new XMLHttpRequest();

    // Making our connection  
    let url = `https://localhost:44392/Employee/Index?InputString=${element.value}`;
    xhr.open("POST", url, true);

    // function execute after request is successful 
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            console.log(this.responseText);
        }
    }
    // Sending our request 
    xhr.send();
})