var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
    acc[i].onclick = function () {
        this.classList.toggle("active");
        var panel = this.nextElementSibling;
        if (panel.style.maxHeight) {
            panel.style.maxHeight = null;
        } else {
            panel.style.maxHeight = panel.scrollHeight + "px";
        }
    }
}

//$(".priceHeight").click(function () {
//    var ProductId = $(this).parent().children("input[name='product-id']").val();
//        console.log("productID: " + ProductId);
//        var myData = { IdFilm: ProductId }

//        $.ajax({
//            url: "/Order/AddToCart",
//            type: "POST",
//            contentType: "application/json",
//            data: JSON.stringify(myData),
//            success: function(data) {
//                console.log("Vsechno v pohode");
//            }
//        });
//    }
//)

//$(function() {
//    $('#search').on('keyup', function() {
//        var pattern = $(this).val();
//        console.log("pattern: " + pattern);
//        $('.searchable-container .items').hide();
//        $('.searchable-container .items').filter(function() {
//            return $(this).text().match(new RegExp(pattern, 'i'));
//        }).show();
//    });
//});