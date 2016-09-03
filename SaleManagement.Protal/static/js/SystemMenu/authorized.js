$(function () {
	var $from = $("#SystemMenuAuthorized");
	$from.ajaxForm({
		success: function (result) {
			if (result.succeeded) {
				shortTips("保存成功");
				setTimeout(function () {
					location.href = "/Role/List";
				}, 1000);
			} else {
				shortTips(errorMessage(result));
			}
		}
	});
})