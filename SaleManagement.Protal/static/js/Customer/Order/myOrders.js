$(function () {
    var $currentUser = $("#currentUser"),
        $orderListPage = $('#orderListPage'),
        $orderId = $("#orderId"),
        $orderStatus = $("#Status"),
        $colorFormId = $("#colorFormId"),
        $tbody = $("#tbody");;
    var Orders = function (data) {
        var self = this;
        self.orders = ko.observableArray(data),
        self.CustomerConfirmClick = function (item, el) {
            $(window).modalDialog({
                title: "提示",
                smallTitle: "",
                content: "设计费用" + item.outputWaxCost + "元,确认通过设计师的设计方案",
                type: "confirm",
                okCallBack: function (e, $el) {
                    $.ajax({
                        url: "/Customer/Order/GoToConfirmStep",
                        type: "POST",
                        data: {
                            orderId: item.id
                        },
                        dataType: "json",
                        success: function (result) {
                            if (result.succeeded) {
                                $el.data("bs.modal").hide();
                                search();
                            } else {
                                shortTips(errorMessage(result));
                            }
                        }
                    });
                }
            });
            el.stopPropagation();
        };
        self.ConfirmHaveGoods = function (item, el) {
            $(window).modalDialog({
                title: "提示",
                smallTitle: "",
                content: item.id + "确认收货!",
                type: "confirm",
                okCallBack: function (e, $el) {
                    $.ajax({
                        url: "/Customer/Order/GoToHaveGoodsStep",
                        type: "POST",
                        data: {
                            orderId: item.id
                        },
                        dataType: "json",
                        success: function (result) {
                            if (result.succeeded) {
                                shortTips("操作成功");
                                $el.data("bs.modal").hide();
                                search();
                            } else {
                                shortTips(errorMessage(result));
                            }
                        }
                    });
                }
            });
            el.stopPropagation();
        };
        self.viewProcessClick = function (item, el) {
            $("#modal").modal({
                remote: "/Order/process?orderId=" + item.id
            }).on("hidden.bs.modal", function () {
                $(this).removeData("bs.modal");
            });
        };
    }

    var ordersView = new Orders([]);
    ko.applyBindings(ordersView);
    $tbody.loading();
    //分页
    $orderListPage.pager({
        url: '/Customer/Order/MyOrders',
        pageSize: 20,
        param: searchArgs(),
        callback: function (data, ui) {
            $tbody.data("loading").hide();
            ordersView.orders(data.list);
        }
    });

    function searchArgs() {
        return {
            orderId: $orderId.val(),
            status: $orderStatus.val(),
            colorFormId: $colorFormId.val(),
            keyword: $("#Remark").val()
        }
    }

    function search() {
        var pager = $orderListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(1);
        $tbody.data("loading").show();
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
});