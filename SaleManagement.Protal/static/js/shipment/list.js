﻿$(function () {
    $orderListPage = $("#orderListPage"),
    $shipmentOrderAduitStatus = $("#shipmentOrderAduitStatus"),
    $shipmentOrderId = $("#shipmentOrderId"),
    $deliveryStartDate = $("#deliveryStartDate"),
    $deliveryEndDate = $("#deliveryEndDate"),
    $orderId = $("#orderId"),
    $tbody = $("#tbody");
    var rec = {
        autoclose: true,
        fontAwesome: true,
        format: "yyyy-mm-dd",
        minView: 2
    };
    var customer = new Customer({
        callback: function (data) {
            search();
        }
    });
    $('.date-conditions input[type="text"]').datetimepicker(rec);

    var Orders = function (data) {
        var self = this;
        self.orders = ko.observableArray(data);
    }

    var ordersView = new Orders([]);
    ko.applyBindings(ordersView);
    $tbody.loading();
    //分页
    $orderListPage.pager({
        url: '/Shipment/List',
        pageSize: 20,
        param: searchArgs(),
        method: "GET",
        callback: function (data, ui) {
            ordersView.orders(data.list);
            $tbody.data("loading").hide();
        }
    });

    function searchArgs() {
        return {
            orderId: $orderId.val(),
            shipmentOrderId: $shipmentOrderId.val(),
            customerId: customer.getValue(),
            deliveryStartDate: $deliveryStartDate.val(),
            deliveryEndDate: $deliveryEndDate.val(),
            status: $shipmentOrderAduitStatus.val()
        }
    }

    function search() {
        var pager = $orderListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(1);
        $tbody.data("loading").show();
    }

    function getData() {
        var pager = $orderListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(pager.opts.currentPage);
        $tbody.data("loading").show();
    }

    $shipmentOrderId.bind('keypress', function (event) {
        if (event.keyCode == "13") {
            var shipmentOrderId = $shipmentOrderId.val();
            if (shipmentOrderId != "" && shipmentOrderId.lastIndexOf(',') != shipmentOrderId.length - 1) {
                $shipmentOrderId.val(shipmentOrderId + ",");
            }
            search();
            $shipmentOrderId.focus();
        }
    });

    $orderId.bind('keypress', function (event) {
        if (event.keyCode == "13") {
            search();
            $orderId.focus();
        }
    });

    $shipmentOrderAduitStatus.bind('change', function () {
        search();
    })

    $deliveryStartDate.bind('change', function () {
        search();
    })

    $deliveryEndDate.bind('change', function () {
        search();
    })

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
                var $okModalBtn = $el.find(".okModalBtn");
                $okModalBtn.attr("disabled", true)
                $.ajax({
                    url: "/shipment/Audit?id=" + id,
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("操作成功"); 
                            getData();
                        } else {
                            shortTips(errorMessage(result));
                        }
                    },
                    error: function (result) {
                        $okModalBtn.attr("disabled", false);
                    }
                });
            }
        });
    })

    $("#btnCancelAudit").on("click", function () {
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
        if ($inputChecked.attr("status") != 1) {
            shortTips("出货单不是已审核状态");
            return false;
        }
        var id = $inputChecked.val();
        $(window).modalDialog({
            title: "提示",
            content: "确定取消审核" + id + "出货单？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $orderListPage.loading();
                $.ajax({
                    url: "/shipment/CancelAudit?id=" + id,
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("操作成功");
                            getData();
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

    $("#btnCreatedRecord").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;
        if (length === 0) {
            shortTips("请选择出货单");
            return false;
        }
        var ids = "";
        for (var i = 0; i < length; i++) {
            var $inputChecked = $(inputCheckeds[i]);
            if ($inputChecked.attr("status") != 0) {
                shortTips("出货单不是待审核状态");
                return false;
            }
            ids = ids + $inputChecked.val() + ",";
        }
         
        $(window).modalDialog({
            title: "提示",
            content: "确定生成出货记录？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $orderListPage.loading();
                $.ajax({
                    url: "/shipment/CreatedRecord?orderIds=" + ids,
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("操作成功");
                            getData();
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