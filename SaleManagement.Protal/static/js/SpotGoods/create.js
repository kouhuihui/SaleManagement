$(function () {
    var addAttachmentUrl = "/SpotGoods/AddAttachment",
        removeAttachmentUrl = "/SpotGoods/RemoveAttachment",
        spotGoodsId = $("#Id").val(),
        isAdd = spotGoodsId === "",
        $form = $("#SpotGoodsForm"),
        $wrap = $(".attachment");

    var viewModel = {
        files: ko.observableArray(filesData),
        setStonInfos: ko.observableArray(stoneData),
        addFile: function (item, el) {
            this.files.push(item);
        },
        deleteFile: function (item, el) {
            var _this = this;
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
                            orderId: spotGoodsId
                        },
                        success: function (result) {
                            $wrap.data("loading").hide();
                            if (result.succeeded) {
                                $el.data("bs.modal").hide();
                                _this.files.remove(item);
                            } else {
                                shortTips(errorMessage(result));
                            }
                        }
                    });
                }
            });
        },
        addSetStone: function (el) {
            var _this = this;
            $("#modal").modal({
                remote: "/SpotGoods/AddSetStone",
            }).on("hidden.bs.modal", function () {
                var $matchStoneNumber = $("#matchStoneNumber");
                var $matchStoneWeight = $("#matchStoneWeight");
                var $matchStoneId = $("#matchStoneId");
                if ($matchStoneId.val() !== "") {
                    var stoneInfo = {
                        "mathchStoneName": $matchStoneId.find("option:selected").text(),
                        "matchStoneId": $matchStoneId.val(),
                        "number": $matchStoneNumber.val(),
                        "weight": $matchStoneWeight.val()
                    };
                    _this.setStonInfos.push(stoneInfo);
                }
                $(this).removeData("bs.modal");
            });
        },
        saveClick: function () {

        }
    }
    ko.applyBindings(viewModel);

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
                viewModel.addFile(result.data);
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
                    location.href = "/spotGoods/list";
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
        $("#SetStoneInfos").val(JSON.stringify(stoneData));
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