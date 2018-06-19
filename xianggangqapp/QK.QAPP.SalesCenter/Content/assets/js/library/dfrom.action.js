//表单行为函数
DFormAction = {
    //根据身份证信息加载生日和性别
    LoadGenderAndDate: function () {
        var cardID = $(this).val();
        var year, month, day, gender;
        var dateFormat = 'yyyy/mm/dd';

        //15位身份证号
        if (cardID.length == 15) {
            //出生日期
            year = '19' + cardID.substr(6, 2);
            month = cardID.substr(8, 2);
            day = cardID.substr(10, 2);

            dateFormat = dateFormat.replace('mm', month);
            dateFormat = dateFormat.replace('dd', day);
            dateFormat = dateFormat.replace('yyyy', year);

            $('#CustomerBirthday').val(dateFormat);

            //性别
            gender = cardID.substr(14, 1);

            if (gender % 2 == 0) {
                $('#CustomerGender').val('genderF');
            } else {
                $('#CustomerGender').val('genderM');
            }
        } else if (cardID.length == 18) {
            //出生日期
            year = cardID.substr(6, 4);
            month = cardID.substr(10, 2);
            day = cardID.substr(12, 2);

            dateFormat = dateFormat.replace('mm', month);
            dateFormat = dateFormat.replace('dd', day);
            dateFormat = dateFormat.replace('yyyy', year);

            $('#CustomerBirthday').val(dateFormat);

            //性别
            gender = cardID.substr(16, 1);

            if (gender % 2 == 0) {
                $('#CustomerGender').val('genderF');
            } else {
                $('#CustomerGender').val('genderM');
            }
        } else {
            $('#CustomerBirthday').val('');
            $('#CustomerGender').val('');
        }
    },

    //将标签分组
    GroupByTab: function (fieldsetObj) {
        var tab = [];
        //var Group = $("#" + div_ID + ">form>fieldset");
        var Group = fieldsetObj;
        for (var i = 0; i < Group.length; i++) {
            Group.eq(i).children(".form-group").wrapAll("<div class='tabbable'></div>");
            Group.eq(i).children().children(".form-group").wrapAll("<div class='tab-content'></div>");
            var tagObj = [];
            Group.eq(i).children().children().children(".form-group").each(function () {
                tagObj.push({
                    name: $(this).find(".dropdown-toggle").text(),
                    OBJ: $(this)
                })
            });
            var ulHTML = "";
            ulHTML += '<ul class="nav nav-tabs">';
            for (var j = 0; j < tagObj.length; j++) {
                ulHTML += '<li class="nav nav-tabs' + (j == 0 ? ' active' : '') + '">';
                ulHTML += '   <a data-toggle="tab" href="#' + tagObj[j].name + '">';
                ulHTML += tagObj[j].name;
                ulHTML += '   </a>';
                ulHTML += '</li>';
                tagObj[j].OBJ.addClass("tab-pane");
                tagObj[j].OBJ.attr("id", tagObj[j].name);
                if (j == 0) {
                    tagObj[j].OBJ.addClass("active");
                }
            }
            ulHTML += '</ul>';
            Group.eq(i).find(".tabbable>.tab-content").before(ulHTML);

        }
    },

    //婚姻状态
    CheckMarrageStatus: function () {
        //婚姻状态-已婚
        var s1 = 'marrageCouple';
        //人员关系-配偶
        var s2 = 'relationshipRelCon';
        //当前控件的值
        var s = $(this).val();
        var contact1 = $('#CONTACT_RELATIVE1-RELATIONSHIP');
        var contact1_name = $('#CONTACT_RELATIVE1-APP_CONTACT_NAME');
        var contact1_mobile = $('#CONTACT_RELATIVE1-APP_CONTACT_MOBILE');
        /* 验证规则 */
        var r = { required: true, __dummy__: true };
        /* 初始化全局变量 ContactOptions1 */
        if (!GlobalConfig.ContactOptions1) {
            GlobalConfig.ContactOptions1 = contact1.children().map(function () {
                return this;
            });
        }
        if (Utilities.IsCheDai()) {
            //车贷，如果是已婚只能选配偶
            /* --只读+hidden 方式
            var optionDic = contact1.children(); //.map(function () { return this })
            if (s === s1) {
                optionDic.filter(':not([value=' + s2 + '])').attr('hidden', 'hidden');
                contact1.val(s2);
                // 设置只读 
                Utilities.SetSelectReadOnly(contact1);
            } else {
                optionDic.removeAttr('hidden');
                // 取消只读 
                Utilities.CancelSelectReadOnly(contact1);
            }
            */

            /* --remove option节点方式 */
            contact1.empty();
            if (s === s1) {
                GlobalConfig.ContactOptions1.filter('[value=' + s2 + ']').appendTo(contact1);
            } else {
                GlobalConfig.ContactOptions1.appendTo(contact1);
            }

        } else if (Utilities.IsHouse()) {

            //房贷，如果是已婚只能选配偶
            contact1.empty();
            if (s === s1) {
                GlobalConfig.ContactOptions1.filter('[value=' + s2 + ']').appendTo(contact1);
                /* 选择 是，添加必填验证*/
                contact1_name.rules('add', r);
                contact1_mobile.rules('add', r);
                contact1.rules('add', r);
                /* 显示星号 */
                contact1.parent().prev().children('.icon-asterisk').show();
                contact1_name.parent().prev().children('.icon-asterisk').show();
                contact1_mobile.parent().prev().children('.icon-asterisk').show();
            } else {
                GlobalConfig.ContactOptions1.appendTo(contact1);
                /* 选择 否 取消必填验证*/
                contact1_name.rules('remove');
                contact1_mobile.rules('remove');
                contact1.rules('remove');
                /* 隐藏星号 */
                contact1.parent().prev().children('.icon-asterisk').hide();
                contact1_name.parent().prev().children('.icon-asterisk').hide();
                contact1_mobile.parent().prev().children('.icon-asterisk').hide();

            }
        } else {
            //非车贷，如果是已婚默认选择配偶，但不做强求
            if (s === s1) {
                if (contact1.val() == '' && contact1.children('[value=' + s2 + ']')) {
                    contact1.val(s2);
                }
            }
        }
    },
    LoanValue: null,
    GetLoanValue: function (p_appId) {
        if (DFormAction.LoanValue != null) {
            return DFormAction.LoanValue;
        }
        else {
            //TODO AJAX 同步获取值
            $.ajax({
                url: "/ProductApplication/GetApplyAmount",
                data: { appId: p_appId },
                async: false,
                success: function (d) {
                    DFormAction.LoanValue = d;
                }
            });
            return DFormAction.LoanValue;
        }
    },
    //驾照持有人
    CheckDrivingLicenseOwner: function () {
        var v = $(this).val();
        //驾照持有人选项中司机的值
        var s1A = 'driverOwnerDri';
        //驾照持有人选项中子女的值
        var s1B = 'driverOwnerChildren';
        //人员关系中司机的值
        var s2A = 'relationshipRelDri';
        //人员关系中子女的值
        var s2B = 'relationshipRelKid';
        var contact2 = $('#CONTACT_RELATIVE2-RELATIONSHIP');
        /* --只读+hidden 方式
        var optionDic = contact2.children();
        if (v === s1A) {
            optionDic.filter('[value=' + s2A + ']').removeAttr('hidden').siblings().attr('hidden', 'hidden');
            contact2.val(s2A);
        } else if (v === s1B) {
            optionDic.filter('[value=' + s2B + ']').removeAttr('hidden').siblings().attr('hidden', 'hidden');
            contact2.val(s2B);
            // 设置只读 
            Utilities.SetSelectReadOnly(contact2);
        } else {
            optionDic.removeAttr('hidden');
            // 取消只读 
            Utilities.CancelSelectReadOnly(contact2);
        }
        */

        /* --remove option节点方式 */
        /* 初始化全局变量 ContactOptions1 */
        if (!GlobalConfig.ContactOptions2) {
            GlobalConfig.ContactOptions2 = contact2.children().map(function () {
                return this;
            });
        }

        contact2.empty();
        if (v === s1A) {
            GlobalConfig.ContactOptions2.filter('[value=' + s2A + ']').appendTo(contact2);
        } else if (v === s1B) {
            GlobalConfig.ContactOptions2.filter('[value=' + s2B + ']').appendTo(contact2);
        } else {
            GlobalConfig.ContactOptions2.appendTo(contact2);
        }
    },
    SpecialFileAmount: null,
    GetSpecialFileAmount: function (p_appId) {
        if (DFormAction.SpecialFileAmount != null) {
            return DFormAction.SpecialFileAmount;
        }
        else {
            //TODO AJAX 同步获取值
            $.ajax({
                url: "/Fileupload/GetSpecialFileAmount",
                type: "POST",
                data: { appId: p_appId },
                async: false,
                success: function (d) {
                    DFormAction.SpecialFileAmount = d;
                }
            });
            return DFormAction.SpecialFileAmount;
        }
    },
    /* 车贷进件 名下是否有车 与 车辆品牌 和 车辆型号 是否必填的处理 */
    HasCarRequiredHandle: function () {
        var v = $(this).val();
        /* 自有车辆品牌 */
        var brandOwned = $('#BrandOwned');
        /* 自有车辆型号 */
        var modelOwned = $('#ModelOwned');
        /* 验证规则 */
        var r = { required: true, __dummy__: true };
        var o = Utilities.getUrlParam("operation");
        if (o != '3') {
            if (v == 'CONSTANTS_YES') {
                /* 选择 是，添加必填验证*/
                brandOwned.rules('add', r);
                modelOwned.rules('add', r);
                /* 启用 */
                brandOwned.removeAttr('disabled');
                modelOwned.removeAttr('disabled');
                /* 显示星号 */
                brandOwned.parent().prev().children('.icon-asterisk').show();
                modelOwned.parent().prev().children('.icon-asterisk').show();

            } else {
                /* 选择 否 取消必填验证*/
                brandOwned.rules('remove');
                modelOwned.rules('remove');
                /* 禁用 */
                brandOwned.attr('disabled', 'disabled');
                modelOwned.attr('disabled', 'disabled');
                /* 隐藏星号 */
                brandOwned.parent().prev().children('.icon-asterisk').hide();
                modelOwned.parent().prev().children('.icon-asterisk').hide();
            }
        }
    },
    MobileHistoryFlag: null,
    HasMobileHistory: function (appId) {
        if (DFormAction.MobileHistoryFlag != null) {
            return DFormAction.MobileHistoryFlag;
        }

        $.ajax({
            url: '/Fileupload/HasMobileHistory',
            type: "GET",
            data: { appId: appId },
            async: false,
            success: function (d) {
                if (d === 'True') {
                    DFormAction.MobileHistoryFlag = true;
                } else {
                    DFormAction.MobileHistoryFlag = false;
                }
            }
        });

        return DFormAction.MobileHistoryFlag;
    },
    GenesisStatus: null,
    GetGenesisStatus:function(appId) {
        if (DFormAction.GenesisStatus != null) {
            return DFormAction.GenesisStatus;
        }
        $.ajax({
            url:'/Fileupload/GenesisStatus',
            type:"GET",
            data:{appId:appId},
            async:false,
            success:function(d) {
                DFormAction.GenesisStatus = {};
                DFormAction.GenesisStatus.MobileStatus = d.MobileStatus;
                DFormAction.GenesisStatus.PbocStatus = d.PbocStatus;
                DFormAction.GenesisStatus.NetbankStatus = d.NetbankStatus;
                DFormAction.GenesisStatus.FundStatus = d.FundStatus;
            }
        });

        return DFormAction.GenesisStatus;
    },
    /**
     * 客群类型与公司名称的处理（极客贷）
     */
    CustomerGroupHandle: function (e) {
        var v = $(this).val();
        var comName = $('#COM_NAME');
        var comCode = $('#COM_NAME_AEOCode');
        var comType = $('#COM_NAME_AEOType');
        var comBtn = $('#COM_NAME_SelectButton');
        //优良职业客户
        if (v === 'customerExcellent') {
            //用户触发，清空公司名称信息
            if (e && !e.isTrigger) {
                comName.val('');
                comCode.val('');
                comType.show().removeAttr('disabled');
                comBtn.show();
                comName.attr('readonly', 'readonly');
                comCode.removeAttr('disabled');
            }
            
        }
            //一般客户 或 按揭房产客户
        else if (v === 'customerGeneral'
            || v === 'customerMortgage') {
            //用户触发，清空公司名称信息
            //if (e && !e.isTrigger) {
                //comName.val('');
                //comCode.val('');
            //}
            comType.hide().attr('disabled','disabled');
            comBtn.hide();
            comName.removeAttr('readonly');
            comCode.attr('disabled','disabled');
        }
    }
};

