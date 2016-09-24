$(function () {
    var $usersListPage = $('#usersListPage'),
        $userStatus = $("#userStatus");

    $("#userStatus option[value=1]").attr("selected", true);
	var Users = function (data) {
		var self = this;
		self.users = ko.observableArray(data);		
	}

	var usersView = new Users([]);
	ko.applyBindings(usersView);
	//分页
	$usersListPage.pager({
		url: '/User/List',
		pageSize: 10,
		param: searchArgs(),
		method: "GET",
		callback: function (data, ui) {
		    usersView.users(data.list);
		}
	});

	function searchArgs() {
		return {
			userName: $("#userName").val(),
			status: $("#userStatus").val()
		}
	}

	function search() {
		var pager = $usersListPage.data("pager");
		pager.opts.param = searchArgs();
		pager.jump(1);
	}

	$("#btnSearch").on("click", function () {
		search();
	})

	function updateUserStatus(operation,status) {
		var inputCheckeds = $("#tbody input:checkbox:checked");
		var length = inputCheckeds.length;
		var ids = "";
		if (length === 0) {
			shortTips("请选择用户");
			return false;
		}
		for (var i = 0; i < length; i++) {
			var $inputChecked = $(inputCheckeds[i]);
			ids = ids + $inputChecked.val() + ",";
		}
		$(window).modalDialog({
			title: "提示",
			smallTitle: "",
			content: "确认" + operation + "选择的用户？",
			type: "confirm",
			okCallBack: function (e, $el) {
				$.ajax({
					url: "/user/updateUserStatus",
					type: "POST",
					dataType: "json",
					data: { "userIds": ids, "status": status },
					success: function (result) {
						if (result.succeeded) {
							$el.data("bs.modal").hide();
							location.reload();
						} else {
							shortTips(errorMessage(result));
						}
					}
				});
			}
		});
	}

	$("#btnDisabled").on("click", function () {
		updateUserStatus("禁用", 0);
	});

	$("#btnEnabled").on("click", function () {
		updateUserStatus("启用", 1);
	});
	$("#btnRestPassword").on("click", function () {
	    var inputCheckeds = $("#tbody input:checkbox:checked");
	    var length = inputCheckeds.length;
	    if (length === 0) {
	        shortTips("请选择用户");
	        return false;
	    }
	    if (length > 1) {
	        shortTips("只能选择一个用户");
	        return false;
	    }
	    $("#modal").modal({
	        remote: "/User/ResetPassword?userId=" + inputCheckeds.val()
	    }).on("hidden.bs.modal", function () {
	        $(this).removeData("bs.modal");
	    });
	});

});