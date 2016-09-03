/*dialog/alert/confirm*/
!function ($) {
    "use strict"
    var modalDialog = function (element, options) {
        var self = this;
        $.fn.modal.Constructor.call(this, element, options);
        $(document).off('focusin.modal');
        this.$element.on('click', '.okModalBtn', function (e) {
            if (typeof self.options.okCallBack === 'function') {
                self.options.okCallBack.call(null, e, self.$element);
            }
        });
        if (this.options.drag) {
            this.bindDrag();
        }
    };
    modalDialog.prototype = (function () {
        var F = function () { };
        F.prototype = $.fn.modal.Constructor.prototype;
        return new F();
    })();
    modalDialog.prototype.bindDrag = function () {
        var dragEl = this.$element.find('.modal-header'),
            innerDialog = this.$element.find('.modal-dialog'),
            self = this;
        var dx = 0,
            dy = 0;
        this.dragDown = false;
        dragEl.on('mousedown', function (e) {
            self.dragDown = true;
            dx = e.pageX - innerDialog.position().left;
            dy = e.pageY - innerDialog.position().top;
        });
        $(document).on('mouseup.modalDialog', function () {
            self.dragDown = false;
        });
        $(document).on('mousemove.modalDialog', function (e) {
            var left = e.pageX - dx,
                top = e.pageY - dy;
            if (self.dragDown) {
                if (left < 0) {
                    left = 0;
                } else if (left + innerDialog.outerWidth(true) > self.$element[0].clientWidth) {
                    left = self.$element[0].clientWidth - innerDialog.outerWidth(true);
                }
                if (top < 0) {
                    top = 0;
                } else if (top + innerDialog.outerHeight(true) > self.$element[0].clientHeight) {
                    top = self.$element[0].clientHeight - innerDialog.outerHeight(true);
                }
                innerDialog.css({
                    left: left + 'px',
                    top: top + 'px'
                });
            }
        });
    };
    modalDialog.prototype.getZIndex = (function () {
        var zIndex = 1030;
        return function () {
            return zIndex += 10;
        };
    })();
    modalDialog.prototype._init = function () {
        var options = this.options,
            $ele = this.$element,
            confirmTemplete = '',
            cancelModalBtn = $ele.find('.cancelModalBtn'),
            modalTop = 30;
        $ele.find('.modal-body').css('padding', '20px 20px 0px');
        cancelModalBtn.show();
        if (options.type == 'alert') {
            modalTop = 100;
            options.width = options.width || 300;
            cancelModalBtn.hide();
            $ele.find('.modal-body').css('padding', '40px 40px 25px');
            if (options.okCallBack == null) {
                options.okCallBack = function () {
                    $ele.modal('hide');
                };
            }
        } else if (options.type == 'confirm') {
            modalTop = 100;
            options.width = options.width || 400;
            confirmTemplete = '<div class="alert alert-info fade in" style="border:0 !important;background:#fff;">' +
                                    '<div class="row">' +
                                        '<div class="col-md-1">' +
                                            '<i class="fa fa-info-circle"></i>' +
                                        '</div>' +
                                        '<div class="col-md-9" style="padding-right:0;">' +
                                            '<h4>' + options.smallTitle + '</h4>' +
                                            '<p>' + options.content + '</p>' +
                                        '</div>' +
                                    '</div>' +
                                '</div>';
        }
        var innerDialog = $ele.find('.modal-dialog');
        innerDialog.width(options.width).css({
            'position': 'absolute',
            'left': ($(window).width() - innerDialog.width()) / 2,
            'margin': 0,
            'top': modalTop + 'px'
        });
        $ele.find('.modal-title').html(options.title);
        if (typeof options.content === 'object') {
            $ele.find('.modal-body').append(options.content);
            options.content.show();
        } else {
            $ele.find('.modal-body').html(confirmTemplete || options.content);
        }
        $ele.find('.okModalBtn').text(options.okText);
        cancelModalBtn.text(options.cancelText);
    };
    $.fn.modalDialog = function (option) {
        return this.each(function () {
            var $this = $(this), modalTemplete = '', modalClass = '';
            if (!this || this == window || this == document) {
                if (typeof option.content === 'string') {
                    modalClass = 'modalTempleteStr';
                    modalTemplete = $('.modalTempleteStr');
                } else {
                    modalTemplete = option.content.closest('.modalTemplete');
                }
                if (modalTemplete[0]) {
                    $this = modalTemplete;
                } else {
                    modalTemplete = '<div class="modal fade in modalTemplete ' + modalClass + '" tabindex="-1" role="dialog">' +
                              '<div class="modal-dialog">' +
                                '<div class="modal-content">' +
                                  '<div class="modal-header">' +
                                    '<button type="button" class="close" data-dismiss="modal">' +
                                        '<span aria-hidden="true">&times;</span><span class="sr-only">Close</span>' +
                                    '</button>' +
                                    '<h4 class="modal-title" id="myModalLabel"></h4>' +
                                  '</div>' +
                                  '<div class="modal-body"></div>' +
                                  '<div class="modal-footer">' +
                                    '<button type="button" class="btn btn-primary okModalBtn">确定</button>' +
                                    '<button type="button" class="btn btn-default cancelModalBtn" data-dismiss="modal">关闭</button>' +
                                  '</div>' +
                                '</div>' +
                              '</div>' +
                            '</div>';
                    $this = $(modalTemplete);
                    $this.appendTo($('body'));
                }
            }
            var data = $this.data('modalDialog'),
                options = $.extend({}, $.fn.modalDialog.defaults, $this.data(), typeof option === 'object' && option);
            if (!data) {
                $this.data('modalDialog', (data = new modalDialog($this[0], options)));
            }
            if (typeof option === 'object' || option === undefined) {
                if (typeof option === 'object') {
                    $this.data('modalDialog').options = options;
                    $this.data('modalDialog')._init();
                }
                $this.modal();
                $('.modal-backdrop:last').css('zIndex', data.getZIndex());
                $this.css('zIndex', data.getZIndex());
            }else if (typeof option === 'string') {
                data[option]();
            } else if (options.show) {
                data.show();
            }
        });
    }
    $.fn.modalDialog.defaults = {
        title: '提示',
        smallTitle:'确认',
        content: '',
        width: null,
        type: 'nomal',  //alert 、confirm、nomal
        okText: '确定',
        cancelText: '取消',
        drag:true,
        okCallBack: null
    };
    $.fn.modalDialog.Constructor = modalDialog;
}(window.jQuery)