$(function () {
    $(".btn-edit").click(function () {
        var $this = $(this);
        var id = $this.attr("data-id");
        $("#modal").modal({
            remote: "/BasicData/EditGemCategory?id=" + id
        }).on("hidden.bs.modal", function () {
            $(this).removeData("bs.modal");
        });
    })

    $(".btn-deleted").click(function () {
        var id = $(this).attr("data-id");
        $(window).modalDialog({
            title: "提示",
            smallTitle: "删除宝石信息",
            content: "确定删除该宝石吗？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/BasicData/RemoveGemCategory/" + id,
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("删除成功");
                            setTimeout(function () {
                                location.href = "/BasicData/GemCategories";
                            }, 1000);
                        } else {
                            shortTips(errorMessage(result));
                        }
                    }
                });
            }
        });
    })
});