var wizard;
var wizardObj;
//绑定表单提交
$(function () {
    var bidContractNo = $("#bidContractNo").val();
    var bidCode = $("#bidCode").val();
    var bidAppCode = $("#bidAppCode").val();
	//表单上一步下一步事件绑定
	var $validation = false;
	//绑定表单提交
	wizard = $('#btn_save');
	wizard.on('click', function (e, info, obj) {
		bindbeforeunload();
		var thisForm = $(".step-pane>form");
		var submitURL = $("#btn_save").attr("data-submitaction");
		var submitSucessURL = $("#btn_save").val();
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
			$.ajax({
				url: submitURL,   // 提交的页面
				data: { "bidCode": bidCode }, // 从表单中获取数据
				//async: false,
				type: "POST",                   // 设置请求类型为"POST"，默认为"GET"
				cache: false,
				beforeSend: function () {
					//$("#loading").modal('show');
				},
				error: function (request) {      // 设置表单提交出错
					//$("#loading").modal('hide');
					bootbox.alert("提交出错，请稍候再试或者联系管理员");
				},
				success: function (msg) {
				    if (msg[0].Sucess) {
				        //bootbox.alert(msg[0].TipMsg);
				     
				        window.location.href = submitSucessURL;
				    }
				    else {
				        if (msg[0].TipMsg != null) {
				            bootbox.alert(msg[0].TipMsg);
				        }
				        else {
				            bootbox.alert("提交失败！");
				        }
				    }
				}
			});


		}


	});
	fileViewerUpload();
});
$.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
	_title: function (title) {
		var $title = this.options.title || '&nbsp;'
		if (("title_html" in this.options) && this.options.title_html == true)
			title.html($title);
		else title.text($title);
	}
}));

/*****加载文件上传*****/
function fileViewerUpload() {
	var form = jQuery('.step-pane').children('form')[0];
	if (!form) return;
	/*字段准备*/
	var f_appId = Utilities.getUrlParam("appid");
	var operation = Utilities.getUrlParam("operation");
	var bidContractNo = jQuery("#bidContractNo").val();
	var bidAppCode = $("#bidAppCode").val();
	var isAdd = (operation == "1");
	var isEdit = (operation == "2");
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
			var href = fileUploadUrl + "?bidContractNo=" + bidContractNo + "&fileType=" + fileKey + "&bidAppCode=" + bidAppCode;
			eval(fileKey + "_ViewOrEdit(href, true, bidContractNo,bidAppCode)");
		});
	}
	else if (isEdit) {
		jQuery(form).find(".FileViewerKeyNames").each(function () {
			var fileKey = jQuery(this).attr("id");
			fileKeys += fileKey + ",";
			var href = fileUploadUrl + "?bidContractNo=" + bidContractNo + "&fileType=" + fileKey + "&IsAdditional=true&bidAppCode=" + bidAppCode;
			eval(fileKey + "_ViewOrEdit(href, true, bidContractNo,bidAppCode)");
		});
	}
	else if (isReadOnly) {
		/*只读状态下打开的上传窗口为只读*/
		jQuery(form).find(".FileViewerKeyNames").each(function () {
			var fileKey = jQuery(this).attr("id");
			fileKeys += fileKey + ",";
			var href = fileUploadUrl + "?readOnly=1&bidContractNo=" + bidContractNo + "&fileType=" + fileKey + "&bidAppCode=" + bidAppCode;
			eval(fileKey + "_ViewOrEdit(href, false, bidContractNo,bidAppCode)");
		});
	}

	/*load exists*/
	var aryKeys = fileKeys.split(',');
	jQuery.ajax({
		type: "post",
		url: fileGetUrl,
		data: { bidContractNo: bidContractNo,bidAppCode:bidAppCode, FileType: fileKeys },
		success: function (msg) {
			if (msg.Status) {
				for (var k = 0; k < aryKeys.length; k++) {
					if (aryKeys[k]) {
					    var ary = msg;
					    eval(aryKeys[k] + "_showExistsBy(ary, bidContractNo,bidAppCode)");
					}
				}
			} else {
				for (var k = 0; k < aryKeys.length; k++) {
					if (aryKeys[k])
					    eval(aryKeys[k] + "_showExistsBy(msg, bidContractNo,bidAppCode)");
				}
			}
		},
		error: function () {
			for (var k = 0; k < aryKeys.length; k++) {
				if (aryKeys[k])
				    eval(aryKeys[k] + "_showExistsBy(null, bidContractNo,bidAppCode)");
			}
		}
	});


};

function bindbeforeunload() {
	if (Utilities.getUrlParam("operation") == 1) {
		//当关闭的时候 提示并且保存
		$(window).bind('beforeunload', function (event) {
			event.stopPropagation();
			return '您输入的内容尚未保存，确定离开此页面吗？';
		});
	}

}