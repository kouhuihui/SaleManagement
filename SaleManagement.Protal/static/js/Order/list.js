$(function () {
    var customer = new Customer();
    var $orderListPage = $('#orderListPage');
    var rec = {
        autoclose: true,
        fontAwesome: true,
        format: "yyyy-mm-dd",
        minView: 2
    }
    $('.date-conditions input[type="text"]').datetimepicker(rec);

    var Orders = function (data) {
        var self = this;
        self.orders = ko.observableArray(data)
    }

    var ordersView = new Orders([]);
    ko.applyBindings(ordersView);

    $orderListPage.pager({
        url: '/Order/List',
        pageSize: 10,
        param: searchArgs(),
        method: "GET",
        callback: function (data, ui) {
            ordersView.orders(data.list);
        }
    });

    function searchArgs() {
        return {
            orderId: $("#orderId").val(),
            customerId: customer.getValue(),
            deliveryStartDate: $("#deliveryStartDate").val(),
            deliveryEndDate: $("#deliveryEndDate").val(),
            status: $("#Status").val(),
            colorFormId: $("#colorFormId").val(),
            urgentStatus: $("#UrgentStatus").val(),
			rushStatus: $("#RushStatus").val()
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

    $("#shipment").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;
        var ids = "";
        if (length === 0) {
            shortTips("请选择订单");
            return false;
        }
        for (var i = 0; i < length; i++) {
            var $inputChecked = $(inputCheckeds[i]);
            if ($inputChecked.attr("status") != 9) {
                shortTips("订单不是待出货状态");
                return false;
            }
            ids = ids + $inputChecked.val() + ",";
        }
        location.href = "/shipment/create?orderIds=" + ids;
    })

    $("#btnDistribution").on("click", function () {
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
            if ($inputChecked.attr("status") != 0 && $inputChecked.attr("status") != 2) {
                shortTips(orderId + "订单不是未确认状态或设计师设计，不能调配");
                return false;
            }
            ids = ids + orderId + ",";
        }
        $("#modal").modal({
            remote: "/Order/DistributionOrder?orderIds=" + ids
        }).on("hidden.bs.modal", function () {
            $(this).removeData("bs.modal");
        });
    })

    $("#btnNextStep").on("click", function () {
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
        $("#modal").modal({
            remote: "/Order/NextStep?orderIds=" + ids
        }).on("hidden.bs.modal", function () {
            $(this).removeData("bs.modal");
        });
    })

    $("#setStone").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;
        if (length === 0) {
            shortTips("请选择订单");
            return false;
        }
        if (length > 1) {
            shortTips("只能选择一个订单");
            return false;
        }
        location.href = "/order/setStone?orderId=" + inputCheckeds.val();
    })

    $("#btnEdit").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;
        if (length === 0) {
            shortTips("请选择订单");
            return false;
        }
        if (length > 1) {
            shortTips("只能选择一个订单");
            return false;
        }
        location.href = "/order/edit?orderId=" + inputCheckeds.val();
    })

    $("#btnPack").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;
        if (length === 0) {
            shortTips("请选择订单");
            return false;
        }
        if (length > 1) {
            shortTips("只能选择一个订单");
            return false;
        }
        $("#modal").modal({
            remote: "/Order/Pack?orderId=" + inputCheckeds.val()
        }).on("hidden.bs.modal", function () {
            $(this).removeData("bs.modal");
        });
    })

    $("#btnDumpModule").on("click", function () {
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
            content: "确认进入倒模阶段？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/order/GotoDumpModule",
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

    $("#btnDelete").on("click", function () {
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
            content: "确认删除订单？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/order/delete",
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

    $("#btnStop").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;

        if (length === 0) {
            shortTips("请选择订单");
            return false;
        }
        if (length > 1) {
            shortTips("只能选择一个订单");
            return false;
        }

        var status = inputCheckeds.attr("status");
        var id = inputCheckeds.val();
        if (status == -1 || status == 9 || status == 10 || status == 11 || status == 12) {
            shortTips(id + "订单已不再生产中，不能消单");
            return false;
        }

        $("#modal").modal({
            remote: "/Order/Stop?orderId=" + id
        }).on("hidden.bs.modal", function () {
            $(this).removeData("bs.modal");
        });
    })
});