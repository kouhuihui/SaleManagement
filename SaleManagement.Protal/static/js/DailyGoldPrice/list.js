$(function () {
	var Prices = function (data) {
		var self = this;
		self.prices = ko.observableArray(data);
		self.editClick = function (item, el) {
		    $("#modal").modal({
			    remote: "/DailyGoldPrice/Edit?id=" + item.id+"&t="+ new Date().getTime()
		    }).on("hidden.bs.modal", function () {
		        $(this).removeData("bs.modal");
		    });
		};
		self.deleteClick = function (item, el) {
			$(window).modalDialog({
				title: "提示",
				smallTitle: "",
				content: "确定删除该条金价记录",
				type: "confirm",
				okCallBack: function (e, $el) {
					$.ajax({
						url: "/DailyGoldPrice/Delete",
						type: "POST",
						data: {
							Id: item.id
						},
						dataType: "json",
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

	var pricesView = new Prices([]);
	ko.applyBindings(pricesView);
	//分页
	$('#dailyGoldPriceListPage').pager({
		url: '/DailyGoldPrice/List',
		pageSize: 10,
		callback: function (data, ui) {
			pricesView.prices(data.list);
		}
	});
});