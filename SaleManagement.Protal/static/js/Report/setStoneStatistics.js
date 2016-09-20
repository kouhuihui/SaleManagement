$(function () {
    var rec = {
        autoclose: true,
        fontAwesome: true,
        format: "yyyy-mm-dd",
        minView: 2
    }
    $('.date-conditions input[type="text"]').datetimepicker(rec);

    var SetStoneStatistics = function (data) {
        var self = this;
        self.setStoneStatistics = ko.observableArray(data);
    }

    var setStoneStatisticsView = new SetStoneStatistics([]);
    ko.applyBindings(setStoneStatisticsView);

    Search();

    function searchArgs() {
        return {
            StatisticStartDate: $("#createdStartDate").val(),
            StatisticEndDate: $("#createdEndDate").val()
        }
    }

    function Search() {
        $.ajax({
            url: "/Report/OrderSetStoneStatistics",
            data: searchArgs(),
            type: "Post",
            success: function (rtn) {
                if (rtn.succeeded) {
                    var data = rtn.data;
                    setStoneStatisticsView.setStoneStatistics(data);
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
        var params = searchArgs(), url = "/report/OrderSetStoneStatisticsExport?";
        for (var i in params) {
            url += i + '=' + params[i] + '&';
        }
        window.open(url, '_parent');
    })
});