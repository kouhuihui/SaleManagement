$(function () {
    var $spotGoodsOrderListPage = $('#spotGoodsOrderListPage'),
        $tbody = $("#tbody");

    var SpotGoodsOrders = function (data) {
        var self = this;
        self.spotGoodsOrders = ko.observableArray(data)
    }

    var spotGoodsOrdersView = new SpotGoodsOrders([]);
    ko.applyBindings(spotGoodsOrdersView);
    $tbody.loading();
    $spotGoodsOrderListPage.pager({
        url: '/SpotGoods/SellList',
        pageSize: 20,
        param: "",
        method: "GET",
        callback: function (data, ui) {
            $tbody.data("loading").hide();
            spotGoodsOrdersView.spotGoodsOrders(data.list);
        }
    });
})