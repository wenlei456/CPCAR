
function Share(p) {
    if (signature && wx&&p) {
        wx.config({
            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: 'wx1898f60d404422c1', // 必填，公众号的唯一标识
            timestamp: p.timestamp, // 必填，生成签名的时间戳
            nonceStr: p.nonceStr, // 必填，生成签名的随机串
            signature: signature,// 必填，签名，见附录1
            jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
        });
        wx.ready(function () {
            // 1 判断当前版本是否支持指定 JS 接口，支持批量判断
            wx.checkJsApi({
                jsApiList: [
                  'onMenuShareTimeline',
                  'onMenuShareAppMessage'
                ],
                success: function (res) {
                    if (!res.checkResult.onMenuShareTimeline) {
                        //微信低版本处理

                    }
                }
            });
            //分享给朋友
            wx.onMenuShareAppMessage({
                title: p.title,
                desc: p.content,
                link: p.link,
                imgUrl: p.imgUrl,
                trigger: function () {
                    // 你可以在这里对分享的数据进行重组

                },
                success: function () {
                    $("#shareDiv").css("display", "none");
                },
                cancel: function () {
                    $("#shareDiv").css("display", "none");
                },
                fail: function (res) {
                }
            });


            //分享到朋友圈
            wx.onMenuShareTimeline({
                title: p.content,
                //desc: '我在贝思客【极致蛋糕也疯狂2】第一关，通关1000万极致币大放送，你牛逼？来挑战！',
                link: p.link,
                imgUrl: p.imgUrl,
                trigger: function () {
                    // 你可以在这里对分享的数据进行重组
                },
                success: function () {
                    $("#shareDiv").css("display", "none");
                },
                cancel: function () {
                    $("#shareDiv").css("display", "none");
                },
                fail: function (res) {
                }
            });
        });
    }
}