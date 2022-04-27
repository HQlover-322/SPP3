(function start() {
let form = document.createElement("form");
let inputSubmit = document.createElement("input",);
inputSubmit.type="submit";
inputSubmit.className="btn btn-primary";

let inputLogin = document.createElement("input",);
inputLogin.type="text";
inputLogin.className="form-control";

let inputPassword = document.createElement("input",);
inputPassword.type="text";
inputPassword.className="form-control";

form.appendChild(inputLogin);
form.appendChild(inputPassword);
form.appendChild(inputSubmit);


form.addEventListener("submit",async event=>{
    event.preventDefault();
    let formData = new FormData();
    formData.append("Username",inputLogin.value);
    formData.append("Password",inputPassword.value);  
    const response = await fetch("https://localhost:7112/users/authenticate", {method:"post", body:formData});
    console.log(response);
});
let body = document.querySelector("body");
body.appendChild(form);
}
)()