$(function () {
	var rec = {
		autoclose: true,
		fontAwesome: true,
		format: "yyyy-mm-dd",
		minView: 2
	};
	$('.date-conditions input[type="text"]').datetimepicker(rec);

	var ShipmentStatistics = function (data) {
		var self = this;
		self.shipmentStatistics = ko.observableArray(data);
	}

	var shipmentStatisticsView = new ShipmentStatistics([]);
	ko.applyBindings(shipmentStatisticsView);

	Search();

	function searchArgs() {
		return {
			StatisticStartDate: $("#createdStartDate").val(),
			StatisticEndDate: $("#createdEndDate").val()
		}
	}

	function Search() {
		$.ajax({
			url: "/Customer/Order/ShipmentStatistics",
			data: searchArgs(),
			type: "Post",
			success: function (rtn) {
				if (rtn.succeeded) {
					var data = rtn.data;
					shipmentStatisticsView.shipmentStatistics(data);
				}
			},
			error: function () {
			}
		});
	}

	$("#btnSearch").on("click", function() {
		Search();
	});

	$("#btnExport").on("click", function() {
		var params = searchArgs(), url = "/customer/order/ShipmentStatisticsExport?";
		for (var i in params) {
			url += i + '=' + params[i] + '&';
		}
		window.open(url, '_parent');
	});
});