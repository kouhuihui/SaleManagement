$(function () {
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
        self.orders = ko.observableArray(data);
        self.distributionClick = function (item, el) {
            $("#modal").modal({
                remote: "/Order/" + item.id + "/Distribution"
            })
        },
        self.nextStepClick = function (item, el) {
            $("#modal").modal({
                remote: "/Order/" + item.id + "/NextStep"
            })
        }
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
            customerId: $("#customerId").val(),
            deliveryStartDate: $("#deliveryStartDate").val(),
            deliveryEndDate: $("#deliveryEndDate").val(),
            status: $("#orderStatus").val()
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

    $("#shipment").on("click",function () {
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
            ids = ids + $inputChecked.val() +",";
        }
        location.href = "/shipment/create?orderIds=" + ids;
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
});