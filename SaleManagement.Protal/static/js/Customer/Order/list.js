$(function () {
    var status = getQueryString("status"),
       $listPage = $("#orderlist");

    if (status == null) {
        status = "0";
    }
    $("#orderli li[status=" + status + "]").addClass("active").siblings("li").removeClass("active");
    var Orders = function (data) {
        var self = this;
        self.orders = ko.observableArray(data);
        self.CustomerConfirmClick = function (item, el) {
            $(window).modalDialog({
                title: "提示",
                smallTitle: "",
                content: "确认通过设计师的设计方案",
                type: "confirm",
                okCallBack: function (e, $el) {
                    $.ajax({
                        url: "/Customer/Order/GoToOutputWaxStep",
                        type: "POST",
                        data: {
                            orderId: item.id
                        },
                        dataType: "json",
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
                                self.orders.remove(item);
                            } else {
                                shortTips(errorMessage(result));
                            }
                        }
                    });
                }
            });
            el.stopPropagation();
        };
        self.ViewProcess = function (item, el) {
            location.href = '/customer/order/Process?orderId=' + item.id;
        }
    }

    var ordersView = new Orders([]);
    ko.applyBindings(ordersView);
    $listPage.loading();
    //分页
    $('#orderListPage').pager({
        url: '/Customer/Order/List',
        pageSize: 10,
        param: { status: status },
        callback: function (data, ui) {
            $listPage.data("loading").hide();
            ordersView.orders(data.list);
        }
    });
});
