$(function () {
    var customer = new Customer({
        callback: function (data) {
            search();
        }
    });
    var $orderListPage = $('#orderListPage'),
        $orderId = $("#orderId"),
        $status = $("#Status"),
        $colorFormId = $("#colorFormId"),
        $urgentStatus = $("#UrgentStatus"),
        $outPutWaxDate = $("#outPutWaxDate"),
        $tbody = $("#tbody"),
        $chxProcess = $("#chxProcess"),
        $process = $("#IsProcess")

    var rec = {
        autoclose: true,
        fontAwesome: true,
        format: "yyyy-mm-dd",
        minView: 2
    }
    $('.date-conditions input[type="text"]').datetimepicker(rec);
    
 
    var Orders = function (data) {
        var self = this;
        self.orders = ko.observableArray(data),
        self.viewProcessClick = function (item, el) {
            $("#modal").modal({
                remote: "/Order/process?orderId=" + item.id
            }).on("hidden.bs.modal", function () {
                $(this).removeData("bs.modal");
            });
        }
    }

    var ordersView = new Orders([]);
    ko.applyBindings(ordersView);
    $tbody.loading();
    $orderListPage.pager({
        url: '/Order/List',
        pageSize: 20,
        param: searchArgs(),
        method: "GET",
        callback: function (data, ui) {
            $tbody.data("loading").hide();
            ordersView.orders(data.list);
        }
    });

    function searchArgs() {
        return {
            orderId: $orderId.val(),
            customerId: customer.getValue(),
            status: $status != undefined ? $status.val() : "",
            colorFormId: $colorFormId.val(),
            urgentStatus: $("#UrgentStatus").val(),
            rushStatus: $("#RushStatus").val(),
            outPutWaxDate: $outPutWaxDate.val(),
            deliveryStartDate: $("#DeliveryStartDate").val(),
            deliveryEndDate: $("#DeliveryEndDate").val(),
            isProcess: $process.val()
        }
    }

    function search() {
        var pager = $orderListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(pager.opts.currentPage);
        $tbody.data("loading").show();
    }

    function Query() {
        var pager = $orderListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(1);
        $tbody.data("loading").show();
    }

    $("#btnSearch").on("click", function () {
        Query();
    })

    $orderId.bind('keypress', function (event) {
        if (event.keyCode == "13") {
            var orderId = $orderId.val();
            if (orderId != "" && orderId.lastIndexOf(',') != orderId.length - 1) {
                $orderId.val(orderId + ",");
            }
            Query();
            $orderId.focus();
        }
    });


    $status.bind('change', function () {
        var status = $status.val();
        if (status === '-1' || status === "1" || status === "11" || status === "12") {
            $chxProcess.attr("checked", false);
        }
        Query();
    })

    $colorFormId.bind('change', function () {
        Query();
    })

    $urgentStatus.bind('change', function () {
        Query();
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
            Query();
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
                            shortTips("修改成功");
                            search();
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
                            shortTips("修改成功");
                            search();
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

    $("#btnSetDesginCost").on("click", function () {
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
        var id = inputCheckeds.val();
        var outputWaxCost = inputCheckeds.attr("outputWaxCost");
        $(window).modalDialog({
            title: "提示",
            smallTitle: "",
            content: "设计费用(元)：<input style=\"width:40px\" name=\"outputWaxCost\" value=\"" + outputWaxCost + "\"/>",
            type: "confirm",
            okCallBack: function (e, $el) {
                var outputWaxCost = $("input[name=outputWaxCost]").val();
                if (isNaN(Number(outputWaxCost))) {
                    outputWaxCost = 0;
                }
                $.ajax({
                    url: "/order/" + id + "/SetOutputwaxCost",
                    type: "POST",
                    dataType: "json",
                    data: { "outputWaxCost": outputWaxCost },
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("修改成功");
                            search();
                        } else {
                            shortTips(errorMessage(result));
                        }
                    }
                });
            }
        });
    })

    $("#waitStone").on("click", function () {
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
            content: "确认进入等石阶段？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/order/GotoWaitStone",
                    type: "POST",
                    dataType: "json",
                    data: { "orderIds": ids },
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("修改成功");
                            search();
                        } else {
                            shortTips(errorMessage(result));
                        }
                    }
                });
            }
        });
    })

    $chxProcess.change(function () {
        if ($chxProcess.is(':checked')) {
            $process.val("true");
            $status.val('');
        }
        else {
            $process.val("false");
        }
        Query();
    })
});