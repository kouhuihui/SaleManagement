$(function () {
    var $from = $("#editDailyGoldPriceForm");
	$("input[name=Date]").datetimepicker({
		autoclose: true,
		fontAwesome: true,
		format: "yyyy-mm-dd",
		minView: 2,
		initialDate: new Date()
	});

	$from.ajaxForm({
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