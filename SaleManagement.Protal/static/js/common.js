/**
    common functions
*/
$.i10n = function (str) { return str; };
$.ajaxSetup({ cache: false }); //禁止全局ajax缓存
var mailReg = /^(\w+([-+.'']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*,?)+$/gi;

function shortTips(text, time) {
	var $wrap = $('<div class="shortTips"></div>');
	if (time === ""||time ===undefined) {
		time = 1500;
	}
	$('<span class="bg img-rounded"></span>').append($.trim(text)).appendTo($wrap);
	$("body").append($wrap);
	setTimeout(function () {
		$wrap.fadeOut("slow", function () {
			$wrap.remove();
		});
	}, time);
}

function loadMore(self, $rowIndexInput, $wrap, url, callback) {
	$(self).next().show();
	$(self).hide();
	$.ajax({
		type: "GET",
		url: url,
		dataType: "html",
		cache: false,
		success: function (html) {
			$(self).show();
			$(self).next().hide();
			var htmlRows = $(html);
			var nextStartRow = parseInt(htmlRows.eq(htmlRows.length - 1).find("input").val());
			if (!isNaN(nextStartRow) && nextStartRow > 0) {
				$rowIndexInput.val(nextStartRow);
			} else {
				$(self).parent().hide();
				$("#pager").show();
			}
			$wrap.append(htmlRows);
			$.isFunction(callback) && callback(html);

		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			$(self).show();
			$(self).next().hide();
			loginTimeout(XMLHttpRequest, textStatus, errorThrown);
		}
	});
}

$.ajaxSetup({
	error: loginTimeout
});
if (!Date.prototype.add) {
	Date.prototype.add = function (obj) { // obj = {year:1,month:-1,day:2};
		var year = this.getFullYear(), month = this.getMonth(), time = 0;
		for (var q in obj) {
			if (q == "year") {
				year += obj[q]
			} else if (q == "month") {
				month += obj[q];
				if (month > 12) {
					year += parseInt(month / 12);
					month -= 12;
				} else if (month < 1) {
					month = 12 + month;
					year += parseInt(month / 12);
				}
			} else if (q == "day") {
				time = obj[q] * 24 * 60 * 60 * 1000;
			}
		}
		return new Date(new Date(year + "/" + (month + 1) + "/" + this.getDate()).getTime() + time);
	}
}
if (!Date.prototype.format) {
	Date.prototype.format = function (format) {
		var o = {
			"M+": this.getMonth() + 1, //month
			"d+": this.getDate(), //day
			"h+": this.getHours(), //hour
			"m+": this.getMinutes(), //minute
			"s+": this.getSeconds(), //second
			"q+": Math.floor((this.getMonth() + 3) / 3), //quarter
			"S": this.getMilliseconds() //millisecond
		}

		if (/(y+)/.test(format)) {
			format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
		}

		for (var k in o) {
			if (new RegExp("(" + k + ")").test(format)) {
				format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
			}
		}

		return format;
	}
}


function loginTimeout(XMLHttpRequest, textStatus, errorThrown) {
	var obj = null;
	try {
		obj = JSON.parse(XMLHttpRequest.responseText);
	} catch (e) {
	}
	if (XMLHttpRequest.responseText == "Unauthorized") {
		setLogin($.i10n("登录超时，请重新登录"));
	}
	else if (XMLHttpRequest.responseText == "LoginInOtherPlace") {
		setLogin($.i10n("帐号已在其它地方登录，当前帐号已下线"));
	}
	else if (obj != null && obj.customError) {
		shortTips(obj.message);
	}
	else {
		shortTips($.i10n("操作失败"));
	}
	$(".bazaLoding").each(function () {
		$(this).parent().data("loading").hide();
	});
}

function magicTime(str) {  // str = "2014/01/12 12:30"
	var date = new Date(str), now = new Date();
	function doubleNumTime(num) {
		num += "";
		return num.length == 1 ? "0" + num : num;
	}
	if (typeof serviceDateOffset == "string") {
		now = new Date(now.getTime() + serviceDateOffset);
	}
	var year = now.getFullYear(),
        month = now.getMonth() + 1,
        day = now.getDate(),
        nowDate = new Date(year + "/" + month + "/" + day),
        hour = now.getHours(),
        minutes = now.getMinutes(),
        yestoday = nowDate.add({ day: -1 }),
        tomorrow = nowDate.add({ day: 1 });

	var maxDate = new Date("2900/01/01").getTime(), miniDate = new Date("2000/01/01").getTime();
	var dateObj = new Date(date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate()),
        dateHours = date.getHours(),
        dateMinu = date.getMinutes(),
        dateTime = date.getTime(),
        nowTime = now.getTime(),
        oneHourTime = 60 * 60 * 1000;
	if (date.toString() == "Invalid Date") {
		return str;
	}
	if (date.getTime() >= maxDate) {
		return $.i10n("很久以后");
	} else if (date.getTime() <= miniDate) {
		return $.i10n("很久以前");
	} else {
		if (yestoday.getTime() == dateObj.getTime()) {
			return $.i10n("昨天") + " " + doubleNumTime(date.getHours()) + ":" + doubleNumTime(date.getMinutes());
		} else if (tomorrow.getTime() == dateObj.getTime()) {
			return $.i10n("明天") + " " + doubleNumTime(date.getHours()) + ":" + doubleNumTime(date.getMinutes());;
		} else if (dateObj.getTime() == nowDate.getTime()) {
			if (dateTime > nowTime - oneHourTime * 4 && dateTime < nowTime - oneHourTime) {  //目标时间小于当前时间，并且不超过4小时
				return (hour - dateHours) + $.i10n("小时前");
			} else if (dateTime < nowTime + oneHourTime * 4 && dateTime > nowTime + oneHourTime) {  //目标时间大于当前时间，并且不超过4小时
				return (dateHours - hour) + $.i10n("小时后");
			} else if (dateHours == hour || (dateTime > nowTime && dateTime < nowTime + oneHourTime) || (dateTime < nowTime && dateTime > nowTime - oneHourTime)) {  // 目标时间与当前时间
				var minutesDiff = Math.floor((dateTime - nowTime) / 60000);
				if (minutesDiff == 0 || minutesDiff == 1) {
					return $.i10n("刚刚");
				} else if (minutesDiff > 0) {
					return minutesDiff + $.i10n("分钟后");
				} else {
					return (minutesDiff * -1) + $.i10n("分钟前");
				}

			} else {
				return $.i10n("今天") + " " + doubleNumTime(dateHours) + ":" + doubleNumTime(dateMinu);
			}
		}
		return str;
	}
}

function autoRendeTime(wrap) {  // must have class "magicTime"
	$(wrap).find(".magicTime").each(function () {
		$(this).html(magicTime($(this).attr("data-time")));
	});
	setInterval(function () {
		$(wrap).find(".magicTime").each(function () {
			$(this).html(magicTime($(this).attr("data-time")));
		});
	}, 60000);
}

function isMobile() {
	if (/AppleWebKit.*Mobile/i.test(navigator.userAgent) || (/MIDP|SymbianOS|NOKIA|SAMSUNG|LG|NEC|TCL|Alcatel|BIRD|DBTEL|Dopod|PHILIPS|HAIER|LENOVO|MOT-|Nokia|SonyEricsson|SIE-|Amoi|ZTE/.test(navigator.userAgent))) {
		return true;
	}
	return false;
}

function createUrl(parameter) {
	var url = location.pathname + (location.search ? location.search : "");
	if (location.search) {
		url = url + "&" + parameter;
	}
	else {
		url = url + "?" + parameter;
	}
	return url;
}

function provinceSelect(p_id, c_id) {
	var $city = $(c_id);
	$(p_id).change(function () {
		if (/^\d+$/.test($(this).val())) {
			$.get("/Data/Area/CN/Provinces/" + $(this).val() + "/Cities", function (result) {
				var str = '';
				if (result.succeeded) {
					$.each(result.data, function (i, v) {
						str += '<option value="' + v.value + '">' + v.text + '</option>';
					});
					$city.html(str);
				}
			});
		} else {
			$city.html('<option value="">请选择</option>');
		}
	});
}

// 使用localStorage记录客户端操作结果，下次访问的时候自动还原，暂时不支持ie7及一下版本
//args:id标示唯一的操作，fn表示需要处理的行为，不接受任何参数，只能保存最后的的结果
function toggleCookie(id, fn) {
	if (!window.localStorage) { return; }
	localStorage[id + "userOpt"] = fn.toString();
	localStorage[id + "url"] = location.href;
	if ($.isFunction(fn)) {
		fn();
	}
}

function errorMessage(result, split) {
	var errorArr = [];
	split = split ? split : ",";
	if (result.message) {
		return result.message;
	} else {
		if (result.data) {
			for (var q in result.data) {
				var curData = result.data[q];
				if ($.isArray(curData) && curData.length > 0) {
					for (var i = 0; i < curData.length; i++) {
						errorArr.push(curData[i]);
					}
				} else if (typeof curData == "string") {
					errorArr.push(curData);
				}
			}
			return errorArr.join(split);
		}
	}
	return "";
}

$.fn.loading = function () {
	var args = arguments;
	this.each(function () {
		if (!$(this).data("loading")) {
			var $loading = $('<div class="bazaLoding"><i class="fa fa-spinner fa-spin"></i></div>');
			$(this).append($loading);
			this.oldPos = $(this).css("position");
			$(this).css("position", "relative").data("loading", $loading);
		} else {
			if (args[0] == "hide") {
				$(this).css("position", this.oldPos).data("loading").hide();
			} else {
				$(this).css("position", "relative").data("loading").show();
				this.oldPos = $(this).css("position");
			}
		}
	});
};

//获取默认头像
function getDefaultAvatar(val) {
	val = $.trim(val);
	return val ? val : "/Static/Images/defaultUser.png";
}

// 延迟执行
function doDelay(fn, time) {
	time = time || 500;
	setTimeout(fn, time);
}

function showGuid(ele, key, path) {
	if (ele.length === 0)
		return;

	var hasLoaded = $.cookie(key);
	if (!hasLoaded) {
		ele.removeClass('hide');
	}
	ele.find('img').click(function () {
		ele.remove();
	});
	$.cookie(key, 'true', {
		expires: 999,
		path: '/'
	});
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function formatDate(date) {
	var d = new Date(date || Date.now()),
    month = '' + (d.getMonth() + 1),
    day = '' + d.getDate(),
    year = d.getFullYear();

	if (month.length < 2) month = '0' + month;
	if (day.length < 2) day = '0' + day;

	return [year, month, day].join('-');
}