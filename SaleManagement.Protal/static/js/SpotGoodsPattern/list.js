$(function () {
    var $spotGoodsPatternListPage = $('#spotGoodsPatternListPage'),
        $tbody = $("#tbody");

    var SpotGoodsPatterns = function (data) {
        var self = this;
        self.spotGoodsPatterns = ko.observableArray(data),
        self.EditClick = function (item, el) {
            $("#modal").modal({
                remote: "/SpotGoodsPattern/Edit?Id=" + item.id
            }).on("hidden.bs.modal", function () {
                $(this).removeData("bs.modal");
                search();
            })
        }
    }

    var spotGoodsPatternsView = new SpotGoodsPatterns([]);
    ko.applyBindings(spotGoodsPatternsView);
    $tbody.loading();
    $spotGoodsPatternListPage.pager({
        url: '/spotGoodsPattern/List',
        pageSize: 20,
        param: "",
        method: "GET",
        callback: function (data, ui) {
            $tbody.data("loading").hide();
            spotGoodsPatternsView.spotGoodsPatterns(data.list);
        }
    });

    function search() {
        var pager = $spotGoodsPatternListPage.data("pager");
        pager.jump(pager.opts.currentPage);
        $tbody.data("loading").show();
    }

    $("#btnAdd").on("click", function () {
        $("#modal").modal({
            remote: "/SpotGoodsPattern/Add"
        }).on("hidden.bs.modal", function () {
            $(this).removeData("bs.modal");
            search();
        });
    })
})