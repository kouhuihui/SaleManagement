$(function () {
    var rec = {
        autoclose: true,
        fontAwesome: true,
        format: "yyyy-mm-dd",
        minView: 2
    }
    $('.date-conditions input[type="text"]').datetimepicker(rec);

    var AccountStatistics = function (data) {
        var self = this;
        self.accountStatistics = ko.observableArray(data);
    }

    var accountStatisticsView = new AccountStatistics([]);
    ko.applyBindings(accountStatisticsView);

    Search();

    function searchArgs() {
        return {
            customerId: $("#customerId").val(),
            StatisticStartDate: $("#createdStartDate").val(),
            StatisticEndDate: $("#createdEndDate").val()
        }
    }

    function Search() {
        $.ajax({
            url: "/Report/AccountStatistics",
            data: searchArgs(),
            type: "Post",
            success: function (rtn) {
                if (rtn.succeeded) {
                    var data = rtn.data;
                    accountStatisticsView.accountStatistics(data);
                }
            },
            error: function () {
            }
        });
    }

    $("#btnSearch").on("click", function () {
        Search();
    })

    $("#btnExport").on("click", function () {
        var params = searchArgs(), url = "/report/accountStatisticsExport?";
        for (var i in params) {
            url += i + '=' + params[i] + '&';
        }
        window.open(url, '_parent');
    })
});