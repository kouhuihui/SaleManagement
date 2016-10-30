
var $wrap = $(".attachment"),
    orderId = $("#OrderId").val(),
  addAttachmentUrl = "/Order/AddAttachment",
  removeAttachmentUrl = "/Order/RemoveAttachment";
$(function () {
    $(".upload-btn").fileupload({
        autoUpload: true,//是否自动上传
        url: addAttachmentUrl,
        dataType: 'json',
        formData: { orderId: orderId },
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
            if ((data.originalFiles.length + $wrap.find('li').length) > 2) {
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
                var data = result.data;
                var str = "";
                str = str + '<li class="thumbnail" data-id="' + data.id + '"><a class="cboxElement" href="javascript:void(0)"><img  src="' + data.url + '" style="display: block;"></a>';
                str = str + '<div class="well gallery-controls"><a onclick="deleteImage(\'' + data.id + '\')" class="gallery-delete btn"><i class="glyphicon glyphicon-remove"></i></a> </div> </li>';
                $wrap.find('ul').append(str);
            } else {
                shortTips(errorMessage(result));
            }
        }
    });

    $("#btnConfirm").on("click", function () {
        location.reload();
    })
})

function deleteImage(id) {
    $(window).modalDialog({
        title: "提示",
        smallTitle: "删除设计图片",
        content: "确定删除该设计图片吗？",
        type: "confirm",
        okCallBack: function (e, $el) {
            $wrap.loading();
            $.ajax({
                url: removeAttachmentUrl,
                type: "POST",
                dataType: "json",
                data: {
                    fileId: id,
                    orderId: orderId
                },
                success: function (result) {
                    $wrap.data("loading").hide();
                    if (result.succeeded) {
                        $el.data("bs.modal").hide();
                        $("li[data-id=" + id + "]").remove();
                    } else {
                        shortTips(errorMessage(result));
                    }
                }
            });
        }
    });
}
