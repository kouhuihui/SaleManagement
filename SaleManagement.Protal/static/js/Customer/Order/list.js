$(function () {
    var status = getQueryString("status"),
       $orderlist = $("#orderlist"),
        $orderId = $("#OrderId"),
       $orderListPage = $('#orderListPage');

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
                        url: "/Customer/Order/GoToConfirmStep",
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
	    self.ViewProcess = function(item, el) {
		    location.href = '/customer/order/Process?orderId=' + item.id;
	    };
	    self.ViewShipmentOrder = function (item, el) {
	    	location.href = '/customer/order/ShipmentOrderDetail?orderId=' + item.id;
	    	el.stopPropagation();
	    };
    }

    var ordersView = new Orders([]);
    ko.applyBindings(ordersView);
    $orderlist.loading();
    //分页
    $orderListPage.pager({
        url: '/Customer/Order/List',
        pageSize: 10,
        param: searchArgs(),
        callback: function (data, ui) {
            $orderlist.data("loading").hide();
            ordersView.orders(data.list);
        }
    });

    function searchArgs() {
        return {
            queryOrderStatus: status,
            orderId: $orderId.val()
        }
    }

    function search() {
        var pager = $orderListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(1);
        $orderlist.data("loading").show();
    }

    $("#btnSearch").on("click", function () {
        search();
    })

});
