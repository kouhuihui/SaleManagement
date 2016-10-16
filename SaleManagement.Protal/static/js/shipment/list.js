﻿$(function () {
    $orderListPage = $("#orderListPage");
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
    }

    var ordersView = new Orders([]);
    ko.applyBindings(ordersView);
    //分页
    $orderListPage.pager({
        url: '/Shipment/List',
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
            shipmentOrderId: $("#shipmentOrderId").val(),
            customerId: $("#customerId").val(),
            deliveryStartDate: $("#deliveryStartDate").val(),
            deliveryEndDate: $("#deliveryEndDate").val(),
            status: $("#shipmentOrderAduitStatus").val()
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

    $("#btnAudit").on("click", function () {
        var $inputChecked = $("#tbody input:checkbox:checked");
        var length = $inputChecked.length;
        if (length === 0) {
            shortTips("请选择出货单");
            return false;
        }
        if (length > 1) {
            shortTips("只能选择一个出货单");
            return false;
        }
        if ($inputChecked.attr("status") != 0) {
            shortTips("出货单不是待审核状态");
            return false;
        }
        var id = $inputChecked.val();
        $(window).modalDialog({
            title: "提示",
            content: "确定审核通过" + id + "出货单？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $orderListPage.loading();
                $.ajax({
                    url: "/shipment/Audit?id=" + id,
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            setTimeout(function () {
                                shortTips("操作成功");
                            }, 1000)
                            location.reload();
                        } else {
                            shortTips(errorMessage(result));
                        }
                    },
                    error: function (result) {
                        $orderListPage.data("loading").hide();
                    }
                });
            }
        });
    })
});