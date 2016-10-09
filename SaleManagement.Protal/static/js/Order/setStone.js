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

		$(".btn-deleted").click(function () {
			var id = $(this).attr("data-id");
			$(window).modalDialog({
				title: "提示",
				smallTitle: "",
				content: "确定删除该配石吗？",
				type: "confirm",
				okCallBack: function (e, $el) {
					$.ajax({
						url: "/order/DeleteSetStone/" + id,
						type: "POST",
						dataType: "json",
						success: function (result) {
							if (result.succeeded) {
								$el.data("bs.modal").hide();
								shortTips("删除成功");
								setTimeout(function () {
									location.reload();
								}, 1000);
							} else {
								shortTips(errorMessage(result));
							}
						}
					});
				}
			});
		})
});