﻿$(function () {
    $(".btn-edit").click(function () {
        var $this = $(this);
        var id = $this.attr("data-id");
        $("#modal").modal({
            remote: "/BasicData/EditMainStone?id=" + id
        }).on("hidden.bs.modal", function () {
            $(this).removeData("bs.modal");
        });
    })

    $(".btn-deleted").click(function () {
        debugger;
        var id = $(this).attr("data-id");
        $(window).modalDialog({
            title: "提示",
            smallTitle: "删除主石信息",
            content: "确定删除该主石吗？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/BasicData/RemoveMainStone/" + id,
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("删除成功");
                            setTimeout(function () {
                                location.href = "/BasicData/MainStones";
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