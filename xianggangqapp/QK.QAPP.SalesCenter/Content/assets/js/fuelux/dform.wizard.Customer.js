var wizard;
var wizardObj;
//绑定表单提交
$(function () {
    //设置默认页面
    var op = Utilities.getUrlParam("operation");
    var appid = Utilities.getUrlParam("appid");
    //编辑状态或车贷的补件状态默认显示表单第一页
    if (op == 1 || op == 4) {
        $(".wizard-steps>li:first").addClass("active");
        bindbeforeunload();
    }
    else if (op == 2) {
        $(".wizard-steps>li").addClass("complete");

        $(".btn-next").text($(".btn-next").attr("data-last"));
    }
    else {
        $(".wizard-steps>li").addClass("complete");
        $(".step-pane").first().addClass("active");
        $(".wizard-actions").hide();
    }

    //表单上一步下一步事件绑定
    var $validation = false;
    //绑定表单提交
    wizard = $('#fuelux-wizard').ace_wizard();
    $(".form-horizontal[data-readonly='False']").parent(".step-pane").first().addClass("active");
    wizard.on('change', function (e, info, obj) {
        wizardObj = info;
        var thisForm = $('#step' + info.step + '>form');

        //在Readonly时不进行提交
        if (op == 3) {
            return true;
        }

        if (!thisForm.valid()) {
            thisForm.find('.has-error:visible').first().focus();
            jQuery('html,body').animate({ scrollTop: thisForm.find('.has-error:visible').first().offset().top - 300 }, 100);
            return false;
        }
        else {
            var url = $.trim(thisForm.attr("action"));
            if (url) {
                $.ajax({
                    url: $.trim(thisForm.attr("action")),   // 提交的页面
                    data: thisForm.serialize(), // 从表单中获取数据
                    //async: false,
                    type: "POST",                   // 设置请求类型为"POST"，默认为"GET"
                    cache: false,
                    beforeSend: function () {
                        //$("#loading").modal('show');
                    },
                    error: function (request) {      // 设置表单提交出错
                        //$("#loading").modal('hide');
                        bootbox.alert("表单提交出错，请稍候再试或者联系管理员");
                    },
                    success: function (data) {
                        if (data) {
                            bootbox.alert(data);
                        }
                        else {
                            wizardObj.obj[wizardObj.triggerName](false);
                            //var index = parseInt(info.step);

                        }

                    }
                });
            }
            else {
                wizardObj.obj[wizardObj.triggerName](false);
            }

        }
        return true;

    }).on('finished', function (e, info, obj) {
        DFormAction.SpecialFileAmount = null;
        bindbeforeunload();
        var thisForm = $(".step-pane.active>form");
        var submitURL = $(".btn-success.btn-next").attr("data-submitaction");

        thisForm.find(".FileViewerKeyNames").each(function () {
            var mn = $(this).attr("id") + "_Verify";
            eval(mn + "();");
        });

        if (!thisForm.valid()) {
            thisForm.find('.has-error:visible').first().focus();
            jQuery('html,body').animate({ scrollTop: thisForm.find('.has-error:visible').first().offset().top - 300 }, 100);
            return false;
        }
        else {
            $(window).unbind('beforeunload');

            var url = $.trim(thisForm.attr("action"));
            $.ajax({
                url: submitURL,   // 提交的页面
                data: { "appId": appid }, // 从表单中获取数据
                //async: false,
                type: "POST",                   // 设置请求类型为"POST"，默认为"GET"
                cache: false,
                beforeSend: function () {
                    //$("#loading").modal('show');
                },
                error: function (request) {      // 设置表单提交出错
                    //$("#loading").modal('hide');
                    bootbox.alert("表单提交出错，请稍候再试或者联系管理员");
                },
                success: function (data) {
                    if (data) {
                        bootbox.alert(data);
                    }
                    else {
                        bootbox.alert("您的信息已经录入成功，请等待审核！", function () {
                            window.location.href = '/';
                        });
                    }

                }
            });


        }


    }).on('stepclick', function (e) {
        //return false;//prevent clicking on steps

    }).on("changed", function (e) {
        //在Readonly下始终为可以互相切换
        if (op == 3) {
            $(".wizard-steps>li").addClass("complete");
        }
        else if (op == 2) {
            $(".wizard-steps>li").addClass("complete");
            //$(".form-horizontal")
        }
        else {

        }
        var i = parseInt($(".wizard-steps").find(".active>.step").text()) - 1;
        ReadData(i);

    });
    //读取初始数据

    ReadData(0);


    //快速选择的地址
    jQuery(':input:not(:disabled)[name$=Detail]').next().click(function () {
        var addressCollection = new Array();
        var addressDoms = jQuery('.address');
        var current_Select = jQuery(this).parent().parent().parent();

        //收集信息
        for (var i = 0; i < addressDoms.length; i++) {
            //省
            var province = jQuery(addressDoms[i]).find('[id*=Province]');
            var temp_pv = jQuery(province).val();

            if (temp_pv == '' || temp_pv == undefined) {
                addressDoms.splice(i, 1);
                i--;
                continue;
            }
            addressCollection[i] = {};
            addressCollection[i].ProvinceVal = temp_pv;
            addressCollection[i].ProvinceText = jQuery(province).find('option:selected').text();
            //市
            var city = jQuery(addressDoms[i]).find('[id*=City]');
            addressCollection[i].CityVal = jQuery(city).val();
            addressCollection[i].CityText = jQuery(city).find('option:selected').text();
            //详细
            addressCollection[i].Detail = jQuery(addressDoms[i]).find('[id*=Detail]').val();

            addressCollection[i].Name = jQuery(addressDoms[i]).data('name');
        }


        var radioHtml = '无地址可供选择！';
        //处理信息
        if (addressCollection.length > 0) {
            radioHtml = '';
            for (var i = 0; i < addressCollection.length; i++) {
                radioHtml += '<div class="control-group"><label><input name="Address_Dialog" value="' + i + '" type="radio" class="ace" /><span class="lbl">' +
                    addressCollection[i].Name + ':' + '  ' +
                    addressCollection[i].ProvinceText + ' ' +
                    addressCollection[i].CityText + ' ' +
                    addressCollection[i].Detail +
                    '</span></label></div>';

                if (i < addressCollection.length - 1) {
                    radioHtml += '<hr>';
                }
            }
        }

        //展示
        bootbox.dialog({
            message: radioHtml,
            title: "选择地址",
            buttons: {
                success: {
                    label: "选择",
                    className: "btn-info",
                    callback: function () {
                        var selectedValue = jQuery(':radio[name=Address_Dialog]:checked').val();

                        jQuery(current_Select)
                            .find('[id*=Detail]').val(addressCollection[selectedValue].Detail).end()
                            .find('[id*=City]').append('<option value="' + addressCollection[selectedValue].CityVal
                            + '" selected="selected">' + addressCollection[selectedValue].CityText + '</option>').end()
                            .find('[id*=Province]').val(addressCollection[selectedValue].ProvinceVal).change();

                        ////500毫秒后设置 城市
                        //setTimeout(function () {
                        //    jQuery(current_Select).find('[id*=City]').val(addressCollection[selectedValue].CityVal);
                        //}, 1000);
                    }
                }
            }
        });

    });//.css('cursor', 'pointer')



    $('#modal-wizard .modal-header').ace_wizard();
    $('#modal-wizard .wizard-actions .btn[data-dismiss=modal]').removeAttr('disabled');


    //override dialog's title function to allow for HTML titles
    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || '&nbsp;'
            if (("title_html" in this.options) && this.options.title_html == true)
                title.html($title);
            else title.text($title);
        }
    }));

    //保存当前

    $("#saveCurrent").click(function () {
        var thisForm = $(".step-pane.active>form");
        var url = $.trim(thisForm.attr("action"));
        //if (!thisForm.valid()) {
        if (false) {
            return false;
        }
        else {
            $(window).unbind('beforeunload');
            if (url) {
                $.ajax({
                    url: url,   // 提交的页面
                    data: thisForm.serialize(), // 从表单中获取数据
                    type: "POST",                   // 设置请求类型为"POST"，默认为"GET"
                    cache: false,
                    beforeSend: function () {
                    },
                    error: function (request) {      // 设置表单提交出错
                        bootbox.alert("表单提交出错，请稍候再试或者联系管理员");
                    },
                    success: function (data) {
                        //    if (data) {
                        //       // bootbox.alert(data);
                        //    }
                        //    else {
                        //        bootbox.alert("保存成功");
                        //    }
                        if (data != "") {
                            Utilities.alertTip("保存失败:" + data);
                        }
                        else {
                            Utilities.alertTip("保存成功");
                        }
                        var i = parseInt($(".wizard-steps").find(".active>.step").text()) - 1;
                        ReadData(i);

                    }
                });


            }
            else {
                Utilities.alertTip("保存成功");
            }
        }
    });


});
var ReadData = function (i) {
    var appid = Utilities.getUrlParam("appid");
    var form = jQuery('.step-pane.active').children('form').eq(0);
    if (!form) return;
    //form.each(function (index, element) {
    //formvar to = $(element);
    //读取文件浏览器
    //form.find(".FileViewerKeyNames").each(function () {
    //    var mn = $(this).attr("id") + "_Trigger";
    //    eval(mn + "();");
    //});

    fileViewerUpload();

    if ($.trim(form.attr("data-readAcion")) && $.trim(form.attr("data-subformID"))) {
        var getUrl = $.trim(form.attr("data-readAcion")) + "?appid=" + appid + "&subformID=" + $.trim(form.attr("data-subformID"));
        $.ajax({
            url: getUrl,   // 提交的页面
            type: "GET",                   // 设置请求类型为"POST"，默认为"GET"
            cache: false,
            beforeSend: function () {
                //$("#loading").modal('show');
                //alert("startajax");
            },
            error: function (request) {      // 设置表单提交出错
                //$("#loading").modal('hide');
                alert("表单读取出错，请稍候再试或者联系管理员");
            },
            success: function (data) {
                //alert("endajax");
                for (var i in data) {
                    if (data[i] != undefined && data[i] != null) {
                        $("#" + i).val(data[i]);
                        $("#" + i).change();
                    }
                }
                //$("#loading").modal('hide');
            }
        });
    }

    //});
}

