$(function () {
    var $customerCombobox = $("#customerCombobox");
    var $vCustomerInput = $customerCombobox.find("#vCustomerId");
    var $customerId = $customerCombobox.find("#customerId");
    var $customerUl = $customerCombobox.find("ul");
    var $customerli = $customerCombobox.find("ul>li");
    var customerstore = [];
    var originCustomerStore = [];
    $vCustomerInput.on("click", function () {
        $vCustomerInput.focus();
        $vCustomerInput.select();
        $customerUl.show();
    });
    $vCustomerInput.on("keyup", function () {
        doSearch();
    });
    $vCustomerInput.on("blur", function () {
        //$customerUl.hide();
    });
    $customerUl.delegate("li", "click", function () {
        var $this = $(this);
        var value = $this.attr("value");
        if (value === "") {
            $vCustomerInput.val("");
        } else {
            $vCustomerInput.val($(this).text());
            $customerId.val(value);
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
            if (items[i].text.toLowerCase().trim().indexOf($vCustomerInput.val().toLowerCase().trim()) !== -1) {
                arr.push(items[i]);
            }
        }
        customerstore = arr;
        initSelect();
    }
})