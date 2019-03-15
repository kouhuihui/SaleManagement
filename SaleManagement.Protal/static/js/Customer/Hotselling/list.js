$(function () {
    var $hotSellingListPage = $('#hotSellingListPage');

    var HotSellings = function (data) {
        var self = this;
        self.hotSellings = ko.observableArray(data); 
    }

    var hotSellingsView = new HotSellings([]);
    ko.applyBindings(hotSellingsView);
    //分页
    $hotSellingListPage.pager({
        url: '/hotSelling/Setting',
        pageSize: 10,
        param: searchArgs(),
        method: "GET",
        callback: function (data, ui) {
            hotSellingsView.hotSellings(data.list);
        }
    });

    function searchArgs() {
        return {

        }
    }

    function search() {
        var pager = $hotSellingListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(1);
    }

    $("#btnSearch").on("click", function () {
        search();
    });

});