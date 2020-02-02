'use strict';

// 刪除按鈕提醒
$(document).delegate('a.delete,:button.delete,input[type="submit"].delete', 'click', function () {
    return confirm("是否確定刪除資料？");
});

// 文字去除空白
$(document).delegate('input[type="text"].trim,input[type="email"].trim', 'blur', function () {
    this.value = $.trim(this.value);
});

// 文字轉大寫
$(document).delegate('input[type="text"].toUpperCase,input[type="email"].toUpperCase', 'blur', function () {
    this.value = this.value.toUpperCase();
});

// 文字轉小寫
$(document).delegate('input[type="text"].toLowerCase,input[type="email"].toLowerCase', 'blur', function () {
    this.value = this.value.toLowerCase();
});

// checkbox全選功能，可設定data-check-group來設定群組
$(document).delegate('input[type="checkbox"].checkAll', 'change', function () {
    let group = $(this).attr("data-check-group");
    let $items = this.form ?
        $('input[type="checkbox"].checkItem:visible', this.form) :
        $('input[type="checkbox"].checkItem:visible');

    if (group) {
        $items.filter('[data-check-group="' + group + '"]').prop('checked', this.checked);
    } else {
        $items.prop('checked', this.checked);
    }
});

// radio全選功能，可設定data-check-group來設定群組
$(document).delegate('input[type="radio"].checkAll', 'change', function () {
    let group = $(this).attr("data-check-group");
    let $items = this.form ?
        $('input[type="radio"].checkItem:visible', this.form) :
        $('input[type="radio"].checkItem:visible');

    if (group) {
        $items.filter('[data-check-group="' + group + '"]').prop('checked', this.checked);
    } else {
        $items.prop('checked', this.checked);
    }
});

// 重設輸入欄位的值
$(document).delegate(':button.reset', 'click', function () {
    /**
     * @param {HTMLInputElement} input - <input />
     * @param {string} value - 預設值
     */
    let resetInput = function (input, value) {
        switch (input.type) {
        case 'text':
        case 'email':
            input.value = value;
            break;

        case 'radio':
            input.checked = input.value === value;
            break;

        case 'checkbox':
            let values = value.split(',');
            input.checked = values.indexOf(input.value) > -1;
            break;
        }
    };

    /**
     * @param {HTMLSelectElement} select - <select />
     * @param {string} value - 預設值
     */
    let resetSelect = function (select, value) {
        let $select = $(select);
        $select.find('option').prop('selected', false);

        if (select.multiple) {
            let values = value.split(',');
            for (let index in values) {
                $select.find('option[value="' + values[index] + '"]').prop('selected', true);
            }
        } else {
            $select.find('option[value="' + value + '"]').prop('selected', true);
        }
    };

    /**
     * @param {HTMLTextAreaElement} textArea - <textarea />
     * @param {string} value - 預設值
     */
    let resetTextArea = function (textArea, value) {
        if (typeof CKEDITOR !== 'undefined' && CKEDITOR !== null &&
            typeof CKEDITOR.instances[textArea.id] !== 'undefined' &&
            CKEDITOR.instances[textArea.id] !== null
        ) {
            CKEDITOR.instances[textArea.id].setData(value);
        } else {
            textArea.value = value;
        }
    };

    let group = $(this).attr('data-reset-group');
    let $items = this.form ?
        $(':input[data-reset-value]', this.form) : $(':input[data-reset-value]');

    if (group) {
        $items = $items.filter('[data-reset-group="' + group + '"]');
    }
    $items.each(function () {
        let value = $(this).attr('data-reset-value');

        switch (this.tagName) {
        case 'INPUT':
            resetInput(this, value);
            break;

        case 'SELECT':
            resetSelect(this, value);
            break;

        case 'TEXTAREA':
            resetTextArea(this, value);
            break;
        }
    });
});

// MVC5的AjaxHelper不支援formaction
$(document).delegate('[type="submit"][formaction]', 'click', function () {
    if ($(this.form).data("ajax") === true) {
        this.form.action = this.formAction;
    }
});