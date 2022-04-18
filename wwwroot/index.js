const api = "https://localhost:7112/api/todo";
(async function render () {
let form = document.createElement("form");
let inputSubmit = document.createElement("input",);
inputSubmit.type="submit";

let inputMyDateTime = document.createElement("input",);
inputMyDateTime.type="date";

let inputTask = document.createElement("input",);
inputTask.type="text";

let inputMyRes = document.createElement("input",);
inputMyRes.type="file";

let inputSelectData = document.createElement("input",);
inputSelectData.type="date";
inputSelectData.onchange=(event=>{
    taskList.forEach(element=>{
        var item = document.getElementById(element.id);
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

form.appendChild(inputSubmit);
form.appendChild(inputMyDateTime);
form.appendChild(inputTask);
form.appendChild(inputMyRes);
form.appendChild(inputSelectData);

let ul = document.createElement("ul");

const taskList = await getlist();

taskList.forEach(element => {
    let li = document.createElement("li");
    let label = document.createElement("label");
   // let hiddenInput = document.createElement("input");
    //hiddenInput.type="hidden";
    //hiddenInput.value=element.id;
    let checkbox = document.createElement("input");
    checkbox.type="checkbox";
    checkbox.checked=element.isDone;
    checkbox.onclick=(async event=>{
        await fetch(`${api}/${element.id}`, {method:"put"});
    }) 
    console.log(element);
    label.innerHTML=element.task+" - "+(new Date(element.myDateTime)).toLocaleDateString()+" - Files:"+ (element.fileNames[0]?element.fileNames[0]:"");
    li.id=element.id;
    li.appendChild(label);
    //li.appendChild(hiddenInput);
    li.appendChild(checkbox);
    ul.appendChild(li);
});

form.addEventListener("submit",async event=>{
    event.preventDefault();
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
    checkbox.checked=responseitem.isDone;
    label.innerHTML=responseitem.task+" - "+(new Date(responseitem.myDateTime)).toLocaleDateString() ;
    li.appendChild(label);
    li.appendChild(checkbox);
    ul.appendChild(li);
});

let body = document.querySelector("body");
body.appendChild(form);
body.appendChild(ul);
})()

async function  getlist()
{
    const data = await fetch(api);
    return await data.json();
}
