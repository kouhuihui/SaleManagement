$(function () {
    var addAttachmentUrl = "/SpotGoods/AddAttachment",
        removeAttachmentUrl = "/SpotGoods/RemoveAttachment",
        spotGoodsId = $("#Id").val(),
        isAdd = spotGoodsId === "",
        $form = $("#SpotGoodsForm"),
        $wrap = $(".attachment");

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
        formData: { SpotGoodsId: spotGoodsId },
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
            if ((data.originalFiles.length + filesData.length) > 7) {
                shortTips("最多只能上传7张图片");
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
            $form.loading();
        },
        success: function (result) {
            if (result.succeeded) {
                var timeOut = 1500;
                shortTips("保存成功");
                setTimeout(function () {
                    location.href = "spotGoods/list";
                }, timeOut);
            } else {
                shortTips(errorMessage(result));
                $form.data("loading").hide();
            }
        }
    });

    $("#saveSpotGoods").click(function () {
        if (filesData.length < 1) {
            shortTips("请上传款式图片");
            return false;
        }
        var attachmentIds = "";
        for (var i = 0; i < filesData.length; i++) {
            attachmentIds += filesData[i].id + ",";
        }
        $("#attachmentIds").val(attachmentIds);
        if (!isAdd) {
            $form.attr("target", "_self").attr("action", "/spotGoods/edit");
        }
    });
    $("span[name=btnSpan]").click(function () {
        var $spanBtn = $(this);
        $spanBtn.siblings().removeClass("btn-primary");
        $spanBtn.addClass("btn-primary");
        $spanBtn.siblings("input").val($spanBtn.attr("data-value"));
    });
})