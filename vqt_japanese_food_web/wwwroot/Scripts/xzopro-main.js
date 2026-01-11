(function ($) {
    "use strict";

    /*====  Document Ready Function =====*/
    jQuery(document).ready(function($){

        $( ".image-popup-link" ).magnificPopup({
            type:"image",
            removalDelay: 300,
            mainClass: 'mfp-fade'
        });

        // Counter Up
        $(".count-number").counterUp({
            delay: 10,
            time: 2000
        });

        //Search trigger
        $(".search-trigger i").on("click", function(e) {
            $(".header-search-form").addClass("show");
            e.stopPropagation();
        });

        $(".header-search-form  .search-submit").on("click", function(e) {
            $(".header-search-form").addClass("clicked");
            e.stopPropagation();
        });

        $(document).on("click", function(e) {
            if ($(e.target).is(".header-search-form") === false && $(e.target).is(".search-field") === false) {
                $(".header-search-form").removeClass("show");
            }
        });

        //Mobile Menu
        $("#primary-menu").slicknav({
            allowParentLinks: false,
            prependTo: '#mobile-menu-wrap',
            label: ''
        });

        $(".video-play-button").magnificPopup({
            type: 'video'
        });

        //Skill bar
        $(".skillbar").each(function() {
            $(this).appear(function() {
                $(this).find(".progress-fill-bar").animate({
                    width:$(this).attr("data-percent")
                },3000);
            });
        });

        // Skill Counte Up
        $(".skill-percent-count").counterUp({
            delay: 10,
            time: 2500
        });

        //Sticky Menu
        $(".top-fixed-menu").sticky({
            stopSpacing: 0
        });

        //Add Header Background after Scroll
        $(window).on("scroll",function(){
            var pagescroll = $(window).scrollTop();
            if(pagescroll > 100){
                $(".main-menu-area").addClass("scrolled");

            }else{
                $(".main-menu-area").removeClass("scrolled");
            }
        });
    });

    /*====  Window Load Function =====*/
    jQuery(window).on('load', function() {

        // preloader js
        setTimeout(function() {
            $('.site').addClass('loaded');
        }, 500);
    });
}(jQuery));