//全局配置
GlobalConfig = {
    //SingalR链接服务器地址
    SignalRConnUrl: "http://qf-dekins-02:8111/signalr",
    TempValue: null,
    GetGlobalSetting: function (key) {
        //同步取值
        $.ajax({
            url: '/SystemConfig/GetGlobalSettingByKey',
            type: 'POST',
            data: { 'key': key },
            async: false,
            cache: false,
            success: function (data) {
                GlobalConfig.TempValue = data;
            },
            error: function (data) {
                GlobalConfig.TempValue = null;
            }
        });
        return GlobalConfig.TempValue;
    },
    /* 联系人1选项，在第一次选择 已婚状态 时会初始化值 */
    ContactOptions1: null,
    /* 联系人2选项，在第一次选择 驾照持有人 时会初始化值 */
    ContactOptions2: null
};
//前端帮助函数
Utilities = {
    ShowLoadingImg: function (e) {
        $("#loading").modal('show');
        $("#loading").show();
        $("body").css("overflow", "hidden");
    },
    HideLoadingImg: function (e) {
        $("#loading").modal('hide');
        $("#loading").hide();
        $("body").css("overflow", "auto");
    },
    //获取RUL参数
    getUrlParam: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg);  //匹配目标参数
        if (r != null) return unescape(r[2]); return ""; //返回参数值
    },
    //获取父页面URL参数
    getParentUrlParam: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = parent.location.search.substr(1).match(reg);  //匹配目标参数
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
    /***
        * 自动关闭窗口顶部弹出内容提示 （add by zhaolei  时间：20160329）
        * timeOut : 4000, //提示层显示的时间
        * msg : "",       //显示的消息
        * speed : 300,    //滑动速度
        * type : "success"//提示类型（1、success 2、error 3、warning）
    ***/
    ShowMsg:function(timeOut, msg, speed, type) {
        $(".tip_container").remove();
        var bid = parseInt(Math.random() * 100000);
        $("body").prepend('<div id="tip_container' + bid + '" class="container tip_container"><div id="tip' + bid + '" class="mtip"><span id="tsc' + bid + '"></span></div></div>');
        var $this = $(this);
        var $tip_container = $("#tip_container" + bid);
        var $tip = $("#tip" + bid);
        var $tipSpan = $("#tsc" + bid);
        //先清楚定时器
        clearTimeout(window.timer);
        //主体元素绑定事件
        $tip.attr("class", type).addClass("mtip");
        $tipSpan.html(msg);
        $tip_container.slideDown(speed);
        //提示层隐藏定时器
        window.timer = setTimeout(function () {
            $tip_container.slideUp(speed);
            $(".tip_container").remove();
        }, timeOut);
        //鼠标移到提示层时清除定时器
        $tip_container.bind("mouseover", function () {
            clearTimeout(window.timer);
        });
        //鼠标移出提示层时启动定时器
        $tip_container.bind("mouseout", function () {
            window.timer = setTimeout(function () {
                $tip_container.slideUp(speed);
                $(".tip_container").remove();
            }, timeOut);
        });
        $("#tip_container" + bid).css("left", ($(window).width() - $("#tip_container" + bid).width()) / 2);
        //$("#tip_container" + bid).css("top", ($(window).height() - $("#tip_container" + bid).height()) / 2);
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
    FormatMSDateTime: function (dateTime) {
        if (dateTime == null) { return ""; }
        var reg = /-?\d+/;
        var dt = reg.exec(dateTime);
        var date = new Date(parseInt(dt[0]));
        return Utilities.formatDate(date, "yyyy-MM-dd hh:mm");
    },
    //长度控制，用于动态表单的Number
    checkLength: function (val, length) {
        regex = '/^(\\d{' + length + '}).*/';
        return val.replace(eval(regex), '$1');
    },
    //小数点后移i位
    MoveDecimalPoint: function (number, i) {
        var tempAry = number.toString().split('.');
        var xiaoshu = 0;
        if (tempAry.length > 1 && tempAry[1].length > 2) {
            /* -i的由来：number * 100 中100的位数 */
            xiaoshu = tempAry[1].length - i;
        }
        return (number * Math.pow(10, i)).toFixed(xiaoshu);
    },
    //判断是否为车贷
    _cheDaiLogos: null,
    IsCheDai: function (logo) {
        if (!this._cheDaiLogos) {
            this._cheDaiLogos = GlobalConfig.GetGlobalSetting('CheDaiLogos');
        }
        if (logo) {
            return this._cheDaiLogos.indexOf(logo) >= 0;
        } else {
            //如果没有参数，则通过当前url中的search判断
            //如果匹配成功，则结果数组中下标为[1]或[2]的为产品logo
            //var r = /dformCode=(\w+)&.*$|dformCode=(\w+)$/;
            //var m = window.location.search.match(r);
            //var l = m && m.length > 2 ? (m[1] ? m[1] : m[2]) : null;
            var l = Utilities.getUrlParam('dformCode');
            return this._cheDaiLogos.indexOf(l) >= 0;
        }
    },
    //判断是否为房贷
    _houseLogos: null,
    IsHouse: function (logo) {
        if (!this._houseLogos) {
            this._houseLogos = GlobalConfig.GetGlobalSetting('HouseLogos');
        }
        if (logo) {
            return this._houseLogos.indexOf(logo) >= 0;
        } else {
            //如果没有参数，则通过当前url中的search判断
            //如果匹配成功，则结果数组中下标为[1]或[2]的为产品logo
            //var r = /dformCode=(\w+)&.*$|dformCode=(\w+)$/;
            //var m = window.location.search.match(r);
            //var l = m && m.length > 2 ? (m[1] ? m[1] : m[2]) : null;
            var l = Utilities.getUrlParam('dformCode');
            return this._houseLogos.indexOf(l) >= 0;
        }
    },
    /* 将select控件设置为只读，element为jQuery对象 */
    SetSelectReadOnly:function(element) {
        element.keydown(function() {
                return false;
            }).mousedown(function() {
                return false;
            }).css('background-color','#f5f5f5');
    },
    CancelSelectReadOnly: function(element) {
        element.unbind('keydown').unbind('mousedown').css('background-color','#FFF');
    },
    //身份证合法性
    IsIdCard: function (value) {
        var Errors = new Array(
            "验证通过!",
            "身份证号码位数不对!",
            "身份证号码出生日期超出范围或含有非法字符!",
            "身份证号码校验错误!",
            "身份证地区非法!"
        );
        var area = {
            11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江",
            31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北",
            43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏",
            61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外"
        }

        var Y, JYM;
        var S, M;
        var cardarray = new Array();
        cardarray = value.split("");

        if (value.length != 18)
            return false;
        //return Errors[1];

        //地区检验
        if (area[parseInt(value.substr(0, 2))] == null)
            return false;
        //return Errors[4];
        //身份号码格式检验

        //出生日期的合法性检查
        //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))
        //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))
        if (parseInt(value.substr(6, 4)) % 4 == 0 || (parseInt(value.substr(6, 4)) % 100 == 0 && parseInt(value.substr(6, 4)) % 4 == 0)) {
            //闰年出生日期的合法性正则表达式
            ereg = /^[1-9][0-9]{5}[0-9]{4}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/;
        } else {
            //平年出生日期的合法性正则表达式
            ereg = /^[1-9][0-9]{5}[0-9]{4}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/;
        }
        if (ereg.test(value)) {
            //测试出生日期的合法性
            //计算校验位
            S = (parseInt(cardarray[0]) + parseInt(cardarray[10])) * 7 + (parseInt(cardarray[1]) + parseInt(cardarray[11])) * 9
                + (parseInt(cardarray[2]) + parseInt(cardarray[12])) * 10 + (parseInt(cardarray[3]) + parseInt(cardarray[13])) * 5
                + (parseInt(cardarray[4]) + parseInt(cardarray[14])) * 8 + (parseInt(cardarray[5]) + parseInt(cardarray[15])) * 4
                + (parseInt(cardarray[6]) + parseInt(cardarray[16])) * 2 + parseInt(cardarray[7]) * 1 + parseInt(cardarray[8]) * 6
                + parseInt(cardarray[9]) * 3;
            Y = S % 11;
            M = "F";
            JYM = "10X98765432";
            M = JYM.substr(Y, 1); //判断校验位
            if (M == cardarray[17])
                return true;
                //return Errors[0]; //检测ID的校验位
            else
                return false;
            //return Errors[3];
        } else
            return false;
        //return Errors[2];

    },
    _geekLogo: 'productCoderapid',
    IsGeek: function(logo) {
        if (logo) {
            return this._geekLogo.indexOf(logo) >= 0;
        } else {
            var l = Utilities.getUrlParam('dformCode');
            return this._geekLogo.indexOf(l) >= 0;
        }
    },
    /*非负数验证*/
    CheckNum: function (val) {
        return /^\d+(\.\d+)?$/.test(val);
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
