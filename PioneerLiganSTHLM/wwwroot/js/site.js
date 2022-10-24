$("#select-league").on("change", function () {
    var selectedId = $(this).val();
    
    window.location.href = window.location.href + '?selectedId=' + selectedId;
});

function Collapser(item) {

    var y = item.nextElementSibling.id;

    if ($("#" + y).hasClass("hideElement")) {

        $("#" + y).removeClass("hideElement");

    } else {

        $("#" + y).addClass("hideElement");

    }
}

const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))