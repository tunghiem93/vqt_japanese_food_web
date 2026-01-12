function ValidationEmail(email) {
    var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return regex.test(email)
}
function NumberFormat(number) {
	return new Intl.NumberFormat().format(number);
}
/************** HuyHoang ******************/
var vuongQuanTuan = {
	addToCard: function (item) {
		var Items = localStorage.getItem("cart");
		if (Items != null) {
			var Orders = JSON.parse(Items)
			var hasItem = Orders.some((o) => { return (o.Id == item.Id && o.Size == item.Size && o.ColorCode == item.ColorCode) })
			if (hasItem == false) {
				Orders.push(item)
			}
			else {
				$.each(Orders, function (index, _item) {
					if (_item.Id === item.Id && _item.Size == item.Size && _item.ColorCode == item.ColorCode) {
						_item.Qty = parseInt(_item.Qty) + parseInt(item.Qty)
					}
				});
			}
			localStorage.removeItem("cart")

			var Json = JSON.stringify(Orders)
			localStorage.setItem("cart", Json);
		} else {
			/*** Created new localStorage ***/
			var _Items = []
			_Items.push(item)
			var Json = JSON.stringify(_Items)
			localStorage.setItem("cart", Json);
		}
		/*** UPDATE NUMBER SHOPPING CART ***/
		this.countOrder()
	},
	getListOrder: function () {
		var Items = localStorage.getItem("cart");
		var Orders = null
		if (Items != null) {
			Orders = JSON.parse(Items)
		}
		return Orders
	},
	countOrder: function () {
		var Items = localStorage.getItem("cart");
		var Total = 0
		if (Items != null) {
			var Orders = JSON.parse(Items)
			$.each(Orders, function (index, _item) {
				Total = Total + parseInt(_item.Qty)
			});

		}
		$('.count').text(Total)
	},
	updateQtyOrder: function (id, quantity, color, size) {
		var Items = localStorage.getItem("cart");
		var Total = 0;
		if (Items != null) {
			var Orders = JSON.parse(Items);
			$.each(Orders, function (index, _item) {
				if (_item.Id === id && _item.ColorCode === color && _item.Size === size) {
					_item.Qty = quantity;
					Total = Total + quantity;
				} else {
					Total = Total + parseInt(_item.Qty);
				}

			});
			localStorage.removeItem("cart");
			var Json = JSON.stringify(Orders);
			localStorage.setItem("cart", Json);
		}
		$('.count').text(Total);
	},
	deleteOrder: function (id, color, size) {
		var Items = localStorage.getItem("cart");
		var Total = 0;
		if (Items != null) {
			var Orders = JSON.parse(Items);
			var indexItem = -1;
			$.each(Orders, function (index, _item) {
				if (_item.Id === id && _item.ColorCode === color && _item.Size === size) {
					indexItem = index;
				} else {
					Total = Total + parseInt(_item.Qty);
				}
			});
			if (indexItem !== -1) {
				Orders.splice(indexItem, 1);
			}
			localStorage.removeItem("cart");
			var Json = JSON.stringify(Orders);
			localStorage.setItem("cart", Json);
		}
		$('.count').text(Total);
	},
	detroyOder: function () {
		localStorage.removeItem("cart");
		$('.count').text(0);
    }
}