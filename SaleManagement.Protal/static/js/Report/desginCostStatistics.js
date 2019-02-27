$(function () {
    var rec = {
        autoclose: true,
        fontAwesome: true,
        format: "yyyy-mm-dd",
        minView: 2
    }; 
    $desginCostList = $("#desginCostList");
    $('.date-conditions input[type="text"]').datetimepicker(rec);
    $desginCostList.loading();
    var DesginCostStatistics = function (data) {
        var self = this;
        self.desginCostStatistics = ko.observableArray(data);
    }

    var desginCostStatisticsView = new DesginCostStatistics([]);
    ko.applyBindings(desginCostStatisticsView);

    Search();

    function searchArgs() {
        return { 
            StatisticStartDate: $("#createdStartDate").val(),
            StatisticEndDate: $("#createdEndDate").val()
        }
    }

    function Search() {
        $.ajax({
            url: "/Report/DesginCostStatistics",
            data: searchArgs(),
            type: "Post",
            success: function (rtn) {
                $desginCostList.data("loading").hide();
                if (rtn.succeeded) {
                    var data = rtn.data;
                    desginCostStatisticsView.desginCostStatistics(data);
                }
            },
            error: function () {
            }
        });
    }

    $("#btnSearch").on("click", function () {
        $desginCostList.data("loading").show();
        Search();
    })

    $("#btnExport").on("click", function () {
        var params = searchArgs(), url = "/report/DesginCostStatisticsExport?";
        for (var i in params) {
            url += i + '=' + params[i] + '&';
        }
        window.open(url, '_parent');
    })
});