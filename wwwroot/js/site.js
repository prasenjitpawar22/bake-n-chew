
$(document).ready(function () {
    $('#cartIcon, .cart-badge').on("click", function () {
        $("#sidebar").toggleClass('show');
    })
    // $('.cart-badge').on("click", function () {
    //     $("#sidebar").toggleClass('show');
    // })
    //chevron backward
    $('#exit').on("click", function () {
        $("#sidebar").toggleClass("show");
    });
})
// sidebar 