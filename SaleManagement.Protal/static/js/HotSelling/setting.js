$(function () {
    var $hotSellingListPage = $('#hotSellingListPage');
    
    var HotSellings = function (data) {
        var self = this;
        self.hotSellings = ko.observableArray(data);
        self.deleteClick = function (item, el) {
            $(window).modalDialog({
                title: "提示",
                smallTitle: "",
                content: "确定删除该条热销款",
                type: "confirm",
                okCallBack: function (e, $el) {
                    $.ajax({
                        url: "/HotSelling/Delete",
                        type: "POST",
                        data: {
                            Id: item.id
                        },
                        dataType: "json",
                        success: function (result) {
                            if (result.succeeded) {
                                $el.data("bs.modal").hide();
                                self.hotSellings.remove(item);
                            } else {
                                shortTips(errorMessage(result));
                            }
                        }
                    });
                }
            });
        }
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

    $("#btnSearch").on("click", function() {
        search();
    }); 

});