// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#select-league").on("change", function () {
    var selectedId = $(this).val();
    
    window.location.href = window.location.href + '?selectedId=' + selectedId;
});

const changeSelected = (e) => {
    const $select = $("#select-league");
    let value = $("#selected-league").innerHtml;
    alert(value);
    $select.id = value;
};

function myFunction() {
    alert("GO!");
}
