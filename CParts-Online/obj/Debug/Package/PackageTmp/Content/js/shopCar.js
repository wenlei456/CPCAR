$(function () {
    //增减数量
    $(".maxNum").click(function () {
        var num = parseInt($(this).siblings(".txtNum").val());
        if (num < 99) {
            num++;
            $(this).siblings(".txtNum").val(num);
            //计算产品总价数量*价格
            var cid = $(this).attr("cid");
            ItemTotle(cid, num);
        }
    })

    $(".minNum").click(function () {
        var num = parseInt($(this).siblings(".txtNum").val());
        if (num > 1) {
            num--;
            $(this).siblings(".txtNum").val(num);
            //计算产品总价数量*价格
            var cid = $(this).attr("cid");
            ItemTotle(cid, num);
        }
    })
    //增减数量
    $(".maxNum2").click(function () {
        var num = parseInt($(this).siblings(".txtNum").val());
        if (num < 99) {
            num++;
            $(this).siblings(".txtNum").val(num);
            //计算产品总价数量*价格
            var cid = $(this).attr("cid");
            ItemTotle(cid, num);
            setQty(cid,num);
        }
    })
    $(".minNum2").click(function () {
        var num = parseInt($(this).siblings(".txtNum").val());
        if (num > 1) {
            num--;
            $(this).siblings(".txtNum").val(num);
            //计算产品总价数量*价格
            var cid = $(this).attr("cid");
            ItemTotle(cid, num);
            setQty(cid,num);
        }
    })
    $(".pqty").blur(function () {
        var cid = $(this).attr("cid");
        var num = $(this).val();
        setQty(cid, num);
    });
    //从购物车删除
    $(".delCar").click(function () {
        if (confirm("确定要移除商品？")) {
            var pid = $(this).attr("pid");
            var attid = $(this).attr("attid");
            removeCart(pid, attid);//cookie移除
            $(this).parent().parent().parent().remove();
            OrderPrice(); //订单总价
        }
    })
    $(".txtNum").change(function () {
        var cid = $(this).attr("cid");
        var num = $(this).val();
        if (!isInteger(num)) {
            $(this).popover('show');
            num = 1;
            $(this).val(num);           
        }
        ItemTotle(cid, num);
        OrderPrice();//订单总价

    })
    //初始化订单总金额
    OrderPrice();
    //提交订单
    $("#SendOrder").click(function () {
        Post("/member/IsLogin", "", function (data,error) {
            if (data.r) {
                //登录了                
                var shopList = []
                $("#tb_rows .cid").each(function () {
                    var cid = $(this).val();
                    var ID = $("#pid-" + cid).val();
                    var Qty = $("#itemNum-" + cid).html();
                    var Price = $("#itemPrice-" + cid).html(); 
                    var Attrs = $("#lastAttr-" + cid).val();
                    var Type = $("#type-" + cid).val();
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
                        Phone: $("#Phone").val()
                    }
                    var orderInfo = {
                        shopList: shopList,
                        shopTotlePrice: shopTotlePrice,
                        sendInfo: sendInfo
                    }
                    if (ValidateOrderInfo(orderInfo))
                    {
                        Post("/Order/AddOrder/", { orderInfo: JSON.stringify(orderInfo) }, function (data, error) {

                            if (data.status == 'ok') {
                                ClearCart();
                                location.href = "/Member/MyOrder";
                            }
                            if (data.status == 'catch') {
                                //系统异常稍后再试
                            }
                            if (data.status == "error") {
                                //错误
                              alert(data.msg);
                            }

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
    var p = $("#itemPrice-" + cid).html();//toFixed
    $("#itemTotlePrice-" + cid).html((p * num).toFixed(2));
    OrderPrice();//订单总金额
}
//购物车总金额
function OrderPrice() {
    var totle = 0;
    $(".Carlist .itemTotlePrice").each(function () {
        var p = parseFloat($(this).html());
        totle += p;
    })
    $("#totlePrice").html(totle.toFixed(2));
    //txtNum
    var totleNum = 0;
    $(".Carlist .txtNum").each(function () {
        var itemQty = $(this).val();
        var p;
        if (itemQty) {
            p = parseInt($(this).val());
        } else {
            p = parseInt($(this).html());
        }
        totleNum += p;
    })
    $("#totleNum").html(totleNum);
    $("#hcarnum").html(totleNum);
    if (totleNum == 0) {
        var sm = "<tr><td colspan='5'><div style='height:300px; text-align:center; padding-top:100px;'><h3>购物车还没有产品，<a href='/'>去选购</a></h3></div></td></tr>";
        $("#tb_rows").append(sm); 
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
    if (!phoneRegex(sendInfo.Phone))
    {
        alert("手机号码格式错误");
        return false;
    }
   
    if(oderInfo.shopList.length==0)
    {
        alert("没有产品！");
        return false;
    }
    if(!isNumber(oderInfo.shopTotlePrice) || oderInfo.shopTotlePrice<=0)
    {
        alert("订单金额错误！");
        return false;
    }
    return true;
  

}
function setQty(cid,num)
{
    var shop = {
        ID: $("#pid-" + cid).val(),
        Price: $("#itemPrice-" + cid).html(),
        Attrs: $("#patt-" + cid).val()
    }
    SetCarQty(shop, num);
}