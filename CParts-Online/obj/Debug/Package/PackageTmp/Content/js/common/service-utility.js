
/*
校验是否为数字
*/
function validate(value) {
    var reg = new RegExp("^[0-9]+$");
    if (!reg.test(value)) {
        return false;
    } else {
        return true;
    }
}
//判断用户是否登陆
function IsLogin() {
    Post("/QuickBuy/CheckLogin", null, function (data, error) {
        if (error) {
            ChangBox("网络异常，请稍后再试");
        }
        if (data == "ok") {
            return true;
        } else {
            return false;
        }
    });

}
//手机验证
function phoneRegex(val) {
    var regex = /^1[3|4|5|7|8][0-9]\d{8}$/;
    if (!regex.test(val)) {
        return false;
    }
    return true;
}
//时间
function tsFormat(ts) {
            if (ts < 0) {
                ts = 0;
            }
            var o = {
                d: Math.floor(ts / (1000 * 60 * 60 * 24)),
                h: Math.floor(ts / (1000 * 60 * 60)) % 24,
                m: Math.floor(ts / (1000 * 60)) % 60,
                s: Math.floor(ts / 1000) % 60,
                ss: Math.floor(ts / 100) % 10
            };

            o.d = o.d > 9 ? o.d.toString() : "0" + o.d;
            o.dh = o.d[0];
            o.dl = o.d[1];

            o.h = o.h > 9 ? o.h.toString() : "0" + o.h;
            o.hh = o.h[0];
            o.hl = o.h[1];

            o.m = o.m > 9 ? o.m.toString() : "0" + o.m;
            o.mh = o.m[0];
            o.ml = o.m[1];

            o.s = o.s > 9 ? o.s.toString() : "0" + o.s;
            o.sh = o.s[0];
            o.sl = o.s[1];

            o.ss = o.ss > 9 ? o.ss.toString() : "0" + o.ss;
            o.ssh = o.ss[0];
            o.ssl = o.ss[1];
            return o;
        }
function dateFormat(fmt, date) {
            if (!date) {
                date = new Date();
            }
            else {
                if (typeof date != 'object') {
                    date = new Date(date);
                }
            }
            var o = {
                "M+": date.getMonth() + 1, //月份 
                "d+": date.getDate(), //日 
                "h+": date.getHours(), //小时 
                "m+": date.getMinutes(), //分 
                "s+": date.getSeconds(), //秒 
                "q+": Math.floor((date.getMonth() + 3) / 3), //季度 
                "S": date.getMilliseconds(), //毫秒
                "w+": this.getWeek(date)
            };
            if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return fmt;
        }
function getWeek(date) {
            var dayNames = ['日', '一', '二', '三', '四', '五', '六'];
            return dayNames[date.getDay()];
        }
 function format(str) {
            var args = arguments;
            var val = str.replace(/{(\d+)}/g, function (match, number) {
                var idx = parseInt(number) + 1;
                return args[idx] === undefined ? match : args[idx];
            });
            return val;
 }
 function jsonDateToDate(date) {
            var date = new Date(parseInt(date.slice(6, -2)));
            return date;
        }
 function jsonDateToDateString(date) {
            var date = new Date(parseInt(date.slice(6, -2)));
            return this.dateFormat("yyyy-MM-dd", date);
        }
 function jsonDateToDateTimeString(date) {
            var date = new Date(parseInt(date.slice(6, -2)));
            return this.dateFormat("yyyy-MM-dd hh:mm:ss", date);
        }
 function JsonDateToDateTimeByFormat(date, fmt) {
            var date = new Date(parseInt(date.slice(6, -2)));
            var fmt = fmt || "yyyy/MM/dd hh:mm:ss";
            return this.dateFormat(fmt, date);
        }
 function addDays(date, d) {
            return new Date(date.getTime()).setDate(date.getDate() + d);
        }
 function nameFromTime(t) {
            var names = [
                { t: 0, n: "凌晨" },
                { t: 5, n: "上午" },
                { t: 11, n: "中午" },
                { t: 13, n: "下午" },
                { t: 17, n: "傍晚" },
                { t: 19, n: "晚上" }
            ];
            for (var i = 0; i < names.length; i++) {
                if (names[i].t > t.getHours()) {
                    return names[i - 1].n;
                }
            }
            return names[5].n;
        }
 function emailRegex(val) {
            var regex = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
            if (!regex.test(val)) {
                return false;
            }
            return true;
        }
 
