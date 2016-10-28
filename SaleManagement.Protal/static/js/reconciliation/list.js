$(function () {
    var $reconciliationListPage = $('#reconciliationListPage'),
             $tbody = $("#tbody");
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
    $tbody.loading();
    $reconciliationListPage.pager({
        url: '/Reconciliation/List',
        pageSize: 20,
        param: searchArgs(),
        method: "GET",
        callback: function (data, ui) {
            $tbody.data("loading").hide();
            reconciliationsView.reconciliations(data.list);
        }
    });

    function searchArgs() {
        return {
            customerId: customer.getValue(),
            createdStartDate: $("#createdStartDate").val(),
            createdEndDate: $("#createdEndDate").val(),
            type: $("#type").val(),
            arrearageType: $("#arrearageType").val()
        }
    }

    function search() {
        var pager = $reconciliationListPage.data("pager");
        pager.opts.param = searchArgs();
        pager.jump(1);
        $tbody.data("loading").show();
    }

    $("#btnSearch").on("click", function () {
        search();
    })

    $("#btnRemove").on("click", function () {
        var inputCheckeds = $("#tbody input:checkbox:checked");
        var length = inputCheckeds.length;
        if (length === 0) {
            shortTips("请选择对账记录");
            return false;
        }
        if (length > 1) {
            shortTips("只能选择一个对账记录");
            return false;
        }
        var id = inputCheckeds.val();
        $(window).modalDialog({
            title: "提示",
            content: "确定删除Id为" + id + "的对账记录？",
            type: "confirm",
            okCallBack: function (e, $el) {
                $.ajax({
                    url: "/Reconciliation/Delete?id=" + id,
                    type: "POST",
                    dataType: "json",
                    success: function (result) {
                        if (result.succeeded) {
                            $el.data("bs.modal").hide();
                            setTimeout(function () {
                                shortTips("操作成功");
                            }, 1000)
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