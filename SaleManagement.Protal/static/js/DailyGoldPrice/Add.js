$(function () {
    var $from = $("#addDailyGoldPriceForm");

    $from.ajaxForm({
        beforeSubmit: function () {
            var prices = $("tbody input[type=text]");
            for (var i = 0, len = prices.length; i < len; i++) {
                var $prices = $(prices[i]);
                var price = Number($prices.val());
                if (isNaN(price) || price <= 0) {
                    shortTips("请输入正确的金价");
                    $prices.val("");
                    $prices.focus();
                    return false;
                }
            }
            return true;
        },
        success: function (result) {
            if (result.succeeded) {
                shortTips("保存成功");
                location.href = "/DailyGoldPrice/list";
            } else {
                shortTips(errorMessage(result));
            }
        }
    });
})