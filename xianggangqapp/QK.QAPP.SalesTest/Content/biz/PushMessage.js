$(function () {
    var SupplementProxy = $.connection.supplementHub;
    $.connection.hub.logging = true;

    SupplementProxy.client.toWeb = function (user, msg, category) {
        //alert("收到服务器消息:" + msg);
        //if (category == "SupplementStart") {
        //    var title = "用户补件通知1";
        //    var content = msg;
        //    AddNotifiy(title, msg, '5000', false);
        //}        
    };

    SupplementProxy.client.toWebAll = function (user, msg, category) {
        if (category == "SYSMSG2ALL") {
            var title = "系统消息";
            AddNotifiy(title, msg, '500000', false, "tip1.png");
        }
        if (category == "LogoutAll") {
            var title = "登录信息";
            AddNotifiy(title, msg + "(5秒后自动登出)", '500000', false, "safetywarning.png");
                window.setTimeout(function () {
                    window.location.href = "/Home/Login";
                }, 5000);
        }
        if (user != "") {
            if (user == GUser.UserAccount && category == "SupplementStart") {
                var title = "用户补件通知";
                var _msg = eval("(" + msg + ")");
                var Url = "/LoanApplication/Application?dformCode=" + _msg.Logo + "&operation=2&appid=" + _msg.AppId;
                var StrTemplate = "<a href='" + Url + "'>客户:'" + _msg.CustomerName + "'需要补件,请及快处理</a>";
                AddNotifiy(title, StrTemplate, '500000', false, "tip2.png");
            }
            if (user == GUser.UserAccount && category == "LoginInfo") {
                var title = "登录信息";
                AddNotifiy(title, msg + "(5秒后自动登出)", '500000', false, "safetywarning.png");
                window.setTimeout(function () {
                    window.location.href = "/Home/Login";
                }, 5000);
            }
            if (user == GUser.UserAccount && category == "SYSMSG2SOMEBODY") {
                var title = "系统消息";
                AddNotifiy(title, msg, '500000', false, "tip1.png");
            }
            
        }
    };

    $.connection.hub.start().done(function () {

    });
    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000);
    });

    //测试发送给APP
    $("#btnSend").bind("click", function () {
        var sendUser = $("#sendUser").val();
        var msg = $("#ttContent").val();
        var category = "SystemReStart";
        SupplementProxy.server.sendMsg(sendUser, msg, category);
    });
    $("#btnSendALL").bind("click", function () {
        var msg = $("#ttContent").val();
        var category = "SystemReStart";
        SupplementProxy.server.sendMsgAll(msg, category);
    });
});

/// <summary>
/// 通知提示框
/// </summary>
/// <param name="title">标题</param>
/// <param name="content">内容</param>
/// <param name="hidetime">显示时间</param>
/// <param name="sticky">是否固定</param>
function AddNotifiy(title, content, hidetime, sticky, img) {
    var unique_id = $.gritter.add({
        title: title,
        text: content,
        sticky: sticky,
        image: "/Content/assets/images/" + img,
        time: hidetime,
        class_name: 'gritter-info '
    });
}