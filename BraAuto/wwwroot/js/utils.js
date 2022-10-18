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

'use strict';

(function ($) {

    /*------------------
        Preloader
    --------------------*/
    $(window).on('load', function () {
        $(".loader").fadeOut();
        $("#preloder").delay(200).fadeOut("slow");

        /*------------------
            Car filter
        --------------------*/
        $('.filter-controls li').on('click', function () {
            $('.filter-controls li').removeClass('active');
            $(this).addClass('active');
        });

        if ($('.car-filter').length > 0) {
            var containerEl = document.querySelector('.car-filter');
            var mixer = mixitup(containerEl, {
                controls: {
                    toggleDefault: "all"
                }
            });

            mixer.toggleOn('.most-viewed')
                .then(function () {
                    // Deactivate all active toggles

                    return mixer.toggleOff('.newest')
                });
        }
    });

    /*------------------
        Background Set
    --------------------*/
    $('.set-bg').each(function () {
        var bg = $(this).data('setbg');
        $(this).css('background-image', 'url(' + bg + ')');
    });

    //Canvas Menu
    $(".canvas-open").on('click', function () {
        $(".offcanvas-menu-wrapper").addClass("active");
        $(".offcanvas-menu-overlay").addClass("active");
    });

    $(".offcanvas-menu-overlay").on('click', function () {
        $(".offcanvas-menu-wrapper").removeClass("active");
        $(".offcanvas-menu-overlay").removeClass("active");
    });

    //Search Switch
    $('.extras-switch').on('click', function () {
        $('.extras-model').fadeIn(400);
    });

    $('.extras-close-switch').on('click', function () {
        $('.extras-model').fadeOut(400, function () {
        });
    });

    /*------------------
        Navigation
    --------------------*/
    $(".header-menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    /*--------------------------
        Testimonial Slider
    ----------------------------*/
    $(".car-item-pic-slider").owlCarousel({
        loop: false,
        margin: 0,
        items: 1,
        dots: true,
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: false
    });

    /*--------------------------
        Testimonial Slider
    ----------------------------*/
    var testimonialSlider = $(".testimonial-slider");
    testimonialSlider.owlCarousel({
        loop: true,
        margin: 0,
        items: 2,
        dots: true,
        nav: true,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: false,
        responsive: {
            768: {
                items: 2
            },
            0: {
                items: 1
            }
        }
    });

    /*-----------------------------
        Car thumb Slider
    -------------------------------*/
    $(".car-thumb-slider").owlCarousel({
        loop: false,
        margin: 25,
        items: 5,
        smartSpeed: 1200,
        autoHeight: true,
        autoplay: false,
        mouseDrag: false,
        dotsEach: true,
        responsive: {

            768: {
                items: 5
            },
            320: {
                items: 3
            },
            0: {
                items: 2
            }
        }
    });

    /*-----------------------
        Range Slider
    ------------------------ */
    var rangeSlider = $(".price-range");
    var max = 100000;

    rangeSlider.slider({
        range: true,
        min: 1000,
        max: max,
        values: [10000, 80000],
        slide: function (event, ui) {
            $("#amount").val("$" + ui.values[0] + " lv - " + ui.values[1] + (ui.values[1] == max ? '+' : '') + " lv");
            $("#PriceFrom").val(ui.values[0]);
            $("#PriceTo").val(ui.values[1]);
            if (ui.values[1] == max) { $("#PriceTo").val(""); }
        }
    });
    $("#amount").val($(".price-range").slider("values", 0) + " lv - " + $(".price-range").slider("values", 1) + " lv");

    /*------------------
        Magnific
    --------------------*/
    $('.video-popup').magnificPopup({
        type: 'iframe'
    });

    /*------------------
        Single Product
    --------------------*/
    $('.car-thumbs-track .ct').on('click', function () {
        $('.car-thumbs-track .ct').removeClass('active');
        var photoUrl = $(this).data('photo-big-url');
        var bigPhoto = $('.car-big-photo').attr('src');
        if (photoUrl != bigPhoto) {
            $('.car-big-photo').attr({
                src: photoUrl
            });
        }
    });

    /*------------------
        Counter Up
    --------------------*/
    $('.counter-num').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });

})(jQuery);