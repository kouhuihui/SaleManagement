$(function () {
    var $customerCombobox = $("#customerCombobox");
    var $customerInput = $customerCombobox.find("input");
    var $customerUl = $customerCombobox.find("ul");
    var $customerli = $customerCombobox.find("ul>li");
    var customerstore = [];
    var originCustomerStore = [];
    $customerInput.on("click", function () {
        $customerInput.focus();
        $customerInput.select();
        $customerUl.show();
    });
    $customerInput.on("keyup", function () {
        doSearch();
    });
    $customerInput.on("blur", function () {
        //$customerUl.hide();
    });
    $customerUl.delegate("li", "click", function () {
        var $this = $(this);
        var value = $this.attr("value");
        if (value === "") {
            $customerInput.val("");
        } else {
            $customerInput.val($(this).text());
            $customerInput.data("value", value);
        }
        $customerUl.hide();
    });

    getStore();

    function getStore() {
        $.ajax({
            url: "/crm/customers",
            success: function (rtn) {
                if (rtn.succeeded) {
                    var data = rtn.data;
                    customerstore = data;
                    initSelect();
                    getOriginStore();
                }
            },
            error: function () {

            }
        });
    }

    function getOriginStore() {
        originCustomerStore = customerstore.concat([]);
    }

    function initSelect() {
        var html = '<li value="">所有客户</li>';
        for (var i = 0, len = customerstore.length; i < len; i++) {
            html += '<li value="' + customerstore[i].value
                         + '">' + customerstore[i].text
                         + '</li>';
        }
        $customerUl.html(html);
    }

    function doSearch() {
        var arr = [];
        for (var i = 0, items = originCustomerStore, len = items.length; i < len; i++) {
            if (items[i].text.toLowerCase().trim().indexOf($customerInput.val().toLowerCase().trim()) !== -1) {
                arr.push(items[i]);
            }
        }
        customerstore = arr;
        initSelect();
    }
})