//注册事件
$(function () {
    $('#CustomerIDNumber').change(DFormAction.LoadGenderAndDate);

    //婚姻状态若为已婚，则家庭联系人1默认选配偶
    $('#MarrageStatus').change(DFormAction.CheckMarrageStatus);

    /* 车贷表单事件 */
    if (Utilities.IsCheDai()) {
        /* 驾照持有人若为司机/子女，则家庭联系人2默认选择司机/子女 */
        $('#DrivingLicenseOwner').change(DFormAction.CheckDrivingLicenseOwner);

        /* 名下若有车，则 车辆品牌 和 车辆型号 必填，否则选填 */
        $('#HasCar').change(DFormAction.HasCarRequiredHandle);
    }

    /**
     * 极客贷表单事件
     * 客群类型选择 作用与 公司名称
     */
    if (Utilities.IsGeek()) {
        $('#CustomerGroup').change(DFormAction.CustomerGroupHandle);
    }

    var f_appId = Utilities.getUrlParam("appid");
    DFormAction.GetLoanValue(f_appId);
    var f_operation = Utilities.getUrlParam("operation");
    if (f_operation == "1") {
        jQuery('#nr_memo_from_decision').parents('fieldset').hide();
    } else {
        jQuery('#nr_memo_from_decision').attr("disabled", "disabled");
    }
});