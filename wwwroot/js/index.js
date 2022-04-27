const api = "https://localhost:7112/api/todo";
(async function render () {
    let form = document.createElement("form");


let ul = document.createElement("ul");
const taskList = await getlist();
initStartComponnets(form,taskList);
SetTasks(ul,taskList);


form.addEventListener("submit",async event=>{
    event.preventDefault();
    Filter(ul);
});

let body = document.querySelector("body");
body.appendChild(form);
body.appendChild(ul);
})()

async function Filter(ul){
    let formData = new FormData();
    formData.append("Task",inputTask.value);
    formData.append("MyDateTime",inputMyDateTime.value);
    formData.append("MyRes",inputMyRes.files[0]);    
    const response = await fetch(api, {method:"post", body:formData});
    const responseitem = await response.json();
    let li = document.createElement("li");
    let label = document.createElement("label");
    let checkbox = document.createElement("input");
    checkbox.type="checkbox";
    checkbox.className="form-check-input";
    checkbox.checked=responseitem.isDone;
    label.innerHTML=responseitem.task+" - "+(new Date(responseitem.myDateTime)).toLocaleDateString() ;
    li.appendChild(label);
    li.appendChild(checkbox);
    ul.appendChild(li);
}

async function  getlist()
{
    const response = await fetch(api);
    console.log(response);
    if(response.status=401)
    window.location.href = "http://127.0.0.1:5500/auth.html";
    return await data.json();
}
function initStartComponnets(form,taskList){

    let inputSubmit = document.createElement("input",);
    inputSubmit.type="submit";
    inputSubmit.className="btn btn-primary";
    
    let inputMyDateTime = document.createElement("input",);
    inputMyDateTime.type="date";
    inputMyDateTime.className="form-control";
    
    let inputTask = document.createElement("input",);
    inputTask.type="text";
    inputTask.className="form-control";
    
    let inputMyRes = document.createElement("input",);
    inputMyRes.type="file";
    
    let inputSelectData = document.createElement("input",);
    inputSelectData.type="date";
    inputSelectData.className="form-control";
    inputSelectData.onchange=(event=>{
        taskList.forEach(element=>{
            var item = document.getElementById(element.id);
            if(!inputSelectData.value)
            item.style.visibility="visible";
            else
            if(new Date(element.myDateTime).toLocaleDateString()!=(new Date(inputSelectData.value)).toLocaleDateString())
            {
            item.style.visibility="hidden";
            }
            else
            {
            item.style.visibility="visible";
            }
        })
    });
    
    form.appendChild(inputTask);
    form.appendChild(inputMyDateTime);
    form.appendChild(inputMyRes);
    form.appendChild(inputSubmit);
    
    form.appendChild(inputSelectData);
}
function SetTasks(ul,taskList){

    taskList.forEach(element => {
        let li = document.createElement("li");
        let label = document.createElement("label");
        let inputName = document.createElement("input");
        inputName.type="text";
        let inputData = document.createElement("input",);
        inputData.type="date";
        let checkbox = document.createElement("input");
        checkbox.type="checkbox";
        checkbox.className="form-check-input";
        checkbox.checked=element.isDone;
        checkbox.onclick=(async event=>{
            let formData = new FormData();
            formData.append("Task",inputName.value);
            formData.append("MyDateTime",inputData.value);
            await fetch(`${api}/${element.id}`, {method:"put",body:formData});
        }) 
        inputName.value=element.task;
        console.log(element.myDateTime);
        var date = new Date();
        date.setDate(new Date(element.myDateTime).getDate());
        inputData.value=date.toISOString().split('T')[0];
        label.innerHTML="Files:"+ (element.fileNames[0]?element.fileNames[0]:"");
        li.id=element.id;
        li.appendChild(inputName);
        li.appendChild(inputData);
        li.appendChild(label);
        li.appendChild(checkbox);
        ul.appendChild(li);
    });
}