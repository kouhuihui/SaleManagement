$(function () {
    var $from = $("#editMatchStone");
    $from.ajaxForm({
        success: function (result) {
            if (result.succeeded) {
                shortTips("保存成功");
                setTimeout(function () {
                    location.href = "/BasicData/MatchStones";
                }, 1000);
            } else {
                shortTips(errorMessage(result));
            }
        }
    });
})