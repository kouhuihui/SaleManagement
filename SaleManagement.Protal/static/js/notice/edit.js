$(function () {
    var $from = $("#editNoticeForm");
	$from.ajaxForm({
		success: function (result) {
			if (result.succeeded) {
				shortTips("保存成功");
				location.href = "/Notice/list";
			} else {
				shortTips(errorMessage(result));
			}
		}
	});
})