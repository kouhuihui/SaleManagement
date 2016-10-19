var defaults = {
    width: '150px',
    callback: function (data) {
        return data;
    }
}
var $customerCombobox;
var $vCustomerInput;
var $customerId;
var $customerUl;
var customerstore = [];
var originCustomerStore = [];
var Customer = function (options) {
    $customerCombobox = $("#customerCombobox");
    $vCustomerInput = $customerCombobox.find("#vCustomerId");
    $customerId = $customerCombobox.find("#customerId");
    $customerUl = $customerCombobox.find("ul");
    customerstore = [];
    originCustomerStore = [];
    this.options = $.extend({}, defaults, options);
    this.init();
};

Customer.prototype = {
    init: function () {
        this.getStore();
        this.method();
    },
    getStore: function () {
        var _this = this;
        $.ajax({
            url: "/crm/customers",
            success: function (rtn) {
                if (rtn.succeeded) {
                    var data = rtn.data;
                    customerstore = data;
                    _this.initSelect();
                    _this.getOriginStore();
                }
            },
            error: function () {

            }
        });
    },
    initSelect: function () {
        var html = '<li value="">所有客户</li>';
        for (var i = 0, len = customerstore.length; i < len; i++) {
            html += '<li value="' + customerstore[i].value
				+ '">' + customerstore[i].text
				+ '</li>';
        }
        $customerUl.html(html);
    },
    getOriginStore: function () {
        originCustomerStore = customerstore.concat([]);
    },
    doSearch: function () {
        var arr = [];
        for (var i = 0, items = originCustomerStore, len = items.length; i < len; i++) {
            if (items[i].text.toLowerCase().trim().indexOf($vCustomerInput.val().toLowerCase().trim()) !== -1) {
                arr.push(items[i]);
            }
        }
        customerstore = arr;
        this.initSelect();
    },
    getValue: function () {
        return $customerId.val();
    },
    getText: function () {
        return $vCustomerInput.val();
    },
    method: function () {
        var _this = this;
        $vCustomerInput.bind("click", function () {
            $vCustomerInput.focus();
            $vCustomerInput.select();
            $customerUl.show();
        });
        $vCustomerInput.on("keyup", function () {
            _this.doSearch();
        });
        $customerUl.delegate("li", "click", function () {
            var $this = $(this);
            var value = $this.attr("value");
            if (value === "") {
                $vCustomerInput.val("");
                $customerId.val("");
            } else {
                $vCustomerInput.val($(this).text());
                $customerId.val(value);
            }
            $customerUl.hide();
            _this.options.callback(value);
        });
    }
};