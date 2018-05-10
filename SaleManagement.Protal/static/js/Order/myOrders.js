$(function () {
    var $currentUser = $("#currentUser"),
        $orderListPage = $('#orderListPage'),
        $orderId = $("#orderId"),
        $orderStatus = $("#orderStatus"),
        $colorFormId = $("#colorFormId");
    var Orders = function (data) {
        var self = this;
        self.orders = ko.observableArray(data);
        self.uploadDesginImageClick = function (item, el) {
            $("#modal").modal({
                remote: "/order/" + item.id + "/upload/desginImage"
            })
        },
        self.CustomerTobeConfirmClick = function (item, el) {
            $(window).modalDialog({
                title: "提示",
                smallTitle: "",
                content: "确认进入客户确认阶段</br>设计费用(元)：<input style=\"width:40px\" name=\"outputWaxCost\"/>",
                type: "confirm",
                okCallBack: function (e, $el) {
                    var outputWaxCost = $("input[name=outputWaxCost]").val();
                    if (isNaN(Number(outputWaxCost))) {
                        outputWaxCost = 0;
                    }
                    $.ajax({
                        url: "/order/" + item.id + "/CustomerTobeConfirm",
                        type: "POST",
                        dataType: "json",
                        data: { "outputWaxCost": outputWaxCost },
                        success: function (result) {
                            if (result.succeeded) {
                                $el.data("bs.modal").hide();
                                location.reload();
                            } else {
                                shortTips(errorMessage(result));
                            }
                        }
                    });
                }
            });
        },
             self.CustomerTobeConfirmMesClick = function (item, el) {
                 $(window).modalDialog({
                     title: "提示",
                     smallTitle: "",
                     content: "发送客户确认设计稿消息",
                     type: "confirm",
                     okCallBack: function (e, $el) {
                         $.ajax({
                             url: "/order/" + item.id + "/CustomerTobeConfirmMsg",
                             type: "POST",
                             dataType: "json",
                             data: {},
                             success: function (result) {
                                 if (result.succeeded) {
                                     $el.data("bs.modal").hide();
                                     location.reload();
                                 } else {
                                     shortTips(errorMessage(result));
                                 }
                             }
                         });
                     }
                 });
             }
    }

    var ordersView = new Orders([]);
    ko.applyBindings(ordersView);
    //分页
    $orderListPage.pager({
        url: '/Order/MyOrders',
        pageSize: 20,
        param: searchArgs(),
        callback: function (data, ui) {
            ordersView.orders(data.list);
        }
    });

    function searchArgs() {
        return {
            orderId: $orderId.val(),
            status: $orderStatus.val(),
            currentUserId: $currentUser.val() == null ? "" : $currentUser.val(),
            colorFormId: $colorFormId.val(),
        }
    }

    function search() {
        var pager = $orderListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(1);
    }

    $("#btnSearch").on("click", function () {
        search();
    })

    $orderId.bind('keypress', function (event) {
        if (event.keyCode == "13") {
            var orderId = $orderId.val();
            if (orderId != "" && orderId.lastIndexOf(',') != orderId.length - 1) {
                $orderId.val(orderId + ",");
            }
            search();
            $orderId.focus();
        }
    });

    $orderStatus.bind('change', function () {
        search();
    })

    $colorFormId.bind('change', function () {
        search();
    })

    $currentUser.bind('change', function () {
        search();
    });

    $("#btnAssginToMe").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;
        var ids = "";
        if (length === 0) {
            shortTips("请选择订单");
            return false;
        }
        for (var i = 0; i < length; i++) {
            var $inputChecked = $(inputCheckeds[i]);
            ids = ids + $inputChecked.val() + ",";
        }
        $(window).modalDialog({
            title: "提示",
            smallTitle: "",
            content: "确认由我来处理所选择订单的设计？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/order/AssginToMe",
                    type: "POST",
                    dataType: "json",
                    data: { "orderIds": ids },
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            location.reload();
                        } else {
                            shortTips(errorMessage(result));
                        }
                    }
                });
            }
        });
    })

    $("#btnOutPutWax").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;
        var ids = "";
        if (length === 0) {
            shortTips("请选择订单");
            return false;
        }
        for (var i = 0; i < length; i++) {
            var $inputChecked = $(inputCheckeds[i]);
            var orderId = $inputChecked.val();
            if ($inputChecked.attr("status") != 4) {
                shortTips(orderId + "订单不是客户确认状态，不能出蜡");
                return false;
            }
            ids = ids + orderId + ",";
        }
        $(window).modalDialog({
            title: "提示",
            smallTitle: "",
            content: "确认进入出蜡阶段？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/order/GotoOutputWax",
                    type: "POST",
                    dataType: "json",
                    data: { "orderIds": ids },
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            location.reload();
                        } else {
                            shortTips(errorMessage(result));
                        }
                    }
                });
            }
        });
    })

    InitDesgin();

    function InitDesgin() {
        $.ajax({
            url: "/user/GetUsersByRole",
            data: { "roleCode": "design" },
            success: function (rtn) {
                if (rtn.succeeded) {
                    var data = rtn.data;
                    var html = '<option value="">请选择设计师</option>';
                    for (var i = 0, len = data.length; i < len; i++) {
                        html += '<option value="' + data[i].id
                                     + '">' + data[i].name
                                     + '</option>';
                    }
                    $currentUser.html(html);
                }
            },
            error: function () {

            }
        });
    }
});