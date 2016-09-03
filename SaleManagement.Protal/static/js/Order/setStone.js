$(function () {
	var $from = $("#setStoneForm");
		$from.ajaxForm({
			success: function (result) {
				if (result.succeeded) {
					shortTips("保存成功");
				} else {
					shortTips(errorMessage(result));
				}
			}
		});
});