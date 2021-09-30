
$('#navbarbutton').click(function(){
    $('.search-box').toggleClass('active');
    
});
$('.owl-carousel').owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    dots: true,
    autoplay: true,
    smartSpeed:500,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 1
        }
    }
})