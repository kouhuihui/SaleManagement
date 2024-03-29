﻿var $wrap = $(".attachment"),
    $form = $("#OrderBooking"),
    addAttachmentUrl = "/Order/AddAttachment",
    removeAttachmentUrl = "/Order/RemoveAttachment",
    identityType = $("#identityType").val(),
    listUrl = identityType === "Employee" ? "/order/list" : "/customer/order/list",
    orderId = $("#Id").val(),
    isAdd = orderId === "",
    $deliveryDate = $("input[name=DeliveryDate]"),
    $minChainLength = $("#MinChainLength"),
    $maxChainLength = $("#MaxChainLength"),
    $handSize = $("#HandSize"),
    $created = $("input[name=Created]"),
    $versionNo = $("#VersionNo");

$(function () {
    var customer = new Customer();
    
    var context = "手工有风险  镶嵌需谨慎"+
                   "<div>◆高风险◆易裂、易伤祖母绿、p级钻、磷灰石、萤石、等多裂或低硬度彩宝及薄如纸的翡翠。3%保价。</div>" +
                   "<div>中风险◆崩边、点坑红蓝宝、碧玺、坦桑、等边棱角易磨损型其它彩宝。1%保价。</div>" +
                   "<div>低风险◆崩角、划痕钻石类及高价值翡翠。0.5%保价。50分以上强制保险，详情询客服。</div>" +
                   "<div>【赔付标准】保价赔付100%，不保价视风险程度至多赔付10倍工费。低于1000元的低价值类彩宝免费保赔偿。</div>";
    $("#showInsureDetail").popover({ 
        html: true,
        placement: 'bottom',
        content: context,
    })
    if ($deliveryDate.length > 0) {
        $deliveryDate.datetimepicker({
            autoclose: true,
            fontAwesome: true,
            format: "yyyy-mm-dd",
            minView: 2,
            initialDate: new Date()
        });
    }
    if ($created.length > 0) {
        $created.datetimepicker({
            autoclose: true,
            fontAwesome: true,
            format: "yyyy-mm-dd",
            minView: 2,
            initialDate: new Date()
        });
    }

    var Attachment = function (data) {
        var self = this;
        self.files = ko.observableArray(data);
        self.addFile = function (item, el) {
            self.files.push(item);
        }
        self.deleteFile = function (item, el) {
            $(window).modalDialog({
                title: "提示",
                smallTitle: "删除款式图片",
                content: "确定删除该款式图片吗？",
                type: "confirm",
                okCallBack: function (e, $el) {
                    $wrap.loading();
                    $.ajax({
                        url: removeAttachmentUrl,
                        type: "POST",
                        dataType: "json",
                        data: {
                            fileId: item.id,
                            orderId: orderId
                        },
                        success: function (result) {
                            $wrap.data("loading").hide();
                            if (result.succeeded) {
                                $el.data("bs.modal").hide();
                                self.files.remove(item);
                            } else {
                                shortTips(errorMessage(result));
                            }
                        }
                    });
                }
            });
        }
    };
    var attachment = new Attachment(filesData);
    ko.applyBindings(attachment);
    $(".upload-btn").fileupload({
        autoUpload: true,//是否自动上传
        url: addAttachmentUrl,
        dataType: 'json',
        formData: { orderId: orderId },
        singleFileUploads: true,
        limitMultiFileUploads: 1,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
        send: function (e, data) {
            $wrap.loading();
        },
        fail: function () {
            $wrap.data("loading").hide();
        },
        add: function (e, data) {
            if ((data.originalFiles.length + filesData.length) > 2) {
                shortTips("最多只能上传2张图片");
                return false;
            }
            data.submit();
        },
        done: function () {
            $wrap.data("loading").hide();
        },
        success: function (result) {//设置文件上传完毕事件的回调函数
            if (result['succeeded']) {
                result.data.acceptVisible = ko.observable(false);
                attachment.addFile(result.data);
            } else {
                shortTips(errorMessage(result));
            }
        }
    });

    $form.ajaxForm({
        beforeSubmit: function () {
            var productCategory = $("#ProductCategoryId").val()
            if (productCategory <= 0) {
                shortTips("请选择品类");
                return false;
            }
            var validChain = validChainLength(productCategory);
            if (!validChain) {
                return false;
            }
            if ($("#ColorFormId").val() <= 0) {
                shortTips("请选择成色");
                return false;
            }
            if ($("#GemCategoryId").val() <= 0) {
                shortTips("请选择宝石类型");
                return false;
            }

            if (!$("#chxInsure").is(":checked")) {
                shortTips("请选择已阅读保价说明");
                return false;
            }
            $form.loading();
        },
        success: function (result) {
            if (result.succeeded) {
                var timeOut = 3000;
                if (isAdd) {
                    var data = result.data;
                    var mes = '恭喜您下单成功<br>' + '您的订单号为：' + data.orderId;
                    mes = mes + "<br>可在跟单栏中根据订单号查询进度";
                    shortTips(mes, timeOut);
                }
                else {
                    shortTips("操作成功");
                    timeOut = 1500;
                };
                setTimeout(function () {
                    location.href = listUrl;
                }, timeOut);
            } else {
                shortTips(errorMessage(result));
                $form.data("loading").hide();
            }
        }
    });

    $("#saveOrder").click(function () {
        if ($versionNo.val() === "") {
            if (filesData.length < 1) {
                shortTips("请上传款式图片");
                return false;
            }
            var attachmentIds = "";
            for (var i = 0; i < filesData.length; i++) {
                attachmentIds += filesData[i].id + ",";
            }
            $("#attachmentIds").val(attachmentIds);
        }
        if (!isAdd) {
            $form.attr("target", "_self").attr("action", "/order/edit");
        }
    });

    $("span[name=btnSpan]").click(function () {
        var $spanBtn = $(this);
        $spanBtn.siblings().removeClass("btn-primary");
        $spanBtn.addClass("btn-primary");
        $spanBtn.siblings("input").val($spanBtn.attr("data-value"));
    });
    $("#divProductCategory span").click(function () {
        var $spanBtn = $(this);
        var spanText = $spanBtn.html();
        ShowRangSize(spanText);
        $spanBtn.siblings().removeClass("btn-primary");
        $spanBtn.addClass("btn-primary");
        $spanBtn.siblings("input").val($spanBtn.attr("data-value"));

        $minChainLength.val(0);
        $maxChainLength.val(0);
        $handSize.val(0);
    });
    $("#divIsInsure span").click(function () {
        var $spanBtn = $(this);
        $divInsurance = $("#divInsurance");
        var spanText = $spanBtn.html();
        if (spanText === "保价") {
            $divInsurance.show();
        } else {
            $divInsurance.hide();
            $divInsurance.find("input").val("0");
        }
    })

    if (!isAdd) {
        var spanText = $("#divProductCategory .btn-primary").html()
        ShowRangSize(spanText);
    }

    if ($versionNo.val() !== "") {
        $("#divUpload").hide();
        $("#divCategory").hide();
        $("#divGemCategory").hide();
        $("#divNumber").hide();
        $("#divMainStone").hide();
        $("#divCertificate").hide();
        $("#divWordsPrinted").hide();
        $("#divHasOldMaterial").hide();

        ShowRangSize($("#divProductCategory .btn-primary").html());
    } else {
        $("#divVersion").hide();
    }
})

function validChainLength(productCategoryId) {
    if (productCategoryId == 2) {
        return true;
    }

    if (!$handSize.is(":hidden") && $handSize.val() <= "0") {
        shortTips("请输入手寸");
        return false;
    }
    if (!$minChainLength.is(":hidden") && $minChainLength.val() <= "0") {
        shortTips("请输入链长");
        return false;
    }
    if (!$maxChainLength.is(":hidden") && $maxChainLength.val() <= "0") {
        shortTips("请输入链长");
        return false;
    }
    return true;
}

function ShowRangSize(spanText) {
    var $divHandSize = $("#divHandSize"),
    $divChain = $("#divChain");
    if (spanText == "女戒" || spanText == "男戒" || spanText == "手镯") {
        $divHandSize.show();
        $divChain.hide();
    } else if (spanText == "吊坠" || spanText == "手链") {
        $divHandSize.hide();
        $divChain.show()
    }
    else {
        $divHandSize.hide();
        $divChain.hide()
    }
}
