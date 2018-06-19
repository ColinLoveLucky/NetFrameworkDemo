using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers.LabelPact
{
    public class PactController : Controller
    {
        public IBID_LabelPactService bidLabelPactService { get; set; }
        public IQFUserService QFUserHelper { get; set; }
        public IAPP_CITYSERVICE cityService { get; set; }
        public IQuotaBidDicConfigService QuotaBidDicConfigService { get; set; }
        public IBID_ContractService ContractService { get; set; }
        public IQBUploadService qbUploadService { get; set; }
        public IBID_PactHistoryService pactHistoryService { get; set; }
        public IAPP_MAINSERVICE appMainService { get; set; }
        public IQuotaManageService quotaManageService { get; set; }

        #region  协议上传
        /// <summary>
        /// 协议上传列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PactList()
        {
            ViewBag.SBid_Step_CollectBid = BidStatusQueryType.Bid_Step_CollectBid;
            ViewBag.SBS_XY_YSC = BidStatusQueryType.BS_XY_YSC;
            ViewBag.SBS_XY_WSC = BidStatusQueryType.BS_XY_WSC;
            return View("~/Views/LabelPact/Manage/PactList.cshtml");
        }
        /// <summary>
        /// 获取上传协议列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPactList()
        {
            /*
             * fuzzySearch为0或1，查询列表按条件搜索框进行精确查询或模糊查询；
             * GlobalSearch = 0为精确查询；
             * GlobalSearch = 1为模糊查询；
             */
            BidListQueryPara queryPara = new BidListQueryPara();
            QFUserAuth userAuth = QFUserHelper.GetUserAuth();
            //权限
            queryPara.IsSelectAll = userAuth.IsSelectAll;
            queryPara.AccountList = Serializer.ListToString(userAuth.AccountList, ",");
            queryPara.ParentIdList = userAuth.ParentIdList.JoinString(",");
            queryPara.CompanyList = userAuth.CompanyList.JoinString(",");

            queryPara.CurrentPage = int.Parse(Request["page"] ?? "0");
            queryPara.PageSize = int.Parse(Request["rows"] ?? "0");
            queryPara.GlobalSearch = 0;//默认精确查询
            if (Request["bidStep"] != null)
            {
                queryPara.BidStep = Request["bidStep"].ToString();
            }
            if (Request["bidState"] != null)
            {
                queryPara.BidState = Request["bidState"].ToString();
            }
            if (Request["fuzzySearch"] != null)
            {
                if (Request["fuzzySearch"].ToString() == "0")
                {
                    queryPara.GlobalSearch = 0;
                    queryPara.BidAppCode = HttpUtility.UrlDecode(Request["bidAppCode"]);
                    queryPara.BidCode = HttpUtility.UrlDecode(Request["bidCode"]);
                    queryPara.BidContractNo = HttpUtility.UrlDecode(Request["bidContractNo"]);
                    queryPara.BidCustomerName = HttpUtility.UrlDecode(Request["bidCustomerName"]);
                    queryPara.BidCustomerManager = HttpUtility.UrlDecode(Request["bidCustomerManager"]);
                    queryPara.BidAgreementState = HttpUtility.UrlDecode(Request["bidAgreementState"]);

                }
                else
                {
                    queryPara.GlobalSearch = 1;
                    queryPara.SearchMsg = HttpUtility.UrlDecode(Request["searchPara"]);
                }
            }
            PageData<QB_V_BIDLIST> bidlist = bidLabelPactService.GetBIDInfoList(queryPara);
            return Json(bidlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 取消挂标操作（更新状态为已划标）
        /// </summary>
        /// <param name="bidCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelHangBid(string bidCode)
        {
            if (string.IsNullOrEmpty(bidCode))
                return Json(false);
            var user = QFUserHelper.GetCurrentUser();
            BidOperateRequest bidOperate = new BidOperateRequest();
            bidOperate.BidList = bidCode;
            bidOperate.UserCode = user.Account;
            bidOperate.UserName = user.RealName;
            var resultMsg = bidLabelPactService.CancelHangBid(bidOperate);
            string msg = string.Empty;
            if (resultMsg != null)
            {
                List<APP_MAIN> appmainList = new List<APP_MAIN>();
                try
                {

                    resultMsg.ForEach(f =>
                    {
                        msg += f.BidCode + ":" + f.TipMsg + ";  ";
                        #region 成功取消挂标后要将appmain的CONTRACT_IS_CREATE=="Y"合同是否全部生成成功更新为null值
                        if (f.Sucess)
                        {
                            var bidInfo = bidLabelPactService.GetBidDetail(f.BidCode);
                            if (bidInfo != null)
                            {
                                var appmain = appMainService.FirstOrDefault(o => o.APP_CODE == bidInfo.BID_APP_CODE && o.CONTRACT_IS_CREATE == "Y");
                                if (appmain != null)
                                {
                                    appmain.CONTRACT_IS_CREATE = null;
                                    appmainList.Add(appmain);
                                }
                            }
                        }
                        #endregion

                    });
                    if (appmainList.Count > 0)//取消挂标后要将appmain的CONTRACT_IS_CREATE=="Y"合同是否全部生成成功更新为null值
                    {
                        appMainService.UpdateMultiple(appmainList);
                        appMainService.UnitOfWork.SaveChanges();
                        Infrastructure.Log4Net.LogWriter.Biz("取消挂标操作-更新appmain表CONTRACT_IS_CREATE成功", bidCode, resultMsg);
                    }

                }
                catch (Exception ex)
                {

                    Infrastructure.Log4Net.LogWriter.Error("取消挂标操作", ex); ;
                }
            }
            else
            {
                msg = "取消挂标失败接口请求异常！";
            }
            Infrastructure.Log4Net.LogWriter.Biz("取消挂标操作", bidCode, resultMsg);
            return Json(msg);
        }
        /// <summary>
        /// 协议上传详情页
        /// </summary>
        /// <param name="bidCode"></param>
        /// <param name="logo"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public ActionResult PactUpload(string bidCode, string logo, string appid)
        {
            if (string.IsNullOrWhiteSpace(bidCode))
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议上传详情非法访问，bidCode为空！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            BidDetailRequest bidDetailR = new BidDetailRequest();
            bidDetailR.BidIdentifier = bidCode;
            #region 验证数据权限  return new RedirectResult("/Home/NoAuthorization");
            //权限
            QFUserAuth userAuth = QFUserHelper.GetUserAuth();
            bidDetailR.IsSelectAll = userAuth.IsSelectAll;
            bidDetailR.AccountList = Serializer.ListToString(userAuth.AccountList, ",");
            bidDetailR.ParentIdList = userAuth.ParentIdList.JoinString(",");
            bidDetailR.CompanyList = userAuth.CompanyList.JoinString(",");
            var bidInfo = bidLabelPactService.GetBidDetailJson(bidDetailR);
            //var bidInfo = bidLabelPactService.GetBidDetailJson(bidCode);
            var dicBidInfo = Serializer.FromJson<Dictionary<string, string>>(bidInfo);
            QB_V_BID_DETAIL bid = Serializer.FromJson<QB_V_BID_DETAIL>(bidInfo);

            if (dicBidInfo == null)
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议上传详情非法访问，未找到相关数据！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            if (GlobalSetting.PACTDETAIL_ORDER_STATUS.Count != 0)
            {
                if (!GlobalSetting.PACTDETAIL_ORDER_STATUS["PACTUPLOAD"].Contains(dicBidInfo["BID_STATE"]))
                {
                    Infrastructure.Log4Net.LogWriter.Biz("协议上传详情非法访问，协议不在可以上传的状态！");
                    return new RedirectResult("/Home/NoAuthorization");
                }
            }
            if (string.IsNullOrWhiteSpace(Request.QueryString["operation"]))
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议上传详情非法访问，operation参数缺失！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            else if (GlobalSetting.PACTDETAIL_OPERATION_STATUS.Count != 0)
            {
                if (!GlobalSetting.PACTDETAIL_OPERATION_STATUS["BS_XY_WSC"].Contains(Request.QueryString["operation"]))
                {
                    Infrastructure.Log4Net.LogWriter.Biz("协议上传详情非法访问，协议不在可以上传添加的状态！");
                    return new RedirectResult("/Home/NoAuthorization");
                }
            }
            if (dicBidInfo["BID_BUS_LOGO"] != logo || dicBidInfo["BID_APP_ID"] != appid)
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议上传详情非法访问，未找到BID_BUS_LOGO,BID_APP_ID相关数据！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            #endregion
            Global.ContractGlobalConfig cgc = new Global.ContractGlobalConfig();
            //当前用户对象
            QFUser currentUser = QFUserHelper.GetCurrentUser();

            //CA申请返回信息
            var caMsg = "";
            if (!string.IsNullOrEmpty(bid.BID_CA_RESULT) && bid.BID_CA_RESULT.ToUpper() != "SUCCESS")
            {
                caMsg = bid.BID_CA_RESULT;
            }
            ViewData["bidCode"] = bidCode;
            ViewData["bidContractNo"] = dicBidInfo["BID_CONTRACT_NO"];
            ViewData["bidAppCode"] = dicBidInfo["BID_APP_CODE"];
            ViewData["pactPrintType"] = Global.GlobalSetting.Contract_PRINT_TYPE;//合同打印显示打印类型配置，E_MANUAL-手动电子签章,P_MANUAL-手动签字,P_NON_SEAL-空的签章
            ViewData["camsg"] = caMsg;
            ViewBag.CreateBidInfo = bidLabelPactService.CreateBidDetailInfo(logo, appid, dicBidInfo);
            
            ContractCreateRequest conRequest=new ContractCreateRequest();
            bool isCreate = ContractService.CreateC(bid, cgc, currentUser, out conRequest);
            if (isCreate)
            {
                ContractService.ContractCreate(conRequest, bid, cgc, currentUser, isCreate);//生成合同
            }
            return View("~/Views/LabelPact/Manage/PactUpload.cshtml");
        }
        /// <summary>
        /// 协议上传提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PactUploadSubmit(string bidCode)
        {
            if (string.IsNullOrEmpty(bidCode))
                return Json(false);
            var resultMsg = bidLabelPactService.AgreementUpload(bidCode);
            Infrastructure.Log4Net.LogWriter.Biz("协议提交", bidCode, resultMsg);
            return Json(new List<BidMatchTip>() { resultMsg });
        }
        #endregion

        #region  协议打印
        /// <summary>
        /// 协议打印
        /// </summary>
        /// <returns></returns>
        public JsonResult PactPrint(string bidContractNo, string bidAppCode)
        {
            var fileLog = new QBFileLogTemp();
            var result = qbUploadService.GetFileList(bidContractNo, bidAppCode, "", out fileLog);
            Infrastructure.Log4Net.LogWriter.Biz("打印合同申请号：" + bidAppCode, bidContractNo + "", fileLog);
            //防止合同还没有生成完，点击打印获取不到合同的情况，加入以下标识，用于前端提示
            string conIsCreate = appMainService.Find(f => f.APP_CODE == bidAppCode).FirstOrDefault().CONTRACT_IS_CREATE;
            return Json(new { result, conIsCreate }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  协议确认
        /// <summary>
        /// 协议确认列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PactConfirmList()
        {
            ViewData["Bid_Channel"] = QuotaBidDicConfigService.GetQbDicType("BidApplyType").ToDictionary(k => k.LEVEL_CODE, k => k.DIC_REMARK);
            ViewData["Bid_City"] = cityService.GetBidCityList().ToDictionary(k => k.CITY_CODE, v => v.CITY_NAME);
            ViewBag.SBS_XY_YSC = BidStatusQueryType.BS_XY_YSC;
            ViewBag.SBS_XY_YQR = BidStatusQueryType.BS_XY_YQR;
            return View("~/Views/LabelPact/Manage/PactConfirmList.cshtml");
        }
        /// <summary>
        /// 协议确认列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPactConfirmList()
        {
            /*
             * fuzzySearch为0或1，查询列表按条件搜索框进行精确查询或模糊查询；
             * GlobalSearch = 0为精确查询；
             * GlobalSearch = 1为模糊查询；
             */
            BidListQueryPara queryPara = new BidListQueryPara();
            queryPara.CurrentPage = int.Parse(Request["page"] ?? "0");
            queryPara.PageSize = int.Parse(Request["rows"] ?? "0");
            queryPara.GlobalSearch = 0;//默认精确查询
            //权限
            QFUserAuth userAuth = QFUserHelper.GetUserAuth();
            queryPara.IsSelectAll = userAuth.IsSelectAll;
            queryPara.AccountList = Serializer.ListToString(userAuth.AccountList, ",");
            queryPara.ParentIdList = userAuth.ParentIdList.JoinString(",");
            queryPara.CompanyList = userAuth.CompanyList.JoinString(",");
            if (Request["bidStep"] != null)
            {
                queryPara.BidStep = Request["bidStep"].ToString();
            }
            if (Request["bidState"] != null)
            {
                queryPara.BidState = Request["bidState"].ToString();
            }
            if (Request["fuzzySearch"] != null)
            {
                if (Request["fuzzySearch"].ToString() == "0")
                {
                    queryPara.GlobalSearch = 0;
                    queryPara.BidAppCode = HttpUtility.UrlDecode(Request["bidAppCode"]);
                    queryPara.BidCode = HttpUtility.UrlDecode(Request["bidCode"]);
                    queryPara.BidContractNo = HttpUtility.UrlDecode(Request["bidContractNo"]);
                    queryPara.BidCustomerName = HttpUtility.UrlDecode(Request["bidCustomerName"]);
                    queryPara.BidCustomerManager = HttpUtility.UrlDecode(Request["bidCustomerManager"]);
                    queryPara.BidSignedTime = HttpUtility.UrlDecode(Request["bidSignedTime"]);
                    queryPara.BidChannel = HttpUtility.UrlDecode(Request["bidChannel"]);
                    queryPara.BidCityCode = HttpUtility.UrlDecode(Request["bidCityCode"]);

                }
                else
                {
                    queryPara.GlobalSearch = 1;
                    queryPara.SearchMsg = HttpUtility.UrlDecode(Request["searchPara"]);
                }
            }
            PageData<QB_V_BIDLIST> bidlist = bidLabelPactService.GetBIDInfoList(queryPara);
            return Json(bidlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 协议确认详情页
        /// </summary>
        /// <param name="bidCode"></param>
        /// <param name="logo"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public ActionResult PactConfirm(string bidCode, string logo, string appid)
        {
            if (string.IsNullOrWhiteSpace(bidCode))
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议确认详情非法访问，bidCode为空！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            BidDetailRequest bidDetailR = new BidDetailRequest();
            bidDetailR.BidIdentifier = bidCode;
            #region 验证数据权限  return new RedirectResult("/Home/NoAuthorization");
            //权限
            QFUserAuth userAuth = QFUserHelper.GetUserAuth();
            bidDetailR.IsSelectAll = userAuth.IsSelectAll;
            bidDetailR.AccountList = Serializer.ListToString(userAuth.AccountList, ",");
            bidDetailR.ParentIdList = userAuth.ParentIdList.JoinString(",");
            bidDetailR.CompanyList = userAuth.CompanyList.JoinString(",");
            //var bidInfo = Serializer.FromJson<Dictionary<string, string>>(bidLabelPactService.GetBidDetailJson(bidCode));
            var bidInfo = Serializer.FromJson<Dictionary<string, string>>(bidLabelPactService.GetBidDetailJson(bidDetailR));
            if (bidInfo == null)
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议确认详情非法访问，未找到相关数据！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            if (GlobalSetting.PACTDETAIL_ORDER_STATUS.Count != 0)
            {
                if (!GlobalSetting.PACTDETAIL_ORDER_STATUS["PACTCONFIRM"].Contains(bidInfo["BID_STATE"]))
                {
                    Infrastructure.Log4Net.LogWriter.Biz("协议确认详情非法访问，协议不在可以确认的状态！");
                    return new RedirectResult("/Home/NoAuthorization");
                }
            }
            if (string.IsNullOrWhiteSpace(Request.QueryString["operation"]))
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议确认详情非法访问，operation参数缺失！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            else if (GlobalSetting.PACTDETAIL_OPERATION_STATUS.Count != 0)
            {
                if (!GlobalSetting.PACTDETAIL_OPERATION_STATUS["BS_XY_YSC"].Contains(Request.QueryString["operation"]))
                {
                    Infrastructure.Log4Net.LogWriter.Biz("协议确认详情非法访问，协议不在可以查看的状态！");
                    return new RedirectResult("/Home/NoAuthorization");
                }
            }
            if (bidInfo["BID_BUS_LOGO"] != logo || bidInfo["BID_APP_ID"] != appid)
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议确认详情非法访问，未找到BID_BUS_LOGO,BID_APP_ID相关数据！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            #endregion
            ViewData["Bid_Reject_Reason"] = QuotaBidDicConfigService.GetRejectReason();
            ViewData["bidCode"] = bidCode;
            ViewData["bidContractNo"] = bidInfo["BID_CONTRACT_NO"];
            ViewData["bidAppCode"] = bidInfo["BID_APP_CODE"];
            ViewBag.CreateBidInfo = bidLabelPactService.CreateBidDetailInfo(logo, appid, bidInfo);
            return View("~/Views/LabelPact/Manage/PactConfirm.cshtml");
        }
        /// <summary>
        /// 协议确认
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PactConfirmSubmit(string bidCode)
        {
            if (string.IsNullOrEmpty(bidCode))
                return Json(false);
            var user = QFUserHelper.GetCurrentUser();
            BidOperateRequest bidOperate = new BidOperateRequest();
            bidOperate.BidList = bidCode;
            bidOperate.UserCode = user.Account;
            var resultMsg = bidLabelPactService.AgreementConfirm(bidOperate);
            if (resultMsg != null)
            {
                //协议确认历史记录
                ContractHistoryRequest historyList = new ContractHistoryRequest();
                List<ContractHistoryRequestType> history = new List<ContractHistoryRequestType>();
                resultMsg.ForEach(f =>
                {

                    if (f.Sucess)
                    {
                        QB_V_BID_DETAIL bidInfo = bidLabelPactService.GetBidDetail(f.BidCode);
                        history.Add(new ContractHistoryRequestType() { BidCode = f.BidCode, ContractCode = bidInfo.BID_CONTRACT_NO, UserCode = user.Account, UserName = user.RealName, DeptCode = user.DepartmentId, DeptName = user.DepartmentName, ContractOptype = ContractOPTYPE.Contract_OP_CONFIRM });
                    }

                });
                historyList.ContractHistory = history;
                pactHistoryService.AgreementAddHistory(historyList);
                Infrastructure.Log4Net.LogWriter.Biz("协议确认", "协议确认", resultMsg);
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议确认失败", "协议确认失败", resultMsg);

            }
            return Json(resultMsg);
        }
        /// <summary>
        /// 协议驳回
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PactReject(string bidCode, string rejectCode, string rejectRemark)
        {
            if (string.IsNullOrEmpty(bidCode))
                return Json(false);
            var user = QFUserHelper.GetCurrentUser();
            BidOperateRequest bidOperate = new BidOperateRequest();
            bidOperate.BidList = bidCode;
            bidOperate.UserCode = user.Account;
            bidOperate.RejectCode = rejectCode;
            bidOperate.RejectRemark = rejectRemark;
            var resultMsg = bidLabelPactService.AgreementReject(bidOperate);
            if (resultMsg != null)
            {
                //协议驳回历史记录
                ContractHistoryRequest historyList = new ContractHistoryRequest();
                List<ContractHistoryRequestType> history = new List<ContractHistoryRequestType>();
                var result = Serializer.FromJson<BidMatchTip>(resultMsg);
                if (result.Sucess)
                {
                    QB_V_BID_DETAIL bidInfo = bidLabelPactService.GetBidDetail(bidCode);
                    history.Add(new ContractHistoryRequestType() { BidCode = bidCode, ContractCode = bidInfo.BID_CONTRACT_NO, UserCode = user.Account, UserName = user.RealName, DeptCode = user.DepartmentId, DeptName = user.DepartmentName, ContractOptype = ContractOPTYPE.Contract_OP_REJECT });
                    historyList.ContractHistory = history;
                    pactHistoryService.AgreementAddHistory(historyList);
                }
            }
            Infrastructure.Log4Net.LogWriter.Biz("协议驳回", bidCode, resultMsg);
            return Json(resultMsg);
        }
        #endregion

        #region 标的详情
        /// <summary>
        /// 协议详情页
        /// </summary>
        /// <param name="bidCode"></param>
        /// <param name="logo"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public ActionResult PactDetail(string bidCode, string logo, string appid)
        {
            if (string.IsNullOrWhiteSpace(bidCode))
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议详情非法访问，bidCode为空！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            BidDetailRequest bidDetailR = new BidDetailRequest();
            bidDetailR.BidIdentifier = bidCode;
            #region 验证数据权限  return new RedirectResult("/Home/NoAuthorization");
            //权限
            QFUserAuth userAuth = QFUserHelper.GetUserAuth();
            bidDetailR.IsSelectAll = userAuth.IsSelectAll;
            bidDetailR.AccountList = Serializer.ListToString(userAuth.AccountList, ",");
            bidDetailR.ParentIdList = userAuth.ParentIdList.JoinString(",");
            bidDetailR.CompanyList = userAuth.CompanyList.JoinString(",");
            //var bidInfo = Serializer.FromJson<Dictionary<string, string>>(bidLabelPactService.GetBidDetailJson(bidCode));
            var bidInfo = Serializer.FromJson<Dictionary<string, string>>(bidLabelPactService.GetBidDetailJson(bidDetailR));
            if (bidInfo == null)
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议详情非法访问，未找到相关数据！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            if (GlobalSetting.PACTDETAIL_ORDER_STATUS.Count != 0)
            {
                if (!GlobalSetting.PACTDETAIL_ORDER_STATUS["PACTDETAIL"].Contains(bidInfo["BID_STATE"]))
                {
                    Infrastructure.Log4Net.LogWriter.Biz("协议详情非法访问，协议不在可以查看的状态！");
                    return new RedirectResult("/Home/NoAuthorization");
                }
            }
            if (string.IsNullOrWhiteSpace(Request.QueryString["operation"]))
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议详情非法访问，协议不在可以查看或编辑的状态！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            else if (GlobalSetting.PACTDETAIL_OPERATION_STATUS.Count != 0)
            {
                if (!(GlobalSetting.PACTDETAIL_OPERATION_STATUS["BS_XY_YQR"].Contains(Request.QueryString["operation"]) || GlobalSetting.PACTDETAIL_OPERATION_STATUS["BS_PPWC"].Contains(Request.QueryString["operation"]) || GlobalSetting.PACTDETAIL_OPERATION_STATUS["BS_XY_YSC"].Contains(Request.QueryString["operation"])))
                {
                    Infrastructure.Log4Net.LogWriter.Biz("协议详情非法访问，协议不在可以查看或编辑的状态！");
                    return new RedirectResult("/Home/NoAuthorization");
                }
            }
            if (bidInfo["BID_BUS_LOGO"] != logo || bidInfo["BID_APP_ID"] != appid)
            {
                Infrastructure.Log4Net.LogWriter.Biz("协议详情非法访问，未找到BID_BUS_LOGO,BID_APP_ID相关数据！");
                return new RedirectResult("/Home/NoAuthorization");
            }

            #endregion
            ViewData["bidCode"] = bidCode;//标的编号
            ViewData["bidContractNo"] = bidInfo["BID_CONTRACT_NO"];//合同号
            ViewData["bidAppCode"] = bidInfo["BID_APP_CODE"];//进件编号
            ViewBag.CreateBidInfo = bidLabelPactService.CreateBidDetailInfo(logo, appid, bidInfo);
            return View("~/Views/LabelPact/Manage/PactDetail.cshtml");
        }
        #endregion

        #region 获取手动签章更新url
        public JsonResult GetUpdateUrl()
        {
            var bidContractNo = Request["bidContractNo"];
            var bidAppCode = Request["bidAppCode"];
            var fileId = Request["fileId"];
            QBFileLogTemp fileLog = new QBFileLogTemp();
            var result = qbUploadService.GetUpdateUrl(bidContractNo, bidAppCode, fileId, ref fileLog);
            Infrastructure.Log4Net.LogWriter.Biz("获取手动签章更新url，合同号为：" + bidContractNo, bidContractNo + "", fileLog);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /// <summary>
        /// 协议补录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PactAdditionalList()
        {
            ViewBag.SBS_XY_YQR = BidStatusQueryType.Bid_Step_Addtion;
            return View("~/Views/LabelPact/Manage/PactAdditionalList.cshtml");
        }

        public ActionResult PactConfig()
        {
            return View("~/Views/SystemConfig/PactConfig.cshtml");
        }
    }
}