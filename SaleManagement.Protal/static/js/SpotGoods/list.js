$(function () {
    var $spotGoodsListPage = $('#spotGoodsListPage'),
        $tbody = $("#tbody");

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
        param: "",
        method: "GET",
        callback: function (data, ui) {
            $tbody.data("loading").hide();
            spotGoodsView.spotGoods(data.list);
        }
    });
})