$("#select-league").on("change", function () {
    var selectedId = $(this).val();
    
    window.location.href = window.location.href + '?selectedId=' + selectedId;
});

//function GetPlayerNames() {
//    $.ajax({
//        url: "/?handler=PlayerNames",
//        dataType: "text",
//    }).done(function (data) {
//        console.log("success");
//        console.log(data);
//        return data;
//    })
//        .fail(function (error) {
//            console.log(error);
//            console.log("random fail");
//            console.log(error.responseText);
//        });
//}

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