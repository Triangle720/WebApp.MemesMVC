/* form show / hide
---------------------------------- */

function showForm(divName, button) {
    document.getElementById(divName).style.display = "block";
    document.getElementById(button).style.display = "none";
}

function hideForm(divName, button) {
    document.getElementById(divName).style.display = "none";
    document.getElementById(button).style.display = "block";
}