/*****加载文件上传*****/
function fileViewerUpload() {
    var form = jQuery('.step-pane.active').children('form')[0];
    if (!form) return;

    /*字段准备*/
    var f_appId = Utilities.getUrlParam("appid");
    var operation = Utilities.getUrlParam("operation");
    var isAdd = (operation == "1");
    var isEdit = (operation == "2" || operation == "4");
    var isReadOnly = (operation == "3");
    var fileKeys = "";

    var fileUploadUrl = jQuery(form).find(".FileViewerFileUploadUrl").first().attr("id");
    var fileGetUrl = jQuery(form).find(".FileViewerFileGetUrl").first().attr("id");
    var fileCheckSDUrl = jQuery(form).find(".FileViewerCheckSDUrl").first().attr("id");
    if (!fileUploadUrl)
        return;

    /*控制查看或者上传*/
    if (isAdd) {
        /*如果是ADD，直接开放上传，并且添加验证功能*/
        jQuery(form).find(".FileViewerKeyNames").each(function () {
            var fileKey = jQuery(this).attr("id");
            fileKeys += fileKey + ",";
            var href = fileUploadUrl + "?appid=" + f_appId + "&fileType=" + fileKey + "&tm=" + new Date();
            eval(fileKey + "_ViewOrEdit(href, true, f_appId)");
        });
    }
    else if (isEdit) {
        jQuery(form).find(".FileViewerKeyNames").each(function () {
            var fileKey = jQuery(this).attr("id");
            fileKeys += fileKey + ",";
        });
        /*如果是Edit，即补件，需要验证该文件类型是否需要补件*/
        jQuery.ajax({
            type: "post",
            url: fileCheckSDUrl,
            data: { appId: f_appId, fileType: fileKeys },
            success: function (msgCheck) {
                if (msgCheck) {
                    for (var jsonKey in msgCheck) {
                        var jsonVal = msgCheck[jsonKey];
                        if (jsonVal) {
                            /*如果需要补件*/
                            if (jsonVal > 0) {
                                var href = fileUploadUrl + "?appid=" + f_appId + "&fileType=" + jsonKey + "&IsSDStatud=true&NrListId=" + jsonVal + "&tm=" + new Date();
                                eval(jsonKey + "_ViewOrEdit(href, true, f_appId)");
                            }
                                /*不需要补件*/
                            else {
                                var href = fileUploadUrl + "?readOnly=1&appid=" + f_appId + "&fileType=" + jsonKey + "&tm=" + new Date();
                                eval(jsonKey + "_ViewOrEdit(href, false, f_appId)");
                            }
                        }
                    }
                }
            }
        });
    }
    else if (isReadOnly) {
        /*只读状态下打开的上传窗口为只读*/
        jQuery(form).find(".FileViewerKeyNames").each(function () {
            var fileKey = jQuery(this).attr("id");
            fileKeys += fileKey + ",";
            var href = fileUploadUrl + "?readOnly=1&appid=" + f_appId + "&fileType=" + fileKey + "&tm=" + new Date();
            eval(fileKey + "_ViewOrEdit(href, false, f_appId)");
        });
    }

    /*load exists*/
    var aryKeys = fileKeys.split(',');
    jQuery.ajax({
        type: "post",
        url: fileGetUrl,
        data: { appId: f_appId, FileType: fileKeys },
        success: function (msg) {
            if (msg.Status) {
                for (var k = 0; k < aryKeys.length; k++) {
                    if (aryKeys[k]) {
                        var ary = jQuery.grep(msg.ReturnObj, function (node) {
                            return node.flType.toLowerCase() == aryKeys[k].toLowerCase();
                        });
                        eval(aryKeys[k] + "_showExistsBy(ary, f_appId)");
                    }
                }
            } else {
                for (var k = 0; k < aryKeys.length; k++) {
                    if (aryKeys[k])
                        eval(aryKeys[k] + "_showExistsBy(null, f_appId)");
                }
            }
        },
        error: function () {
            for (var k = 0; k < aryKeys.length; k++) {
                if (aryKeys[k])
                    eval(aryKeys[k] + "_showExistsBy(null, f_appId)");
            }
        }
    });

    /*修改特殊的必传文件数目*/
    if (isAdd) {
        var logo = Utilities.getUrlParam('dformCode');
        /**
         * 若为商易贷，则没有以下相关逻辑-具体见需求#1567描述
         */
        if (logo !== 'productCodesyd') {
            var dic = DFormAction.GetSpecialFileAmount(f_appId);
            if (dic) {
                for (var dKey in dic) {
                    if (dic.hasOwnProperty(dKey)) {
                        var dVal = dic[dKey];
                        if (dVal) {
                            /* 改变dKey控件的最少上传文件数量为dVal[1] */
                            var element = jQuery('#' + dKey + '_hidden');
                            if (element.length > 0) {
                                element.attr('data-val-range-min', dVal[1]);
                                var rule = element.rules('remove');
                                rule.min = parseInt(dVal[1]);
                                element.rules('add', rule);
                            }

                            /**
                             * 如果征信渠道(dVal[0])逻辑-需求#1521：
                             * 海尔-> 1.0信用报告 2.0信用报告 非必传
                             * 其他-> 1.0信用报告 2.0信用报告 二选一
                             * 此功能前提条件需要配置FileView控件的选择性上传和数量，以及必填
                             */
                            if (dVal[0] === 'CREDIT_CHANNEL_HAIR') {
                                var pbocv1 = jQuery('#Pbocv1_hidden');
                                var pbocv2 = jQuery('#Pbocv2_hidden');
                                if (pbocv1.length > 0) {
                                    $('#Pbocv1_legend .icon-asterisk').hide();
                                    pbocv1.rules('remove');
                                }
                                if (pbocv2.length > 0) {
                                    $('#Pbocv2_legend .icon-asterisk').hide();
                                    pbocv2.rules('remove');
                                }

                            }
                            else {
                                var pbocv1 = jQuery('#Pbocv1_hidden');
                                var pbocv2 = jQuery('#Pbocv2_hidden');
                                var r = { required: true };

                                if (pbocv1.length > 0) {
                                    pbocv1.rules('add', r);
                                }
                                if (pbocv2.length > 0) {
                                    pbocv2.rules('add', r);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /*判断通话详单，Pboc，公积金，网银是否必传*/
    if (isAdd) {
        //var flag = DFormAction.HasMobileHistory(f_appId);
        //if (flag) {
        //    var mobileHistoryEle = jQuery('#fileTypeMobileHistforMobileHist_hidden');
        //    $('#fileTypeMobileHistforMobileHist_legend .icon-asterisk').hide();
        //    if (mobileHistoryEle.length > 0) {
        //        mobileHistoryEle.rules('remove');
        //    }
        //}
        var flags = DFormAction.GetGenesisStatus(f_appId);
        if (flags.MobileStatus) {
            removeRules('fileTypeMobileHistforMobileHist');
        }
        if (flags.PbocStatus) {
            removeRules('Pbocv1');
            removeRules('Pbocv2');
        }
        if (flags.NetbankStatus) {
            //工资卡incomeSalaryCard 储蓄卡incomeSavingsCard
            //获取分组
            var incomeGroup = $('#incomeSalaryCard_optionalGroup');
            if (incomeGroup.length <= 0)
                incomeGroup = $('#incomeSavingsCard_optionalGroup');
            if (incomeGroup.length > 0) {
                var groupVal = incomeGroup.data('group');
                if (groupVal != null && groupVal.trim() !== '') {
                    var gList = groupVal.trim().split(',');
                    $(gList).each(function (index, ele) {
                        removeRules(ele);
                    });
                }
            }
        }
        //公积金incomeAccFund
        if (flags.FundStatus) {
            var fundGroup = $('#incomeAccFund_optionalGroup');
            if (fundGroup.length > 0) {
                var val = fundGroup.data('group');
                if (val != null && val.trim() !== '') {
                    var l = val.trim().split(',');
                    $(l).each(function (index, ele) {
                        removeRules(ele);
                    });
                }
            }
        }
    }
};

function removeRules(id) {
    var element = jQuery('#' + id + '_hidden');
    $('#' + id + '_legend .icon-asterisk').hide();
    if (element.length > 0) {
        element.rules('remove');
    }
}

function bindbeforeunload() {
    if (Utilities.getUrlParam("operation") == 1) {
        //当关闭的时候 提示并且保存
        $(window).bind('beforeunload', function (event) {
            event.stopPropagation();
            return '您输入的内容尚未保存，确定离开此页面吗？';
        });
    }

}