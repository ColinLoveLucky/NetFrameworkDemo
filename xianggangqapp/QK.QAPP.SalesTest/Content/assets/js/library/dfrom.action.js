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
        var s = $(this).val();
        if (s == 'marrageCouple') {
            var contact1 = $('#CONTACT_RELATIVE1-RELATIONSHIP');
            if (contact1.val() == '' && contact1.children('[value=relationshipRelCon]')) {
                contact1.val('relationshipRelCon');
            }
        }
    }
    ,
    LoanValue: null,
    GetLoanValue: function (p_appId) {
        if (DFormAction.LoanValue != null) {
            return DFormAction.LoanValue;
        }
        else {
            //TODO AJAX 同步获取值
            $.ajax({
                url: "/ProductApplication/GetApplyAmount",
                data: {appId:p_appId},
                async: false,
                success: function (d) {
                    DFormAction.LoanValue = d;
                }
            });
            return DFormAction.LoanValue;
        }
    }

};

//注册事件
$(function () {
    $('#CustomerIDNumber').change(DFormAction.LoadGenderAndDate);

    //婚姻状态若为已婚，则家庭联系人1默认选配偶
    $('#MarrageStatus').change(DFormAction.CheckMarrageStatus);

    var f_appId = Utilities.getUrlParam("appid");
    DFormAction.GetLoanValue(f_appId);
    var f_operation = Utilities.getUrlParam("operation");
    if (f_operation == "1") {
        jQuery('#nr_memo_from_decision').parents('fieldset').hide();
    } else {
        jQuery('#nr_memo_from_decision').attr("disabled", "disabled");
    }
});