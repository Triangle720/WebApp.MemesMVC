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

function previewFile(input, hiddenDivId) {
    document.getElementById(hiddenDivId).style.display = "block";

    var file = $("input[type=file]").get(0).files[0];

    if (file) {
        var reader = new FileReader();

        reader.onload = function () {
            $("#previewImg").attr("src", reader.result);
        }

        reader.readAsDataURL(file);
    }
}
