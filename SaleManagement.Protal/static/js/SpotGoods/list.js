$(function () {
    var $spotGoodsListPage = $('#spotGoodsListPage'),
        $tbody = $("#tbody"),
        $orderId = $("#orderId"),
        $status = $("#Status");

    var SpotGoods = function (data) {
        var self = this;
        self.spotGoods = ko.observableArray(data)
    }

    var spotGoodsView = new SpotGoods([]);
    ko.applyBindings(spotGoodsView);
    $tbody.loading();
    $spotGoodsListPage.pager({
        url: '/SpotGoods/List',
        pageSize: 20,
        param: searchArgs(),
        method: "GET",
        callback: function (data, ui) {
            $tbody.data("loading").hide();
            spotGoodsView.spotGoods(data.list);
        }
    });

    function searchArgs() {
        return {
            orderIds: $orderId.val(),
            status: $status != undefined ? $status.val() : ""
        }
    }

    function search() {
        var pager = $spotGoodsListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(pager.opts.currentPage);
        $tbody.data("loading").show();
    }

    function Query() {
        var pager = $spotGoodsListPage.data("pager");
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