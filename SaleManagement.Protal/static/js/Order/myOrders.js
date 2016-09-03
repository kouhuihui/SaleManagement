$(function () {
    var Orders = function (data) {
        var self = this;
        self.orders = ko.observableArray(data);
        self.uploadDesginImageClick = function (item, el) {
            $("#modal").modal({
                remote: "/order/" + item.id + "/upload/desginImage"
            })
        },
        self.CustomerTobeConfirmClick = function (item, el) {
            $(window).modalDialog({
                title: "提示",
                smallTitle: "",
                content: "确认进入客户确认阶段</br>起版/出蜡(元)：<input style=\"width:40px\" name=\"outputWaxCost\"/>",
                type: "confirm",
                okCallBack: function (e, $el) {
                    var outputWaxCost =$("input[name=outputWaxCost]").val();
                    if(isNaN(Number(outputWaxCost))){
                        outputWaxCost = 0;
                    }
                    $.ajax({
                        url: "/order/" + item.id + "/CustomerTobeConfirm",
                        type: "POST",
                        dataType: "json",
                        data: { "outputWaxCost": outputWaxCost },
                        success: function (result) {
                            if (result.succeeded) {
                                $el.data("bs.modal").hide();
                                self.orders.remove(item);
                            } else {
                                shortTips(errorMessage(result));
                            }
                        }
                    });
                }
            });
        }
    }

    var ordersView = new Orders([]);
    ko.applyBindings(ordersView);
    //分页
    $('#orderListPage').pager({
        url: '/Order/MyOrders',
        pageSize: 10,
        callback: function (data, ui) {
            ordersView.orders(data.list);
        }
    });
});