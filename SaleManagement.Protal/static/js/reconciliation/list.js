$(function () {
    var $reconciliationListPage = $('#reconciliationListPage');
    var rec = {
        autoclose: true,
        fontAwesome: true,
        format: "yyyy-mm-dd",
        minView: 2
    }
    $('.date-conditions input[type="text"]').datetimepicker(rec);
    var customer = new Customer();
    var Reconciliations = function (data) {
        var self = this;
        self.reconciliations = ko.observableArray(data);
    }

    var reconciliationsView = new Reconciliations([]);
    ko.applyBindings(reconciliationsView);
    //分页
    $reconciliationListPage.pager({
        url: '/Reconciliation/List',
        pageSize: 10,
        param: searchArgs(),
        method: "GET",
        callback: function (data, ui) {
            reconciliationsView.reconciliations(data.list);
        }
    });

    function searchArgs() {
        return {
            customerId: customer.getValue(),
            createdStartDate: $("#createdStartDate").val(),
            createdEndDate: $("#createdEndDate").val(),
            type: $("#type").val()
        }
    }

    function search() {
        var pager = $reconciliationListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(1);
    }

    $("#btnSearch").on("click", function () {
        search();
    })

    $("#btnAudit").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;
        if (length === 0) {
            shortTips("请选择出货单");
            return false;
        }
        if (length > 1) {
            shortTips("只能选择一个出货单");
            return false;
        }
        var id = inputCheckeds.val();
        $(window).modalDialog({
            title: "提示",
            content: "确定审核通过" + id + "出货单？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/shipment/Audit?id=" + id,
                    type: "POST",
                    dataType: "json",
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
    })

    $("#btnExport").on("click", function () {
        var params = searchArgs(), url = "/Reconciliation/Export?";
        for (var i in params) {
            url += i + '=' + params[i] + '&';
        }
        window.open(url, '_parent');
    })
});