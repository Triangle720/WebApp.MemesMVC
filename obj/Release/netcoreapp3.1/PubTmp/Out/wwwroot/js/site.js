/* form show / hide
---------------------------------- */

function showForm(divId, button) {
    document.getElementById(divId).style.display = "block";
    document.getElementById(button).style.display = "none";
}

function hideForm(divId, button) {
    document.getElementById(divId).style.display = "none";
    document.getElementById(button).style.display = "block";
}