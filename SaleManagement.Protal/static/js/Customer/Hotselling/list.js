$(function () {
    var $hotSellingListPage = $('#hotSellingListPage');
    var spotGoodTypeId = "";

    var HotSellings = function (data) {
        var self = this;
        self.hotSellings = ko.observableArray(data); 
    }

    var hotSellingsView = new HotSellings([]);
    ko.applyBindings(hotSellingsView);
    //分页
    $hotSellingListPage.pager({
        url: '/SpotGoodsPattern/List',
        pageSize: 10,
        param: searchArgs(),
        method: "GET",
        callback: function (data) {
            hotSellingsView.hotSellings(data.list);
        }
    });

    getSpotGoodType();
    function getSpotGoodType() {
        $.ajax({
            url: "/SpotGoodType/All",
            type: "Get",
            success: function (rtn) {
                if (rtn.succeeded) {
                    var html = "";
                    for (var i = 0; i < rtn.data.length; i++) {
                        html += "<button id='" + rtn.data[i].id + "' type='button' class='btn btn-default'  name='btnSpan'>" + rtn.data[i].name + "</button>";
                    }
                    $("#spotgoodTypeDiv").html(html); 
                    $("button[name=btnSpan]").click(function () {
                        var $spanBtn = $(this);
                        $spanBtn.siblings().removeClass("btn-primary");
                        $spanBtn.addClass("btn-primary");
                        spotGoodTypeId = $spanBtn.attr("id");
                        search();
                    });
                }
            },
            error: function () {
            }
        });
    }

    function searchArgs() {
        return {
            spotGoodTypeId: spotGoodTypeId

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