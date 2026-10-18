const API_URL = "http://localhost:5298/api/task";

const taskForm = document.getElementById("taskForm");
const titleInput = document.getElementById("title");
const descriptionInput = document.getElementById("description");
const taskList = document.getElementById("taskList");
const submitBtn = document.getElementById("submitBtn");

let editingTaskId = null;

async function loadTasks() {
    try {
        const response = await fetch(API_URL, {
            method: "GET",
            cache: "no-store"
        });

        if (!response.ok) {
            throw new Error("Failed to load tasks");
        }

        const tasks = await response.json();

        console.log("Latest tasks from API:", tasks);

        displayTasks(tasks);

    } catch (error) {
        console.error("Error loading tasks:", error);

        taskList.innerHTML = `
            <p class="empty">Could not load tasks.</p>
        `;
    }
}
function displayTasks(tasks) {

taskList.innerHTML = "";

if (!tasks || tasks.length === 0) {

    taskList.innerHTML = `
        <p class="empty">
            No tasks yet. Add your first task.
        </p>
    `;

    return;
}

tasks.forEach(task => {

    const taskElement = document.createElement("div");

    taskElement.className = "task";

    taskElement.innerHTML = `
        <h3>${task.title}</h3>

        <p>${task.description}</p>

        <div class="task-actions">

            <button
                class="edit-btn"
                onclick="editTask(${task.id})">
                Edit
            </button>

            <button
                class="delete-btn"
                onclick="deleteTask(${task.id})">
                Delete
            </button>

        </div>
    `;

    taskList.appendChild(taskElement);
});


}


taskForm.addEventListener("submit", async function (event) {

event.preventDefault();

const title = titleInput.value.trim();
const description = descriptionInput.value.trim() ;


try {

    if (editingTaskId !== null) {

        const response = await fetch(
            `${API_URL}/${editingTaskId}`,
            {
                method: "PUT",

                headers: {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify({
                    id: editingTaskId,
                    title: title,
                    description: description
                })
            }
        );

        if (!response.ok) {
            throw new Error("Failed to update task");
        }

        editingTaskId = null;

        submitBtn.textContent = "Add Task";
    }

    else {

        const response = await fetch(
            API_URL,
            {
                method: "POST",

                headers: {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify({
                    title: title,
                    description: description
                })
            }
        );

        if (!response.ok) {
            throw new Error("Failed to create task");
        }
    }


    taskForm.reset();

    await loadTasks();

} catch (error) {

    console.error(error);

    alert("Something went wrong. Check your backend.");
}


});


async function editTask(id) {

try {

    const response = await fetch(`${API_URL}/${id}`);

    if (!response.ok) {
        throw new Error("Failed to get task");
    }

    const task = await response.json();

    titleInput.value = task.title;
    descriptionInput.value = task.description;

    editingTaskId = id;

    submitBtn.textContent = "Update Task";

    window.scrollTo({
        top: 0,
        behavior: "smooth"
    });

} catch (error) {

    console.error(error);

    alert("Could not load the task.");
}


}

async function deleteTask(id) {


try {

    const response = await fetch(
        `${API_URL}/${id}`,
        {
            method: "DELETE"
        }
    );

    if (!response.ok) {
        throw new Error("Failed to delete task");
    }
    
    await loadTasks();

} catch (error) {

    console.error(error);

    alert("Could not delete the task.");
}


}


loadTasks();