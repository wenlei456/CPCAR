//$(function () {
//    $(document).ajaxComplete(function (event, jqXHR, options) {
//        setTimeout(function () {
//            HideModal();
//        }, 500);
//    });
//})
var Base = {
    //时间戳标记
    getNow: function () {
        var myDate = new Date();
        var now = myDate.getTime();
        var st = encodeURI(window.location.pathname);
        st = st.replace(/\//g, "-");
        st = st.replace(/\./g, "_");
        now = st + now;
        return now;
    },
    //post请求
    ajaxPost: function (src, param, callback) {
        var cb = arguments[arguments.length - 1];
        $.ajax({
            type: 'POST',
            url: src + "?v=" + this.getNow(),
            data: param,
            dataType: 'json',   
            success: function (data) {
                if (cb) {
                    cb(data);
                }
            },
            error: function (xhr, type) {
                Base.HideModal();
                //alert('网络异常请稍后再试!');
            },
        });
    },
    //get请求
    ajaxGet: function (src, param, callback) {
        var cb = arguments[arguments.length - 1];
        $.ajax({
            type: 'Get',
            url: src + "?v=" + this.getNow(),
            data: param,
            dataType: 'json',      
            success: function (data) {
                if (cb) {
                    cb(data);
                }
            },
            error: function (xhr, type) {
                Base.HideModal();
                alert('Ajax error!')
            },
        });
    },
    //json to boject
    Json2Obj: function (json) {
        var obj;
        if (json) {
            obj = eval("(" + json + ")");
            return obj;
        } else {
            return null;
        }
    },
    //获取URL参数
    getQueryString: function (value) {
        var reg = new RegExp("(^|&)" + value + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) {
            return unescape(r[2]);
        } else {
            return null;
        }
    },
    //显示等待
    ShowModal: function () {
        $("#modal-bg").show();
        $("#modal-show").show();
    },
    //隐藏等待
    HideModal: function () {
        $("#modal-bg").hide();
        $("#modal-show").hide();
    },
    Trim: function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },

};
/***
验证
**/
var Validata = {
    /*
    校验是否为整数
    */
    isInteger:function (value) {
        var reg = new RegExp("^[0-9]+$");
        if (!reg.test(value)) {
            return false;
        } else {
            return true;
        }
    },
    /*
    校验是否为数字包含小数
    */
    isNumber:function (oNum) {
    if (!oNum) return false;
    var strP = /^\d+(\.\d+)?$/;
    if (!strP.test(oNum)) return false;
    try {
        if (parseFloat(oNum) != oNum) return false;
    }
    catch (ex) {
        return false;
    }
    return true;
},
//手机验证
    isPhone:function (val) {
    var regex = /^1[3|4|5|7|8][0-9]\d{8}$/;
    if (!regex.test(val)) {
        return false;
    }
    return true;
},
//判断客户端
    IsPC:function IsPC() {
    var userAgentInfo = navigator.userAgent;
    var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
    var flag = true;
    for (var v = 0; v < Agents.length; v++)
    {
        if (userAgentInfo.indexOf(Agents[v]) > 0) { flag = false; break; }
    }
    return flag;
    },
    isNull: function (obj) {
    if (typeof (obj) == "undefined")
return true;
if (obj == undefined)
    return true;
if (obj == null)
    return true;
if (obj === "")
    return true;
return false;
    },
};