//////////////////////////////////////////////////////////////////////////
function CopyToClipboard(txt) {
    try {
        var oInput = document.createElement('div');
        oInput.style.width = "1px";
        oInput.style.height = "1px";
        oInput.innerText = txt;
        document.body.appendChild(oInput);
        var range = document.createRange();
        range.selectNode(oInput);
        var selection = window.getSelection();
        if (selection.rangeCount > 0) selection.removeAllRanges();
        selection.addRange(range);
        document.execCommand("Copy");
        document.body.removeChild(oInput);

        return true;
    } catch (e) {
        return false;
    }
}
//////////////////////////////////////////////////////////////////////////
//格式化千分位
String.prototype.FormatMoney = function () {
    var num = this;
    if (num != null && num != "") {
        num = num * 1.0;
        return (num.toFixed(2) + '').replace(/\d{1,3}(?=(\d{3})+(\.\d*)?$)/g, '$&,');
    } else {
        return num;
    }
};
//从千分位去掉逗号
String.prototype.FormatFromMoney = function () {
    var str = this;
    if (str != null && str != "") {
        return str.replace(/[^0123456789.]/g, '');
    } else {
        return str;
    }
};
//////////////////////////////////////////////////////////////////////////
String.prototype.trim = function () {
    return this.replace(/(^\s+)|\s+$/g, "");
};
//////////////////////////////////////////////////////////////////////////
Array.prototype.remove = function (dx) {
    if (isNaN(dx) || dx > this.length) { return false; }
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != this[dx]) {
            this[n++] = this[i]
        }
    }
    this.length -= 1
};
String.prototype.replaceAll = function (value1, value2)//replace加强版
{
    var returnValue = this;
    var i = 0;
    while (returnValue.indexOf(value1, i) != -1) {
        returnValue = returnValue.replace(value1, value2);
        i++;
    }
    return returnValue;
};
//////////////////////////////////////////////////////////////////////////
//时间操作
String.prototype.ToDate = function () {
    var str = this.replaceAll("-", "/");
    if (str.indexOf('.') !== -1) { str = str.split('.')[0]; }
    var t1 = new Date(Date.parse(str));
    if (isNaN(t1)) {
        var ts = str.split(' ')[0].split("/");
        return new Date(parseInt(ts[0]), parseInt(ts[1]) - 1, parseInt(ts[2]));
    } else
        return t1;
};
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份 
        "d+": this.getDate(),                    //日 
        "h+": this.getHours(),                   //小时 
        "m+": this.getMinutes(),                 //分 
        "s+": this.getSeconds(),                 //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds()             //毫秒 
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};
Date.prototype.AddDay = function (day) {
    var a = this.valueOf();
    a = a + day * 24 * 60 * 60 * 1000;
    a = new Date(a);
    return a;
};
Date.prototype.ToString = function () {
    var ms = this.valueOf();
    if (ms === null)
        return null;
    try {
        var date = new Date(ms);
        var seperator1 = "-";
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentdate = year + seperator1 + month + seperator1 + strDate;
        return currentdate;
    }
    catch (e) {
        console.log(e);
    }
    return null;
};
Array.prototype.First = function () {
    var datas = this.valueOf();
    var value = null;
    if (datas && datas.length > 0) {
        value = $(datas).get(0);
    }
    return value;
};
//////////////////////////////////////////////////////////////////////////