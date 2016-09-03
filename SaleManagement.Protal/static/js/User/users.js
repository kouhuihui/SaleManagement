$(function () {
	var Users = function (data) {
		var self = this;
		self.users = ko.observableArray(data);		
	}

	var usersView = new Users([]);
	ko.applyBindings(usersView);
	//分页
	$('#usersListPage').pager({
		url: '/User/List',
		pageSize: 10,
		callback: function (data, ui) {
		    usersView.users(data.list);
		}
	});
});