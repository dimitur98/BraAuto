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
        $('.filter__controls li').on('click', function () {
            $('.filter__controls li').removeClass('active');
            $(this).addClass('active');
        });
        if ($('.car-filter').length > 0) {
            var containerEl = document.querySelector('.car-filter');
            var mixer = mixitup(containerEl);
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
    $(".canvas__open").on('click', function () {
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
    $(".header__menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    /*--------------------------
        Testimonial Slider
    ----------------------------*/
    $(".car__item__pic__slider").owlCarousel({
        loop: true,
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
    var testimonialSlider = $(".testimonial__slider");
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
    $(".car__thumb__slider").owlCarousel({
        loop: false,
        margin: 25,
        items: 5,
        smartSpeed: 1200,
        autoHeight: true,
        autoplay: false,
        mouseDrag: false,
        nav: true,
        navElement: "#navEl",
        dots: true,
        dotsEach:true,
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
            $("#amount").val("$" + ui.values[0] + " lv - " + ui.values[1] + (ui.values[1] == max ? '+' : '')+" lv");
        }
    });
    $("#amount").val($(".price-range").slider("values", 0) + " lv - " + $(".price-range").slider("values", 1) + " lv");

    //var carSlider = $(".car-price-range");
    //carSlider.slider({
    //    range: true,
    //    min: 1,
    //    max: 4000,
    //    values: [900, 3000],
    //    slide: function (event, ui) {
    //        $("#caramount").val("$" + ui.values[0] + " - $" + ui.values[1] + ".100");
    //    }
    //});
    //$("#caramount").val("$" + $(".car-price-range").slider("values", 0) + " - $" + $(".car-price-range").slider("values", 1) + ".100");

    //var filterSlider = $(".filter-price-range");
    //var max = 100000;
    //filterSlider.slider({
    //    range: true,
    //    min: 1,
    //    max: max,
    //    values: [20000, 80000],
    //    slide: function (event, ui) {
    //        $("#filterAmount").val("[ " + "$" + ui.values[0] + " - $" + ui.values[1] + (ui.values[1] == max ? '+' : '') + " ]");
    //        $("#PriceFrom").val(ui.values[0]);
    //        $("#PriceTo").val(ui.values[1]);
    //    }
    //});
    //$("#filterAmount").val("[ " + "$" + $(".filter-price-range").slider("values", 0) + " - $" + $(".filter-price-range").slider("values", 1) + " ]");

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
        var imgurl = $(this).data('imgbigurl');
        var bigImg = $('.car-big-img').attr('src');
        if (imgurl != bigImg) {
            $('.car-big-img').attr({
                src: imgurl
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