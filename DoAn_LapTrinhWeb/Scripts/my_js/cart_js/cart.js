/*
 * Format giỏ hàng: product_{id}={quantity}
 */

// Khởi tạo giỏ hàng khi vào trang giỏ hàng
$(window).ready(function (ev) {
	$("#cartCount").text(Cookie.countWithPrefix("product"));
});

// Button giảm số lượng
$(".btn-dec").click(function (ev) {
	var quanInput = $(ev.currentTarget).next();
	var id = quanInput.data("id");
	var quan = Number(quanInput.val());
	if (quan > 1) {
		quan = quan - 1;
		Cookie.set("product_" + id, quan, 2);
		quanInput.val(quan);
		updateOrderPrice();
	}
});

// Button tăng số lượng
$(".btn-inc").click(function (ev) {
	var quanInput = $(ev.currentTarget).prev();
	var id = quanInput.data("id");
	var quan = Number(quanInput.val());
	quan = quan + 1;
	Cookie.set("product_" + id, quan, 2);
	quanInput.val(quan);
	updateOrderPrice();
});

// Button xóa sản phẩm khỏi giỏ hàng
$(".js-delete").click(function (ev) {
	var id = $(ev.currentTarget).data("id");
	var item = $(ev.currentTarget).closest(".item");
	item.remove();
	Cookie.remove("product_" + id);
	var productCount = Cookie.countWithPrefix("product")
	$("#cartCount").text(productCount);
	$("#lblCartCount").text(productCount == 0 ? "" : productCount);
	updateOrderPrice();
});

function updateOrderPrice() {
	var quanInputs = $("input.qty-input");
	var newTotal = 0;
	quanInputs.each(function (i, e) {
		var price = Number($(e).data('price'));
		var quan = Number($(e).val());
		newTotal += price * quan;
	});
	var eleDiscount = $("#discount");
	var discount = 0;
	if (eleDiscount.attr("data-price")) {
		discount = Number(eleDiscount.attr("data-price"));
	}
	var totalWithFee = newTotal + 30000 - discount;
	totalWithFee += "";
	newTotal += "";
	discount += "";
	var regex = /(\d)(?=(\d{3})+(?!\d))/g;
	$("#totalPrice").text(newTotal.replace(regex, "$1,") +" ₫");
	$("#totalPriceWithFee").text(totalWithFee.replace(regex, "$1,") + " ₫");
	$("#discount").text(discount.replace(regex, "$1,") + " ₫");
};

$(".js-checkout").click(function (ev) {
	ev.preventDefault();
	$.get("/account/userlogged", {},
		function (isLogged, textStatus, jqXHR) {
			if (!isLogged) {
				PopupModalSignIn();
			} else {
				location.href = ev.currentTarget.href;
			}
		},
		"json"
	);
});

$(".btn-submitcoupon").click(function (ev) {
	var input = $(ev.currentTarget).prev();
	var _code = input.val().trim();
	if (_code.length) {
		$.getJSON("/cart/UseDiscountCode", { code: _code },
			function (data, textStatus, jqXHR) {
				if (data.success) {
					$("#discount").attr("data-price", data.discountPrice);
					updateOrderPrice();
					// Hiển thị thông báo
					$.toast({
						heading: 'Thông báo',
						text: 'Mã giảm giá được áp dụng thành công',
						icon: 'success',
						hideAfter: 5000,
						loader: false
					});
				} else {
					$.toast({
						heading: 'Thông báo',
						text: 'Không thể sử dụng mã giảm giá này, vui lòng kiểm tra lại',
						icon: 'error',
						hideAfter: 5000,
						loader: false
					});
				}
			}
		);
	}
})