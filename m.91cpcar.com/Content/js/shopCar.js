$(function () {
   
    //从购物车删除
    $(".delCar").click(function () {
        var pid = $(this).attr("pid");
        var attid = $(this).attr("attid");
        var thisDom = $(this);
        var btnArray = ['否', '是'];
        mui.confirm('您确定要移除商品？', '', btnArray, function (e) {
            if (e.index == 1) {            
                shopCar.removeCart(pid, attid);//cookie移除
                thisDom.parents(".car-pro").remove();
                OrderPrice(); //订单总价
            } 
        })
    })
    $(".car-item-num").change(function () {
        var cid = $(this).attr("cid");
        var num = $(this).val();
        if (!Validata.isInteger(num)) {         
            num = 1;
            $(this).val(num);           
        }
        upToCart(cid, num);
       // ItemTotle(cid, num);
        OrderPrice();//订单总价

    })

    function upToCart(id,num) {
        var cart = this.GetCart();

            for (var i = 0; i < cart.List.length; i++) {
                if (cart.List[i].ID == id ) {
                    cart.List[i].Qty = num;
                    break;
                } 
            }
            SetCartCookie(cart);
            GetAllCount();
        

    }

    //初始化订单总金额
    $(".car-item-num").blur(function () {
        var cid = $(this).attr("cid");
        var num = $(this).val();
        setQty(cid, num);
    });
    OrderPrice();


    //提交订单
    $("#SendOrder").click(function () {
        Base.ajaxPost("/Member/IsLogin", "", function (data, error) {
            if (data.r) {
                //登录了                
                var shopList = []
                $(".car-pros .cid").each(function () {
                    var cid = $(this).val();
                    var ID = $("#pid-" + cid).val();
                    var Qty = $("#itemNum-" + cid).val();
                    var Price = $("#itemPrice-" + cid + " .car-item-price-v").html();
                    var Attrs = $("#lastAttr-" + cid).val();
                    var Type = $("#type-"+cid).val();
                    var shop = {
                        ID: ID,
                        Qty: Qty,
                        Price: Price,
                        Attrs: Attrs,
                        Type:Type
                    }
                    shopList.push(shop);//商品列表                    
                })

                if (shopList.length > 0) {
                    var shopTotlePrice = $("#orderPrice").html();//商品总价
                    var sendInfo = {
                        Name: $("#sendPeople").val(),
                        Prov: $("#cmbProvince").val(),
                        City: $("#cmbCity").val(),
                        Area: $("#cmbArea").val(),
                        Address: $("#Address").val(),
                        Phone: $("#Phone").val(),
                        Beizhu: $("#memo").val()
                    }
                    var orderInfo = {
                        shopList: shopList,
                        shopTotlePrice: shopTotlePrice,
                        sendInfo: sendInfo
                    }
                    if (ValidateOrderInfo(orderInfo))
                    {

                        Base.ajaxPost("/Order/AddOrder/", { orderInfo: JSON.stringify(orderInfo) }, function (data, error) {
                            if (data.status == 'ok') {
                                shopCar.ClearCart();
                                location.href = "/Member/MyOrder";
                            } else
                            {
                                mui.alert(data.msg,"警告");
                            }
                            //if (data.status == 'catch') {
                            //    //系统异常稍后再试
                            //}
                            //if (data.status == "error") {
                            //    //错误
                            //    data.msg;
                            //}

                        })
                     
                    }
                } else {
                    alert("没有商品！");
                    window.location.href = "/Product/Index";
                }
            } else {
                //没有登录
                window.location.href = "/Member/Login";
                
            }

        })

    }) 
})
//单个产品小计
function ItemTotle(cid, num)
{
    var p = $("#itemPrice-" + cid + " .car-item-price-v").html();//toFixed
    $("#itemTotlePrice-" + cid).html((p * num).toFixed(2));
    OrderPrice();//订单总金额
}
//购物车总金额
function OrderPrice() {
    var totle = 0;
    $(".car-pro").each(function () {
        //var p = $(this).children(".car-pro-info").children(".car-item-price").children(".car-item-price-v").html();
        var p = $(this).children("a").children(".car-pro-info").children(".car-item-price").children(".car-item-price-v").html();
        var n = $(this).children(".car-pro-op").children("div").children(".car-item-num").val();
        var t = parseFloat(p) * parseFloat(n);
        totle += t;
    })
    $("#totlePrice").html(totle.toFixed(2));
    //txtNum
    var totleNum = 0;
    $(".car-item-num").each(function () {
        var itemQty = $(this).val();
        var p;
        if (itemQty) {
            p = parseInt($(this).val());
        } else {
            p = 0;
        }
        totleNum += p;
    })
    $("#totleNum").html(totleNum);
    //$("#hcarnum").html(totleNum);
    if (totleNum == 0) {
        var sm = "<div style='height:300px; text-align:center; padding-top:100px;'><h4>购物车还没有产品，<a href='/Home/index'>去选购</a></h4></div>";
        $(".car-pros").append(sm); 
          }
}
function ValidateOrderInfo(oderInfo)
{
    var sendInfo = oderInfo.sendInfo;
   
    if (sendInfo.Name.length == 0 )
    {
        alert("收货人不能为空");
        return false;
    }
    if (sendInfo.Name.length > 50)
    {
        alert("收货人姓名太长");
        return false;    
    }
    if (sendInfo.Prov.length == 0)
    {
        alert("省份不能为空");
        return false;
    }
    if (sendInfo.City.length == 0)
    {
        alert("城市不能为空");
        return false;
    }
    if (sendInfo.Area.length == 0)
    {
        alert("区域不能为空");
        return false;
    }
    if (sendInfo.Address.length == 0)
    {
        alert("配送地址不能为空");
        return false;
    }
    if (sendInfo.Phone.length == 0)
    {
        alert("手机号码不能为空");
        return false;
    }
    if (!Validata.isPhone(sendInfo.Phone))
    {
        alert("手机号码格式错误");
        return false;
    }
   
    if(oderInfo.shopList.length==0)
    {
        alert("没有产品！");
        return false;
    }
    if (!Validata.isNumber(oderInfo.shopTotlePrice) || parseFloat(oderInfo.shopTotlePrice) <= 0)
    {
        alert("订单金额错误！");
        return false;
    }
    return true;
  

}
function setQty(cid, num) {
    var shop = {
        ID: $("#pid-" + cid).val(),
        Price: $("#itemPrice-" + cid + " .car-item-price-v").html(),
        Attrs: $("#patt-" + cid).val()
    }
    shopCar.SetCarQty(shop, num);
}
