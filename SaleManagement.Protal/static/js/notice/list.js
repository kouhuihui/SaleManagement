$(function () {
	var Notices = function (data) {
		var self = this;
		self.notices = ko.observableArray(data);
		self.editClick = function (item, el) {
			$("#modal").modal({
				remote: "/Notice/Edit?id=" + item.id
			});
		};
		self.deleteClick = function (item, el) {
			$(window).modalDialog({
				title: "提示",
				smallTitle: "",
				content: "确定删除该条公告",
				type: "confirm",
				okCallBack: function (e, $el) {
					$.ajax({
						url: "/Notice/Delete",
						type: "POST",
						data: {
							Id: item.id
						},
						dataType: "json",
						success: function (result) {
							if (result.succeeded) {
								$el.data("bs.modal").hide();
								self.notices.remove(item);
							} else {
								shortTips(errorMessage(result));
							}
						}
					});
				}
			});
		}
	}

	var noticesView = new Notices([]);
	ko.applyBindings(noticesView);
	//分页
	$('#noticeListPage').pager({
		url: '/Notice/List',
		pageSize: 10,
		callback: function (data, ui) {
			noticesView.notices(data.list);
		}
	});
});