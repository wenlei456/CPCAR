
//生成一个时间戳字符串
function randomChar(l) {
    var x = "0123456789qwertyuioplkjhgfdsazxcvbnm";
    var tmp = "";
    var timestamp = new Date().getTime();
    for (var i = 0; i < l; i++) {
        tmp += x.charAt(Math.ceil(Math.random() * 100000000) % x.length);
    }
    return timestamp + tmp;
}


function removeCart(pid,attid)
{   //删除选中
    var cart = GetCart();
    var arr = [];//cartList.push  
    if (cart.List.length > 0)
    {
        for (var i = 0; i < cart.List.length; i++) {
            var curr=cart.List[i];
            if (curr.ID != pid || curr.Attrs != attid)
            {
                arr.push(curr);
            }
        }
        cart.List = arr;
        SetCartCookie(cart);
        GetAllCount();
    }    
}
//购物车数据
function GetCart() {
    var cart = {};
    var ck = $.cookie("ShoppingCartObj");
    if (ck) {
        cart = eval("(" + ck + ")");
    }
    
    return cart;
}
//设置cookies数据
function SetCartCookie(cart) {
    //var cookietime = new Date();
   // cookietime.setTime(cookietime.getTime() + (60 * 60 * 1000));//coockie保存一小时 
    $.cookie("ShoppingCartObj", JSON.stringify(cart), { path: "/", expires: 7 });

    
}
//清除cookies数据
function ClearCart() {
    $.cookie("ShoppingCartObj", '', { path: "/", expires: -1 });  
    GetAllCount();
}
//添加数据到cookies
function AddToCart(obj) {
    var shop = {
        ID: obj.ID,
        Qty: obj.Qty,
        Price: obj.Price,
        Attrs: obj.Attrs,
        Type: isNull(obj.Type)?"-1":obj.Type
    }
    var cart = this.GetCart();
    if (!cart) {
        cart = {};
    }
    if (!cart.List || (cart.List&&cart.List.length == 0)) {
        var cartList = [];
        cartList.push(shop);
        cart.List = cartList;
        SetCartCookie(cart);
    }else{
        var cartList = [];
        for (var i = 0; i < cart.List.length; i++) {
            cartList.push(cart.List[i]);
        }
        for (var i = 0; i < cart.List.length; i++) {
            if (cart.List[i].ID == shop.ID && parseFloat(cart.List[i].Price) == parseFloat(shop.Price) && cart.List[i].Attrs == shop.Attrs) {
                cart.List[i].Qty = parseInt(cart.List[i].Qty) + parseInt(shop.Qty);
                break;
            } else {
                if (i==cart.List.length-1) {
                    cartList.push(shop);
                    break;
                }
            }
        }
        cart.List = cartList;
        SetCartCookie(cart);
        GetAllCount();
    }

}
function GetAllCount()
{
    var cart = this.GetCart();
    if (cart.List) {
        if (cart.List.length > 0) {
            var n = 0;
            for (var i = 0; i < cart.List.length; i++) {
                n += parseInt(cart.List[i].Qty);
            }
            $("#hcarnum").html(n);
        }
    }

}
function SetCarQty(obj, qty)
{
    var shop = {
        ID: obj.ID,
        Qty: obj.Qty,
        Price: obj.Price,
        Attrs: obj.Attrs,
        Type: isNull(obj.Type) ? "-1" : obj.Type
    }
    var cart = this.GetCart();
    if (!cart) {
        cart = {};
    }
    if (!cart.List || (cart.List && cart.List.length == 0)) {
        var cartList = [];
        cartList.push(shop);
        cart.List = cartList;
        SetCartCookie(cart);
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
        SetCartCookie(cart);
        GetAllCount();
    }
}

