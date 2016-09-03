var $wrap = $(".attachment"),
	$form = $("#OrderBooking"),
	addAttachmentUrl = "/Order/AddAttachment",
	removeAttachmentUrl = "/Order/RemoveAttachment",
	identityType = $("#identityType").val(),
	listUrl = identityType === "Employee" ? "/order/list" : "/customer/order/list",
	orderId = $("#Id").val(),
	isAdd = orderId === "",
    $deliveryDate= $("input[name=DeliveryDate]");

$(function () {
    if ($deliveryDate.length > 0) {
        $deliveryDate.datetimepicker({
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
        formData: {},
        singleFileUploads: false,
        limitMultiFileUploads: 1,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
        send: function (e, data) {
            $wrap.loading();
        },
        fail: function () {
            $wrap.data("loading").hide();
        },
        add: function (e, data) {
            if (filesData.length >= 4) {
                shortTips("最多只能上传4张图片");
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
            if ($("#ProductCategoryId").val() <= 0) {
                shortTips("请选择品类");
                return false;
            }
            if ($("#ColorForm").val() <= 0) {
                shortTips("请选择成色");
                return false;
            }
            if ($("#GemCategoryId").val() <= 0) {
                shortTips("请选择宝石类型");
                return false;
            }
        },
        success: function (result) {
            if (result.succeeded) {
                shortTips("保存成功");
                location.href = listUrl;
            } else {
                shortTips(errorMessage(result));
            }
        }
    });

	$("#saveOrder").click(function() {
		if (filesData.length < 2) {
			shortTips("请上传两张款式图片");
			return false;
		}
		var attachmentIds = "";
		for (var i = 0; i < filesData.length; i++) {
			attachmentIds += filesData[i].id + ",";
		}
		$("#attachmentIds").val(attachmentIds);
		if (!isAdd) {
			$form.attr("target", "_self").attr("action", "/order/edit");
		}
	});

	$("span[name=btnSpan]").click(function() {
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
	});

	if (!isAdd) {
	    var spanText = $("#divProductCategory .btn-primary").html()
	    ShowRangSize(spanText);
	}
})

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