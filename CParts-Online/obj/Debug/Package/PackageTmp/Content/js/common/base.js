//标记
function getnow() {
    var myDate = new Date();
    var now = myDate.getTime();
    var st = encodeURI(window.location.pathname);
    st = st.replace(/\//g, "-");
    st = st.replace(/\./g, "_");
    now = st + now;
    return now;
}
//post请求
function Post(src,param, callback) {
    var cb = arguments[arguments.length - 1];
    $.post(src+"?v=" + getnow(), param, function (data) {   
            if (cb)
            {
                cb(data);
            }
      
    }, "json");
}
//get请求
function Get(src,param, callback) {
    var cb = arguments[arguments.length - 1];
 var msg= $.getJSON(src + "?v=" + getnow(), param, function (data) {
        if (data) {         
            if (cb) {
                cb(data);
            }
        }       
 });

}
//json to boject
function JSON2Obj(json) {
    var obj;
    if (json) {
        obj = eval("(" + json + ")");
        return obj;
    } else {
        return null;
    }
   
}
//转义
function strZY(str)
{
 
    str=str.replace(/&quot;/g, "\"");
    /**
    string = string.Replace(">", "&gt;");
    string = string.Replace("<", "&lt;");
    string = string.Replace(" ", "&nbsp;");
    string = string.Replace("\"", "&quot;");
    string = string.Replace("\'", "&#39;");
    string = string.Replace("\\", "\\\\");//对斜线的转义  
    string = string.Replace("\n", "\\n");
    string = string.Replace("\r", "\\r");
    **/
    return str;
}
//获取URL参数
function getQueryString (value)
{
    var reg = new RegExp("(^|&)" + value + "=([^&]*)(&|$)", "i"); 
    var href = decodeURI(window.location.search);
    var r = href.substr(1).match(reg);
    if (r != null) {
        return unescape(r[2]);
    } else {
        return null;
    }

}
//js post提交
function Js2Post(URL, PARAMS) {
  
    var temp = document.createElement("form");
    temp.action = URL;
    temp.method = "post";
    temp.style.display = "none";
    for (var x in PARAMS) {
        var opt = document.createElement("textarea");
        opt.name = x;
        opt.value = PARAMS[x];
        // alert(opt.name)      
        temp.appendChild(opt);
    }
    document.body.appendChild(temp);
    temp.submit();
 
    return temp;
}
//预览网页

function PreviewHTML(content) {
    {       
        go=open('','预览','');
        go.document.open();
        go.document.write(content);
        go.document.close();
    }
}
/*
校验是否为整数
*/
function isInteger(value) {
    var reg = new RegExp("^[0-9]+$");
    if (!reg.test(value)) {
        return false;
    } else {
        return true;
    }
}
/*
校验是否为数字包含小数
*/
function isNumber(oNum) {
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
}
//手机验证
function phoneRegex(val) {
    var regex = /^1[3|4|5|7|8][0-9]\d{8}$/;
    if (!regex.test(val)) {
        return false;
    }
    return true;
}
//判断客户端
function IsPC() {
    var userAgentInfo = navigator.userAgent;
    var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
    var flag = true;
    for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) { flag = false; break; }
    }
    return flag;
}

function isNull(obj) {
    if (typeof (obj) == "undefined")
        return true;
    if (obj == undefined)
        return true;
    if (obj == null)
        return true;
    if (obj === "")
        return true;
    return false;
}


if (!IsPC())
{
    window.location.href = "http://m.91cpcar.com";
} 
