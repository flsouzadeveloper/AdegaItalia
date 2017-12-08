$(document).ready(function () {
    $("#addCarrinho").click(addCarrinho_Click);
});

function addCarrinho_Click(t) {

    $.ajax({
        url: Action("AddCarrinho", "Ecommerce"),
        type: 'POST',
        data: $("#EcommerceContent").find("[name]").serialize(),
        success: function (data) {
            ExibirModal($("#overlay"));
            ExibirModal($("#addCarrinhoSucesso"));

        }
    });

    //if ($(t).parent().parent().find("#disponivel").data("disponivel")) {
    //    ExibirModal($("#overlay"));

    //    $("#addCarrinhoSucesso").dialog({
    //        open: function (event, ui) {
    //            $(".ui-dialog-content").css("height", "280px");
    //            $(".ui-dialog").css("border", "none");
    //            $(".ui-dialog").css("background", "none");
    //            CarregarCssButton();
    //        },
    //        width: 700,
    //        show: { effect: 'drop', direction: "up" },
    //        modal: true,
    //        buttons: [
    //       {
    //           text: "Confirmar",
    //           "class": 'confirmButtonDialog',
    //           click: function () {
    //               var id = $(t).parent().parent().find("#idProduto").val();
    //               var quantidade = $(t).parent().parent().find("#descricaoDetalhesImageDireitoDetalhesQuantity").val();

    //               $.ajax({
    //                   url: Action("AddCarrinho", "Ecommerce"),
    //                   type: 'POST',
    //                   data: { id: id, quantidade: quantidade },
    //                   success: function (data) {
    //                       $("[name='quantidade']").val(0);
    //                       $("#EcommerceContent").html(data);
    //                       $("[name='quantidade']").val(0);
    //                   }
    //               });
    //               $("[name='quantidade']").val(0);
    //               FecharModal($("#overlay"));
    //               $(this).dialog("close");
    //           }
    //       },
    //       {
    //           text: "Cancelar",
    //           "class": 'cancelButtonDialog',
    //           click: function () {
    //               FecharModal($("#overlay"));
    //               $(this).dialog("close");
    //           }
    //       }
    //        ]
    //    });
    //}
}

//function CarregarCssButton() {
//    $(".confirmButtonDialog").css("position", "absolute");
//    $(".confirmButtonDialog").css("top", "215px");
//    $(".confirmButtonDialog").css("left", "230px");

//    $(".cancelButtonDialog").css("position", "absolute");
//    $(".cancelButtonDialog").css("top", "215px");
//    $(".cancelButtonDialog").css("left", "355px");
//}


function btnFinalizarCompra_Click() {
    var d = $("#pagseguro").find("[name]").toArray();
    var a = {}; for (var i = 0; i < d.length; i++) { a[d[i].name] = d[i].value; } a;
    var t = JSON.stringify(a);

    $.ajax({
        url: Action("FinalizarCompra", "Ecommerce"),
        type: 'POST',
        data: " { 'a': '" + t.toString() + "' }",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            PagSeguroLightbox(data);
        }
    });
}


