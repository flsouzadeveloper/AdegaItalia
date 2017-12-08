$(document).ready(function () {
    $("#btnSalvarDadosContato").click(SalvarDadosContato_Click);
});

function SalvarDadosContato_Click() {

    $.ajax({
        url: Action("SalvarContato", "Contato"),
        type: 'POST',
        data: $("#ContatoContent").find("[name]").serialize(),
        success: function (data) {

            $("#ContatoContent").html(data);
            var idSummary = $("#testeContato").find("[id='valSumId']").find("li").html();
            if (idSummary == "" || idSummary == undefined) {

                ExibirModal($("#overlay"));
                $("#cont").dialog({
                    open: function (event, ui) {
                        $(".ui-dialog-content").css("height", "280px");
                        $(".ui-dialog").css("border", "none");
                        $(".ui-dialog").css("background", "none");
                        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                        CarregarCssButton();
                    },
                    width: 700,
                    show: { effect: 'drop', direction: "up" },
                    modal: true,
                    buttons: [
                        {
                            text: "Ok",
                            "class": 'cancelButtonDialog',
                            click: function () {
                                location.reload();
                            }
                        }
                    ]
                });
            }
        }
    });
}

function CarregarCssButton() {
    $(".cancelButtonDialog").css("position", "absolute");
    $(".cancelButtonDialog").css("top", "215px");
    $(".cancelButtonDialog").css("left", "325px");
    $(".cancelButtonDialog").css("color", "#FDF1D3");
    $(".cancelButtonDialog").css("background-color", "#5D3F24");
    $(".cancelButtonDialog").css("border", "none");
    $(".cancelButtonDialog").css("border-radius", "3px");
    $(".cancelButtonDialog").css("width", "55px");
}

function ExibirModal(object) {
    ToogleModal(object, true);
}

function FecharModal(object) {
    ToogleModal(object, false);
}

function ToogleModal(object, display) {
    if (display) {
        $(object).fadeIn();
        $("#overlay").fadeIn();
    } else {
        $(object).fadeOut();
        $("#overlay").fadeOut();
    }
}

