
//前端帮助函数
Utilities = {
    ShowLoadingImg: function (e) {
        $("#loading").modal('show');
        $("#loading").show();
        $("body").css("overflow-y", "hidden");
    },
    HideLoadingImg: function (e) {
        $("#loading").modal('hide');
        $("#loading").hide();
        $("body").css("overflow-y", "auto");
    },
    //获取RUL参数
    getUrlParam: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg);  //匹配目标参数
        if (r != null) return unescape(r[2]); return ""; //返回参数值
    },
    alertTip: function (str) {
        $.gritter.add({
            title: '提示',
            text: str,
            time: '1500',
            class_name: 'gritter-info gritter-center'
        });
    },
    //格式化日期,
    formatDate: function (date, format) {
        var paddNum = function (num) {
            num += "";
            return num.replace(/^(\d)$/, "0$1");
        }
        //指定格式字符
        var cfg = {
            yyyy: date.getFullYear() //年 : 4位
            , yy: date.getFullYear().toString().substring(2)//年 : 2位
            , M: date.getMonth() + 1  //月 : 如果1位的时候不补0
            , MM: paddNum(date.getMonth() + 1) //月 : 如果1位的时候补0
            , d: date.getDate()   //日 : 如果1位的时候不补0
            , dd: paddNum(date.getDate())//日 : 如果1位的时候补0
            , hh: date.getHours()  //时
            , mm: date.getMinutes() //分
            , ss: date.getSeconds() //秒
        }
        format || (format = "yyyy-MM-dd hh:mm:ss");
        return format.replace(/([a-z])(\1)*/ig, function (m) { return cfg[m]; });
    },
    //长度控制，用于动态表单的Number
    checkLength: function (val,length) {
        regex = '/^(\\d{'+length+'}).*/';
        return val.replace(eval(regex), '$1');
    }
};




//JS载入
$(function () {
    $(document).ajaxStart(Utilities.ShowLoadingImg)
               .ajaxStop(Utilities.HideLoadingImg);
    $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
        if (jqxhr.status == 401) {
            alert("授权过期，请重新登录");
            if (self.frameElement != null && self.frameElement.tagName == "IFRAME") {
                window.parent.location = "/home/login?backUrl=" + escape(window.parent.location);
            } else {
                window.location = "/home/login?backUrl=" + escape(window.location);
            }
        }
    });
});
