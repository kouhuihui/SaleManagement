var $wrap = $(".attachment"),
    $form = $("#OrderBooking"),
    addAttachmentUrl = "/HotSelling/AddAttachment",
    removeAttachmentUrl = "/HotSelling/RemoveAttachment",
    hotSellingId = $("#Id").val(),
    isAdd = hotSellingId === "";

$(function () {  

    var Attachment = function (dataFiles,paramFiles) {
        var self = this;
        self.files = ko.observableArray(dataFiles);
        self.paramFiles = ko.observableArray(paramFiles);
        self.addFile = function(item, el) {
            self.files.push(item);
        };
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
                            hotSellingId: hotSellingId
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
        self.addParamFile = function (item, el) {
            self.paramFiles.push(item);
        };
        self.deleteParamFile = function (item, el) {
            $(window).modalDialog({
                title: "提示",
                smallTitle: "删除参考图片",
                content: "确定删除该参考图片吗？",
                type: "confirm",
                okCallBack: function (e, $el) {
                    $wrap.loading();
                    $.ajax({
                        url: removeAttachmentUrl,
                        type: "POST",
                        dataType: "json",
                        data: {
                            fileId: item.id,
                            hotSellingId: hotSellingId
                        },
                        success: function (result) {
                            $wrap.data("loading").hide();
                            if (result.succeeded) {
                                $el.data("bs.modal").hide();
                                self.paramFiles.remove(item);
                            } else {
                                shortTips(errorMessage(result));
                            }
                        }
                    });
                }
            });
        }
    };
    var attachment = new Attachment(filesData,paramFilesData);
    ko.applyBindings(attachment);

    $("#uploadFile").fileupload({
        autoUpload: true,//是否自动上传
        url: addAttachmentUrl,
        dataType: 'json',
        formData: {
            hotSellingId: hotSellingId,
            FilePurpose: 4
        },
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
    $("#uploadParamFile").fileupload({
        autoUpload: true,//是否自动上传
        url: addAttachmentUrl,
        dataType: 'json',
        formData: {
            hotSellingId: hotSellingId,
            FilePurpose: 5
        },
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
            if ((data.originalFiles.length + filesData.length) > 4) {
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
                attachment.addParamFile(result.data);
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
          
            if ($("#ColorFormId").val() <= 0) {
                shortTips("请选择成色");
                return false;
            }
            if ($("#GemCategoryId").val() <= 0) {
                shortTips("请选择宝石类型");
                return false;
            }
            $form.loading();
        },
        success: function (result) {
            if (result.succeeded) {
                var timeOut = 1000;
                shortTips("操作成功"); 
                setTimeout(function () {
                    location.href = "/hotSelling/setting";
                }, timeOut);
            } else {
                shortTips(errorMessage(result));
                $form.data("loading").hide();
            }
        }
    });

    $("#saveOrder").click(function () {
        if (filesData.length < 1) {
            shortTips("请上传款式图片");
            return false;
        }
        var attachmentIds = "";
        for (var i = 0; i < filesData.length; i++) {
            attachmentIds += filesData[i].id + ",";
        }

        for (var i = 0; i < paramFilesData.length; i++) {
            attachmentIds += paramFilesData[i].id + ",";
        }

        $("#attachmentIds").val(attachmentIds);
    });

    $("span[name=btnSpan]").click(function () {
        var $spanBtn = $(this);
        $spanBtn.siblings().removeClass("btn-primary");
        $spanBtn.addClass("btn-primary");
        $spanBtn.siblings("input").val($spanBtn.attr("data-value"));
    });
})

 