$(function () {
    var $from = $("#userCreate");
    $from.ajaxForm({
        success: function (result) {
        	if (result.succeeded) {
        	    shortTips("保存成功");
        	    location.href = "/user/list";
            } else {
                shortTips(errorMessage(result));
            }
        }
    });
})