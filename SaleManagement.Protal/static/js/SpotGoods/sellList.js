$(function () {
    var $spotGoodsOrderListPage = $('#spotGoodsOrderListPage'),
        $tbody = $("#tbody"),
        $orderId = $("#orderId"),
        $status = $("#Status");

    var SpotGoodsOrders = function (data) {
        var self = this;
        self.spotGoodsOrders = ko.observableArray(data);
        self.editClick = function (item, el) {
            $("#modal").modal({
                remote: "/SpotGoods/EditOrder?Id=" + item.id
            }).on("hidden.bs.modal", function () {
                $(this).removeData("bs.modal");
            });
        }
    }

    var spotGoodsOrdersView = new SpotGoodsOrders([]);
    ko.applyBindings(spotGoodsOrdersView);
    $tbody.loading();
    $spotGoodsOrderListPage.pager({
        url: '/SpotGoods/SellList',
        pageSize: 20,
        param: searchArgs(),
        method: "GET",
        callback: function (data, ui) {
            $tbody.data("loading").hide();
            spotGoodsOrdersView.spotGoodsOrders(data.list);
        }
    });


    function searchArgs() {
        return {
            orderIds: $orderId.val(),
            status: $status != undefined ? $status.val() : ""
        }
    }

    function search() {
        var pager = $spotGoodsOrderListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(pager.opts.currentPage);
        $tbody.data("loading").show();
    }

    function Query() {
        var pager = $spotGoodsOrderListPage.data("pager");
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
})