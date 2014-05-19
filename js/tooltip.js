jQuery(document).ready(function () {
    //Tooltips
    jQuery(".tip_trigger").hover(function () {
        tip = jQuery(this).find('.tip');
        //tip.show();
        tip.stop().slideToggle(200); //Show tooltip
    }, function () {
        //tip.hide();
        tip.stop().slideToggle(200); //Hide tooltip
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coodrinates
        var mousey = e.pageY; //Get Y coordinates
        var tipWidth = tip.width(); //Find width of tooltip
        var tipHeight = tip.height(); //Find height of tooltip
        
        //Distance of element from the right edge of viewport
        var tipVisX = jQuery(window).width() + jQuery(document).scrollLeft() - (mousex + tipWidth);
        //Distance of element from the bottom of viewport
        var tipVisY = jQuery(window).height() + jQuery(document).scrollTop() - (mousey + tipHeight);

        if (tipVisX < 20) { //If tooltip exceeds the X coordinate of viewport
            mousex = e.pageX - tipWidth - tipWidth - 30;
        } if (tipVisY < 50) { //If tooltip exceeds the Y coordinate of viewport
            mousey = e.pageY - tipHeight - 60;
        }
        //Absolute position the tooltip according to mouse position
        tip.css({ top: mousey - 100, left: mousex });
    });
});