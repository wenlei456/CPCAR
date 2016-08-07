
$(function () {
    $(".btn-div").hover(function () {
        $(this).animate({ padding: "0" }, 150);
    }, function () { $(this).animate({ padding: "2" }, 100); })
    //换验证码
    $("#changeVerCode").click(function () {
        $(this).attr("src", "/ValidateCode.ashx?" + getnow());

    })
    //车型搜索
    $("#btnSearchCar").click(function () {
        if($(".buttonList2").is(":visible")==false)
        {
            $(".buttonList2").show();
        }else{
            $(".buttonList2").hide();
        }

    })
    //品牌
    $("#nav_proModel").change(function () {
        var carBrand = $("#nav_proBrand").val();
        var carModel = $(this).val();
        var options; //CarYear
        $("#nav_proYear option").remove();        
        $("#nav_proYear").append(" <option value='-1'>选择出产年份</option>");
        var n = {}, r = []; //n为hash表，r为临时数组
        for (var i = 0; i < CarObjs.length; i++) {
            if (CarObjs[i].Brand == carBrand && CarObjs[i].Model == carModel) {
                if (!n[CarObjs[i].ProYear]) //如果hash表中没有当前项
                {
                    n[CarObjs[i].ProYear] = true; //存入hash表   
                    options = "<option value='" + CarObjs[i].ProYear + "'>" + CarObjs[i].ProYear + "年</option>";
                    $("#nav_proYear").append(options);//Model
                }
            }
        }     
        //window.location.href = "/product/SearchProduct?keyword=" + encodeURI(kw);
        //selectCarType("-1");
    })
    //车型搜索
    $("#nav_proBrand").change(function () {
        var carBrand = $(this).val();
        var options; //CarYear
        $("#nav_proModel option").remove();
        $("#nav_proModel").append("<option value='-1'>选择汽车型号</option>");//Model
        var n = {}, r = []; //n为hash表，r为临时数组
        for (var i = 0; i < CarObjs.length; i++) {
            if (CarObjs[i].Brand == carBrand)
            {
                if (!n[CarObjs[i].Model]) //如果hash表中没有当前项
                {
                    n[CarObjs[i].Model] = true; //存入hash表                 
                    options = "<option value='" + CarObjs[i].Model + "'>" + CarObjs[i].Model + "</option>";
                    $("#nav_proModel").append(options);//Model
                }
            }
        }       
        proModelChange();
       // selectCarType("-1");
    })
    $("#nav_proYear").change(function () {      
        selectCarType("-1");
    })
   // $("#nav_proBrand").change();
    //$("#nav_proModel").change();

    //点击btn2
    $(".btnlist2").click(function () {     
        var tid = $(this).attr("tid");
        selectCarType(tid);
    })

    //我的订单切换订单状态
    $(".orderTabs li").click(function () {
        $(".orderTabs li").removeClass("active");
        $(this).addClass("active");
        $(".tab-content").addClass("tab-hide");
        $($(this).attr("tab")).removeClass("tab-hide");

    })
  
    /***
    //母版页 初始化
    **/    
    GetAllCount();//初始购物车数量
    LoginAre(); //登录区域
 
   InitCarSelect();//初始化汽车类型

})
//登录区域
function LoginAre() {
    Post("/member/IsLogin", "", function (data, error) {
        if (data.r) {
            $("#nologinAre").hide();
            $("#loginAre").show();
        } else {
            $("#nologinAre").show();
            $("#loginAre").hide();
        }

    })
}
//初始化汽车选择
function InitCarSelect() {
    //t=1&&b=大众&&m=Golf%20GTI&&y=2013
    var t = getQueryString("t");
    var b = getQueryString("b");//品牌
    if (b) {
        if (b !== "-1") {
            var m = getQueryString("m");//型号
            var y = getQueryString("y");//年份
            $("#nav_proBrand").val(b);
            proBrandChange();
            $("#nav_proModel").val(m);
            proModelChange();
            $("#nav_proYear").val(y);
        } else {
            //Product/TypeListByCar?t=-1&&b=-1&&m=null&&y=null
            if (location.href.indexOf("Product/TypeListByCar?") > 0) {
                location.href = "/product/index";
            } else {
                $("#nav_proYear").val("-1");
                $("#nav_proModel").val("-1");
            }
        }
        //$(".prohref").each(function () {
        //    $(this).attr("href", $(this).attr("href") + "?t=" + t + "&&b=" + b + "&&m=" + m + "&&y=" + y)
        //})     
       
    } else {
        $("#nav_proBrand ").val($("#nav_proBrand option:first").val());
         proBrandChange();
         proModelChange();
    }

    ///Product/TypeListByCar?t=1&&b=大众&&m=Golf%20GTI&&y=2014
}
//选择型号
function selectCarType(id) {
    var tid = id;
    var carBrand = $("#nav_proBrand").val();
    var carModel = $("#nav_proModel").val();
    var carYear = $("#nav_proYear").val();
    window.location.href = "/Product/TypeListByCar?t=" + tid + "&&b=" + carBrand + "&&m=" + carModel + "&&y=" + carYear;
}
function proModelChange() {
    try{
        if (CarObjs) {
            var carModel = $("#nav_proModel").val();
            var b = $("#nav_proBrand").val();
            if (carModel !== "-1" && b !== "-1") {
                var options2; //CarYear
                $("#nav_proYear option").remove();
                var n = {}, r = []; //n为hash表，r为临时数组
                for (var i = 0; i < CarObjs.length; i++) {
                    if (CarObjs[i].Brand == b && CarObjs[i].Model == carModel) {
                        if (!n[CarObjs[i].ProYear]) //如果hash表中没有当前项
                        {
                            n[CarObjs[i].ProYear] = true; //存入hash表   
                            options2 = "<option value='" + CarObjs[i].ProYear + "'>" + CarObjs[i].ProYear + "年</option>";
                            $("#nav_proYear").append(options2);//Model
                        }
                    }
                }
            } else {
                //Product/TypeListByCar?t=-1&&b=-1&&m=null&&y=null
                if (location.href.indexOf("Product/TypeListByCar") > 0) {
                    location.href = "/product/index";
                } else {
                    $("#nav_proYear").val("-1");                  
                }
             
            }
        }
    }catch(exc){}
}
function proBrandChange() {
    try{
    if (CarObjs) {
        var b = $("#nav_proBrand").val();
        if (b !== "-1") {
            var options; //CarYear
            $("#nav_proModel option").remove();
            var n = {}, r = []; //n为hash表，r为临时数组
            for (var i = 0; i < CarObjs.length; i++) {
                if (CarObjs[i].Brand == b) {
                    if (!n[CarObjs[i].Model]) //如果hash表中没有当前项
                    {
                        n[CarObjs[i].Model] = true; //存入hash表                 
                        options = "<option value='" + CarObjs[i].Model + "'>" + CarObjs[i].Model + "</option>";
                        $("#nav_proModel").append(options);//Model
                    }
                }
            }
        } else {
            $("#nav_proModel").val("-1");
        }

    }
    } catch (exc) { }
}
