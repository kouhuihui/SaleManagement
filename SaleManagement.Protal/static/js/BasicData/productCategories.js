$(function () {
    $(".btn-edit").click(function () {
        var $this = $(this);
        var id = $this.attr("data-id");
        $("#modal").modal({
            remote: "/BasicData/EditProductCategory?id=" + id
        }).on("hidden.bs.modal", function () {
            $(this).removeData("bs.modal");
        });
    })

    $(".btn-deleted").click(function () {
        var id = $(this).attr("data-id");
        $(window).modalDialog({
            title: "提示",
            smallTitle: "删除品类信息",
            content: "确定删除该品类吗？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/BasicData/RemoveProductCategory/" + id,
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("删除成功");
                            setTimeout(function () {
                                location.href = "/BasicData/ProductCategories";
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