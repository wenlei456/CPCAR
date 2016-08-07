
var shopCar = {
    //生成一个时间戳字符串
    randomChar: function (l) {
        var x = "0123456789qwertyuioplkjhgfdsazxcvbnm";
        var tmp = "";
        var timestamp = new Date().getTime();
        for (var i = 0; i < l; i++) {
            tmp += x.charAt(Math.ceil(Math.random() * 100000000) % x.length);
        }
        return timestamp + tmp;
    },
    //移除购物车
    removeCart: function (pid, attid) {   //删除选中
        var cart = shopCar.GetCart();
        var arr = [];//cartList.push  
        if (cart.List.length > 0) {
            for (var i = 0; i < cart.List.length; i++) {
                var curr = cart.List[i];
                if (curr.ID != pid || curr.Attrs != attid) {
                    arr.push(curr);
                }
            }
            cart.List = arr;
            shopCar.SetCartCookie(cart);
            shopCar.GetAllCount();
        }
    },

    //购物车数据
    GetCart: function () {
        var cart = {};
        var ck = $.fn.cookie("ShoppingCartObj");
        if (ck) {
            cart = eval("(" + ck + ")");
        }

        return cart;
    },
    //设置cookies数据
    SetCartCookie: function (cart) {
        //var cookietime = new Date();
        // cookietime.setTime(cookietime.getTime() + (60 * 60 * 1000));//coockie保存一小时 
        $.fn.cookie("ShoppingCartObj", JSON.stringify(cart), { path: "/", expires: 7 });
    },
    //清除cookies数据
    ClearCart: function () {
        $.fn.cookie("ShoppingCartObj", '', { path: "/", expires: -1 });
        shopCar.GetAllCount();
    },
    //添加数据到cookies
    AddToCart: function (obj) {
        var shop = {
            ID: obj.ID,
            Qty: obj.Qty,
            Price: obj.Price,
            Attrs: obj.Attrs,
            Type: Validata.isNull(obj.Type) ? "-1" : obj.Type
        }
        var cart = shopCar.GetCart();
        if (!cart) {
            cart = {};
        }
        if (!cart.List || (cart.List && cart.List.length == 0)) {
            var cartList = [];
            cartList.push(shop);
            cart.List = cartList;
            shopCar.SetCartCookie(cart);
        } else {
            var cartList = [];
            for (var i = 0; i < cart.List.length; i++) {
                cartList.push(cart.List[i]);
            }
            for (var i = 0; i < cart.List.length; i++) {
                if (cart.List[i].ID == shop.ID && cart.List[i].Price == shop.Price && cart.List[i].Attrs == shop.Attrs) {
                    cart.List[i].Qty = parseInt(cart.List[i].Qty) + parseInt(shop.Qty);
                    break;
                } else {
                    if (i == cart.List.length - 1) {
                        cartList.push(shop);
                        break;
                    }
                }
            }
            cart.List = cartList;
            shopCar.SetCartCookie(cart);
            shopCar.GetAllCount();
        }

    },
    //购物车产品数量
    GetAllCount: function () {
        var cart = shopCar.GetCart();
        if (cart.List) {
            if (cart.List.length > 0) {
                var n = 0;
                for (var i = 0; i < cart.List.length; i++) {
                    n += parseInt(cart.List[i].Qty);
                }
                return n;
            } else {
                return 0;
            }
        }
    },

    SetCarQty:function(obj, qty)
{
    var shop = {
        ID: obj.ID,    
        Price: obj.Price,
        Attrs: obj.Attrs    
    }
    var cart = shopCar.GetCart();
    if (!cart) {
        cart = {};
    }
    if (!cart.List || (cart.List && cart.List.length == 0)) {
        var cartList = [];
        cartList.push(shop);
        cart.List = cartList;
        shopCar.SetCartCookie(cart);
    } else {
        var cartList = [];
        for (var i = 0; i < cart.List.length; i++) {
            cartList.push(cart.List[i]);
        }
        for (var i = 0; i < cart.List.length; i++) {
            if (cart.List[i].ID == shop.ID && parseFloat(cart.List[i].Price) ==  parseFloat(shop.Price) && cart.List[i].Attrs == shop.Attrs) {
                cart.List[i].Qty = qty;
                break;
            } else {
                if (i == cart.List.length - 1) {
                    cartList.push(shop);
                    break;
                }
            }
        }
        cart.List = cartList;
        shopCar.SetCartCookie(cart);
        shopCar.GetAllCount();
    }
}

};