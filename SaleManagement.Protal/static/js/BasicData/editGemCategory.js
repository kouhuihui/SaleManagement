$(function () {
    var $from = $("#editGemCategory");
    $from.ajaxForm({
        success: function (result) {
            if (result.succeeded) {
                shortTips("保存成功");
                setTimeout(function () {
                    location.href = "/BasicData/GemCategories";
                }, 1000);
            } else {
                shortTips(errorMessage(result));
            }
        }
    });
})