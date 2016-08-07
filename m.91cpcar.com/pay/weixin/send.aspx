<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="send.aspx.cs" Inherits="m.cpcar.com.weixin.send" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微信支付转跳</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
      <script src="/Content/js/zepto.min.js"></script>
    <script src="/Content/js/Base.js"></script>
    <script type="text/javascript">

        $(function () {
            var pack = eval(<%=packString %>);

            var oid = Base.getQueryString("oid");;

            var timer = setInterval(function () {
              
                if (typeof WeixinJSBridge != "undefined") {
                    clearInterval(timer);
                    call();
                }
            }, 100);

            function call() {
                if (typeof WeixinJSBridge != "undefined") {
                 //   alert("进入---call 处理方法");
                    WeixinJSBridge.invoke('getBrandWCPayRequest', pack, function (res) {
                       // var type = getQueryString("recharge");
                        $(".isloading").hide();
                     //   alert("res.err_msg：" + res.err_msg);
                        if (res.err_msg == "get_brand_wcpay_request:ok") {
                          
                            updateOrder();
                        } else {
                            alert("支付失败或已取消支付，系统将在您点击确定后跳回订单页");
                         //   if (type == "recharge") {
                        //        window.location.href = "/Member/MyInfo";
                        //    } else {
                                window.location.href = "/Member/MyOrder";
                        //    }
                        }
                    });
                }
                else {
                    alert("此浏览器不支持支付，请使用微信内置浏览器");
                }
            }

            function callpay() {
                if (typeof WeixinJSBridge == "undefined") {
                    if (document.addEventListener) {
                        document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                    }
                    else if (document.attachEvent) {
                        document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                        document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                    }
                }
                else {
                    jsApiCall();
                }
            }

            function updateOrder() {
              //  alert("进入---updateOrder");
                Base.ajaxPost("/WeixinPay/updateOrderForPayFinished/", { oid: oid }, function (data, error) {
                    if (data.Status == "ok") {                  
                            window.location.href = "/Order/Detail?oid=" + oid;///pay/recharge_success.html
                     
                    } else {
                        alert(data.error);
                    }
                })

            }
        })


    </script>
    <style>

        .cue-pre {
            top: 20%;
            left: 50%;
            width: 80px;
            height: 80px;
            margin-left: -40px;
            border-radius: 60px;
            z-index: 999;
            position: absolute;
            overflow: hidden;
            -webkit-transition: opacity 0.3s linear;
            transition: opacity 0.3s linear;
        }

        * {
  margin: 0;
  padding: 0;
  line-height: 1em;
  font-family: 'microsoft yahei',Verdana,Arial,Helvetica,sans-serif;
}


         .load1 { margin: auto; width: 100%; height: 20%; text-align: center; font-size: 10px; position:absolute; top:0; left:0; bottom:0; right:0; }
        .load1 > div {
          background-color: #67CF22;
          height: 100px;
          width: 6px;
          display: inline-block;
          -webkit-animation: stretchdelay 1.2s infinite ease-in-out;
          animation: stretchdelay 1.2s infinite ease-in-out;
        }
        .load1 .rect2 {
          -webkit-animation-delay: -1.1s;
          animation-delay: -1.1s;
        }
        .load1 .rect3 {
          -webkit-animation-delay: -1.0s;
          animation-delay: -1.0s;
        }
        .load1 .rect4 {
          -webkit-animation-delay: -0.9s;
          animation-delay: -0.9s;
        }
		p{ font-size:14px; color:#999; font-family:microsoft yahei;}
        @-webkit-keyframes stretchdelay {
          0%, 40%, 100% { -webkit-transform: scaleY(0.4) }
          20% { -webkit-transform: scaleY(1.0) }
        }
        @keyframes stretchdelay {
          0%, 40%, 100% {
            transform: scaleY(0.4);
            -webkit-transform: scaleY(0.4);
          }  20% {
            transform: scaleY(1.0);
            -webkit-transform: scaleY(1.0);
          }
        }
    </style>



</head>
<body >
    <!--顶部栏-->
    <header class="m_top">
        <a href="/" title="首页" class="m_return"><span></span>返回首页</a>
        <h1>微信支付</h1>
    </header>
    <!--底部栏-->
   <!--- <footer class="m_footer">
        <p>
            <a href="login.html" title="">登录</a>|
        <a href="reg.html" title="">注册</a>
        </p>
        <p>
            <a href="#" title="">电脑版</a>|
        <a href="#" title="">触屏版</a>|
        <a href="#" title="">客户端</a>
        </p>
        <p>m.bestcake.com       400-627-5757     沪ICP备xxxxxxxxxx号</p>
    </footer>
       --->
     <div class="load1" id="div_2">
          <div class="rect1"></div>
          <div class="rect2"></div>
          <div class="rect3"></div>
          <div class="rect4"></div>
          <p>页面正在加载中，请稍候…</p>
    </div>
   <!----
      <div class="waiting-bg-pre isloading">
    </div>
  
    <div class="cue-pre  isloading" >
        <div class="loading-pre">
            <div id="waiteL">
                <div class="bar1"></div>
                <div class="bar2"></div>
                <div class="bar3"></div>
                <div class="bar4"></div>
                <div class="bar5"></div>
                <div class="bar6"></div>
                <div class="bar7"></div>
                <div class="bar8"></div>
            </div>
        </div>
    </div>--->

</body>
</html>
