/*
    new hirede style pager.js
*/
!function ($) {
    var pager = function (element, options) {
        var self = this;
        this.element = $(element);
        this.prevEl = '<li><a class="prev" href="javascript:;"><</a></li>';
        this.nextEl = '<li><a class="next" href="javascript:;">></a></li>';
        this.shortEl = '<li><i>. . .</i></li>';
        this.noDataEl = '<li><i>暂无数据</i></li>';
        this.opts = {
            url: "",
            param: {},
            method:"GET",
            curPageArg: "",
            curStartArg: "start",
            start:0,
            takeArg: "take",
            pageSizeArg: "",
            totalRecordArg: "total",
            totalRecord: 0,
            dataInnerWrap:'',  //data包裹的层
            currentPage: 1,
            pageSize: 4,
            staticData:null,
            callback: function () { },
            afterInit: function () { }
        }
        if (typeof options === "object") {
            $.extend(this.opts, options);
        }
        this.element.addClass('custom-page-wrap pagination');
        this.opts.totalPage = Math.ceil(this.opts.totalRecordRecord / this.opts.pageSize);
        this.element.on("click", "a", function () {
            if ($(this).parent().hasClass('active')) {
                return false;
            }
            var curPage = parseInt($(this).html());
            if ($(this).hasClass("prev")) {
                self.prev();
                return;
            }
            if ($(this).hasClass("next")) {
                self.next();
                return;
            }
            if ($.isNumeric(curPage)) {
                self.jump(curPage);
            }
            
        });
        this.isInit = false;
        var self = this;
        if (history.pushState) {
            //window.addEventListener("popstate", function () {
            //    self.isHistory = true;
            //    var historyPage = self._getHistoryPage();
            //    self.jump(historyPage);
            //});
            //var initPage = self._getHistoryPage();
            //self.jump(initPage);
            //return;
        }
        this._loadData();
    }
    pager.prototype._getHistoryPage = function () {
        var historyPage = location.search.substring(1).match(/(^|&)page=(\d*)(&|$)/);
        return historyPage && historyPage[2] || 1;
    }
    pager.prototype._setHistory = function (curPage) {
        var self = this;
        var url = location.href, search = location.search.substring(1);
        curPage = curPage ? curPage : 1;
        function setUrlParam(param, value) {
            var reg = new RegExp('(&|^)(' + param + ')(=)([^&]*)(&|$)', 'g'), split = '&';
            var replaceSearch = search;
            if (reg.test(search)) {
                replaceSearch = replaceSearch.replace(reg, function ($0, $1, $2, $3, $4, $5) {
                    return $1 + param + $3 + value + $5;
                });
                url = url.substring(0, url.indexOf(search)) + replaceSearch;
                search = replaceSearch;
            } else {
                split = search ? '&' : '?';
                url = url + split + param + '=' + value;
            }
/*
            $.each(search.split('&'), function (k, v) {
                if (v.split('=')[0]!='page' && v.split('=')[0] && self.opts.param[v.split('=')[0]] == undefined) {
                    url = url.replace("&"+v, '');
                }
            })
*/
        }
        setUrlParam('page', curPage);
        var optParams = this.opts.param;
        for (var i in optParams) {
            setUrlParam(i, optParams[i]);
        }
        if (history.pushState) {
            //初始化当前页为1时不需要创建历史
            //if (!this.isInit && curPage == 1) {
            //    return;
            //}
            //history.pushState({}, document.title, url);
        }
    }
    pager.prototype._loadData = function (data) {
        var self = this,
            method = this.opts.method.toLocaleLowerCase();
        var staticRender = function (data) {
            var curStart = (self.opts.currentPage -1)  * self.opts.pageSize,
                curEnd = curStart + self.opts.pageSize,
                curStaticData = data.slice(curStart, curEnd);
            self._render(curStaticData);
        };
        if (!this.opts.url) {
            data = data || this.opts.staticData;
            this.opts.totalPage = Math.ceil( data.length / this.opts.pageSize );
            staticRender( data );
            return;
        }
        
        this.opts.start = this.opts.currentPage==0?0:this.opts.currentPage * this.opts.pageSize - this.opts.pageSize;
        var connectors = ~this.opts.url.indexOf('?') ? '&' : '?';
        this.paramStr = '';
        if (this.opts.param) {
            for (var i in this.opts.param) {
                this.paramStr += '&' + i + '=' + this.opts.param[i];
            }
        }
        var url = this.opts.url, postData = {};
        if (method == "get" || method == "delete") {
            url += connectors + this.opts.curStartArg + "=" + this.opts.start + "&" + this.opts.takeArg + "=" + this.opts.pageSize + this.paramStr;
        } else {
            postData = $.extend({}, this.opts.param);
            postData[this.opts.curStartArg] = this.opts.start;
            postData[this.opts.takeArg] = this.opts.pageSize;
        }
        
        $.ajax({
            url: url,
            cache: false,
            data: postData,
            type: this.opts.method,
            success: function (data) {
                var total;
                if (data||data.successed) {
                    data = data['data'] || data;
                    if (self.opts.dataInnerWrap && data[self.opts.dataInnerWrap]) {
                        self.allData = data;
                        data = data.data[self.opts.dataInnerWrap];
                    }
                    self.data = data;
                    total = data[self.opts.totalRecordArg];
                    if (total == undefined) {
                        total = self.opts.totalRecord;
                    }
                } else {
                    total = self.opts.totalRecord;
                }
                self.opts.totalPage = Math.ceil(total / self.opts.pageSize);
                self._render(data);
                if (!self.isInit) {
                    self.opts.afterInit(self);
                    self.isInit = true;
                }
            }
        });
    }

    pager.prototype.prev = function () {
        this.opts.currentPage--;
        if (this.opts.currentPage < 1) {
            this.opts.currentPage = 1;
            return;
        }
        this._loadData();
    }

    pager.prototype.next = function () {
        this.opts.currentPage++;
        if (this.opts.currentPage > this.opts.totalPage) {
            this.opts.currentPage = this.opts.totalPage
            return;
        }
        this._loadData();
    }

    pager.prototype.jump = function (num) {
        num = parseInt(num);
        if (num <= 1) {
            num = 1;
        } else if (num > this.opts.totalPage) {
            num = this.opts.totalPage;
        }
        this.opts.currentPage = num;
        this._loadData();
    }

    pager.prototype._render = function (data) {
        var html = this.prevEl, cur = this.opts.currentPage, total = this.opts.totalPage;
        var self = this;
        function getLi(i) {
            var str = i == cur ? '<li class="active"><a href="javascript:;">' : '<li><a href="javascript:;">';
            return str + i + '</a></li>';
        }
        function renderLimited(startN) {
            var str = "", minLenth = total<=9?total:startN + 7 > total ? total + 1 - startN : 7;
            for (var i = startN; i < minLenth + startN; i++) {
                str += getLi(i);
            }
            return str;
        }
        if (total == 0 || cur > total) {
            html = this.noDataEl;
        } else if(total == 1){
            html = '';
        }else {
            if (total <= 9) { //总页数小于9时不用考虑省略号的
                html += renderLimited(1);
            } else { // 总页数大于9 则判断是左边显示省略号 还是右边或者两边都要显示省略号
                if (cur < 7) { // 总是显示当中7页
                    html += renderLimited(1);
                    html += this.shortEl;
                    html += getLi(total);
                } else {
                    html += getLi(1);
                    html += this.shortEl;
                    html += renderLimited(cur - 3);
                    if (cur + 4 <= total) {
                        html += this.shortEl;
                        html += getLi(total);
                    }
                }
            }
            html += this.nextEl;
        }
        this.element.html(html);
        $.isFunction(this.opts.callback) && this.opts.callback(data, this);
        if (!this.isHistory) {
            //this._setHistory(cur);
        }
    }

    $.fn.pager = function (option) {
        var args = Array.apply(null, arguments);
        args.shift();
        var internal_return;
        this.each(function () {
            var $this = $(this),
                data = $this.data('pager'),
                options = typeof option === 'object' && option;
            if (!data) {
                $this.data('pager', (data = new pager(this, $.extend({}, $.fn.pager.defaults, options))));
            }
            if (typeof option === 'string' && typeof data[option] === 'function') {
                internal_return = data[option].apply(data, args);
                if (internal_return !== undefined) {
                    return false;
                }
            }
        });
        if (internal_return !== undefined)
            return internal_return;
        else
            return this;
    };
    $.fn.pager.defaults = {};
    $.fn.pager.Constructor = pager;
}(window.jQuery);