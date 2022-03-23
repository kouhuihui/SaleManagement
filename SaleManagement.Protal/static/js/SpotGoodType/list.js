$(function () {
    var $spotGoodTypeListPage = $('#spotGoodTypeListPage'),
        $tbody = $("#tbody");

    var SpotGoodTypes = function (data) {
        var self = this;
        self.spotGoodTypes = ko.observableArray(data),
        self.EditClick = function (item, el) {
            $("#modal").modal({
                remote: "/SpotGoodType/Edit?Id=" + item.id
            }).on("hidden.bs.modal", function () {
                $(this).removeData("bs.modal");
                search();
            })
        }
    }

    var spotGoodTypesView = new SpotGoodTypes([]);
    ko.applyBindings(spotGoodTypesView);
    $tbody.loading();
    $spotGoodTypeListPage.pager({
        url: '/SpotGoodType/List',
        pageSize: 20,
        param: "",
        method: "GET",
        callback: function (data, ui) {
            $tbody.data("loading").hide();
            spotGoodTypesView.spotGoodTypes(data.list);
        }
    });

    function search() {
        var pager = $spotGoodTypeListPage.data("pager");
        pager.jump(pager.opts.currentPage);
        $tbody.data("loading").show();
    }

    $("#btnAdd").on("click", function () {
        $("#modal").modal({
            remote: "/SpotGoodType/Add"
        }).on("hidden.bs.modal", function () {
            $(this).removeData("bs.modal");
            search();
        });
    })
})