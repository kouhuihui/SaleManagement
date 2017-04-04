$(function () {
    var spotGoodsId = $("#Id").val(),
        isAdd = spotGoodsId === "",
        $form = $("#SpotGoodsForm"),
        $wrap = $(".attachment");
 
    var ViewModel = function() {
        var self = this;  
        self.setStonInfos = ko.observableArray(stoneData),
        self.patterns = ko.observableArray([]),
        self.addSetStone = function (el) {
            $("#modal").modal({
                remote: "/SpotGoods/AddSetStone",
            }).on("hidden.bs.modal", function () {
                var $matchStoneNumber = $("#matchStoneNumber");
                var $matchStoneWeight = $("#matchStoneWeight");
                var $matchStoneId = $("#matchStoneId");
                var stoneInfo = {
                    "matchStoneName": $matchStoneId.find("option:selected").text(),
                    "matchStoneId": $matchStoneId.val(),
                    "number": $matchStoneNumber.val(),
                    "weight": $matchStoneWeight.val(),
                    "spotGoodsId": spotGoodsId,
                    "id": "0"
                };
                if (spotGoodsId !== "") {
                    $.ajax({
                        url: "/SpotGoods/AddSetStone",
                        type: "post",
                        data: { spotGoodsSetStoneInfoViewModel: stoneInfo},
                        success: function (rtn) {
                            if (rtn.succeeded) {
                                self.setStonInfos.push(stoneInfo);
                            }
                        },
                        error: function () {
                            shortTips(errorMessage(rtn));
                        }
                    });
                }
                else {
                    self.setStonInfos.push(stoneInfo);
                }
                $(this).removeData("bs.modal");
            });
        },
        self.deleteClick = function (item, el) { 
            if (item.id !== "") {
                $.ajax({
                    url: "/SpotGoods/DeleteSetStone",
                    type: "post",
                    data:{"id":item.id,"spotGoodsId":spotGoodsId},
                    success: function (rtn) {
                        if (rtn.succeeded) {
                            self.setStonInfos.remove(item);
                        }
                    },
                    error: function () {
                        shortTips(errorMessage(result));
                    }
                });
            }
            else {
                self.setStonInfos.remove(item);
            }
        }
    }
    var soptViewModel = new ViewModel();

    ko.applyBindings(soptViewModel);
    InitPatterns();
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

    function InitPatterns() {
        $.ajax({
            url: "/SpotGoodsPattern/GetSpotGoodsPatterns?type=0",
            success: function (rtn) {
                if (rtn.succeeded) {
                    var data = rtn.data;
                    soptViewModel.patterns(data);
                    $("#spotGoodsPatternId").val($("#hideSpotGoodsPatternId").val());
                }
            },
            error: function () {
                shortTips(errorMessage(result));
            }
        });
    }
})