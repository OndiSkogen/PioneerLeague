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