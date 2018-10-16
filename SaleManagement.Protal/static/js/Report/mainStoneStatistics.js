$(function () {
    var rec = {
        autoclose: true,
        fontAwesome: true,
        format: "yyyy-mm-dd",
        minView: 2
    };

    $orderId = $("#orderId");
    var customer = new Customer();
    $('.date-conditions input[type="text"]').datetimepicker(rec);

    var MainStoneStatistics = function (data) {
        var self = this;
        self.mainStoneStatistics = ko.observableArray(data);
    }

    var mainStoneStatisticsView = new MainStoneStatistics([]);
    ko.applyBindings(mainStoneStatisticsView);

    Search();

    function searchArgs() {
        return {
            StatisticStartDate: $("#createdStartDate").val(),
            StatisticEndDate: $("#createdEndDate").val(),
            customerId: customer.getValue(),
            orderId: $orderId.val()
        }
    }

    function Search() {
        $.ajax({
            url: "/Report/OrderMainStoneStatistics",
            data: searchArgs(),
            type: "Post",
            success: function (rtn) {
                if (rtn.succeeded) {
                    var data = rtn.data;
                    mainStoneStatisticsView.mainStoneStatistics(data);
                }
            },
            error: function () {
            }
        });
    }

    $("#btnSearch").on("click", function () {
        Search();
    })

    $("#btnExport").on("click", function() {
        var params = searchArgs(), url = "/report/OrderMainStoneStatisticsExport?";
        for (var i in params) {
            url += i + '=' + params[i] + '&';
        }
        window.open(url, '_parent');
    });

    $orderId.bind('keypress', function (event) {
        if (event.keyCode == "13") {
            var orderId = $orderId.val();
            if (orderId != "" && orderId.lastIndexOf(',') != orderId.length - 1) {
                $orderId.val(orderId + ",");
            }
            Search();
            $orderId.focus();
        }
    });
});