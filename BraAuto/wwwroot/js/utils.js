var braAuto = braAuto || {};

braAuto.submitJson = function (options) {
    var defaults = {
        //url: undefined,
        //data: undefined,
        method: 'POST',
        submit: true
    };

    $.extend(true, defaults, options);

    if (defaults.url === undefined) { throw new Error("Missing param: url"); }

    var form = jQuery("<form>")
        .attr("method", defaults.method)
        .attr("accept-charset", "UTF-8")
        .attr("action", defaults.url);

    if (defaults.target !== undefined) { form.attr("target", defaults.target); }

    var data = serializeObject(defaults.data);

    for (var i = 0; i < data.length; i++) {
        jQuery("<input type='hidden'>")
            .attr("name", data[i].name)
            .attr("value", data[i].value)
            .appendTo(form);
    }

    if (defaults.submit == true) {
        form.appendTo("body");
        form.submit();
        form.remove();
    }

    function serializeObject(object) {
        var results = [];
        var plainObjectsIndex = -1;

        jQuery.each(object, function (name, value) {

            if (jQuery.isPlainObject(value)) {
                var items = serializeObject(value);
                var key = null;

                for (var i = 0; i < items.length; i++) {
                    var item = items[i];

                    if (key != item.name) {
                        key = item.name;
                        plainObjectsIndex++;

                        results.push({ name: name + '[' + plainObjectsIndex + '].Key', value: item.name });
                    }

                    results.push({ name: name + '[' + plainObjectsIndex + '].Value', value: item.value });
                }
            } else {
                var values = !jQuery.isArray(value) ? [value] : value;

                for (var i = 0; i < values.length; i++) {
                    // results.push({ name: name, value: values[i] });
                    var val = values[i];

                    if (jQuery.isPlainObject(val)) {
                        var items = serializeObject(val);

                        for (var j = 0; j < items.length; j++) {
                            var item = items[j];

                            results.push({ name: name + '[' + i + '].' + item.name, value: item.value });
                        }
                    } else {
                        results.push({ name: name, value: val });
                    }
                }
            }
        });

        return results;
    };
};

braAuto.confirmDialog = function (title, message, callback) {
    var $modal = $('<div class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" >\
                        <div class="modal-dialog" role="document" > \
                            <div class="modal-content"> \
                                \
                                <div class="modal-header"> \
                                    <h5 class="modal-title">Modal title</h5> \
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button> \
                                </div> \
                                \
                                <div class="modal-body"></div> \
                                \
                                <div class="modal-footer"> \
                                    <button type="button" class="btn btn-ok btn-info" data-bs-dismiss="modal">Ok</button> \
                                    <button type="button" class="btn btn-outline-info" data-bs-dismiss="modal">Cancel</button> \
                                </div> \
                            </div> \
                        </div > \
                    </div >');

    $modal.find('.modal-title').html(title);
    $modal.find('.modal-body').html(message);

    $modal.find('.btn-ok').on('click', function () {
        if (callback !== undefined) { callback(); }
    });

    $modal.on('keyup', function (event) {
        var keyCode = event.which ? event.which : event.keyCode;

        if (keyCode === 13) { $modal.find('.btn-ok').trigger('click'); }
    });

    $modal
        .appendTo('body')
        .on('hidden.bs.modal', function (event) {
            $(this).remove();
        })
        .modal('show');
};

$(document).ready(function () {
    $('.confirm-dialog-trigger').on('click', function (event) {
        var $this = $(this);
        var title = $this.data('dialog-title');
        var message = $this.data('dialog-message');
        var url = $this.attr('href');
        console.log(url)
        braAuto.confirmDialog(title, message, function () {
            console.log("in")
            window.location.href = url;
        });

        return false;
    });
});