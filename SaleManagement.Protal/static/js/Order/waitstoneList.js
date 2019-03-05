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
        url: '/Order/WaitStoneList',
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
            WaitStone: $("#waitStoneStatus").val()
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

    $("#sendwaitStonemsg").on("click", function () {
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

        $(window).modalDialog({
            title: "提示",
            smallTitle: "",
            content: "发送催客户邮寄主石消息",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/order/" + id + "/WaitStoneMsg",
                    type: "POST",
                    dataType: "json",
                    data: {},
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            shortTips("发送成功");
                            search();
                        } else {
                            shortTips(errorMessage(result));
                        }
                    }
                });
            }
        });
    })


    $("#receivedStone").on("click", function () {
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
        location.href = "/order/setMainStone?orderId=" + inputCheckeds.val();
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

function setDeliverDay(value) {
    $("input[name=deliverDay]").val(value);
}