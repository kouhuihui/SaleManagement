

$(function () {

    $(".btn-deleted").click(function () {
        var id = $(this).attr("data-id");
        $(window).modalDialog({
            title: "提示",
            smallTitle: "",
            content: "确定删除该主石吗？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/order/DeleteMainStone/" + id,
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("删除成功");
                            setTimeout(function () {
                                location.reload();
                            }, 1000);
                        } else {
                            shortTips(errorMessage(result));
                        }
                    }
                });
            }
        });
    });
});