using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 进件状态枚举
    /// </summary>
    public enum EnterStatusType
    {
        /// <summary>
        /// 反欺诈确认
        /// </summary>
        FRAUDOK = 0,

        /// <summary>
        /// 初审待补件
        /// </summary>
        SDAPPRWT,

        /// <summary>
        /// 初审补件中
        /// </summary>
        SDAPPRING,

        /// <summary>
        /// 初审补件完成
        /// </summary>
        SDAPPROK,

        /// <summary>
        /// 录入待分配
        /// </summary>
        ENTRYDISPWT,

        /// <summary>
        /// 等待录入
        /// </summary>
        ENTRYDISPED,

        /// <summary>
        /// 录入中
        /// </summary>
        ENTRYING,

        /// <summary>
        /// 初审待分配
        /// </summary>
        ENTRYOK,

        /// <summary>
        /// 等待初审
        /// </summary>
        APPRDISPED,

        /// <summary>
        /// 等待初审
        /// </summary>
        FRAUDBACK,

        /// <summary>
        /// 初审中
        /// </summary>
        APPRING,

        /// <summary>
        /// 初审完成
        /// </summary>
        APPROK,

        /// <summary>
        /// 终审待分配
        /// </summary>
        APPRDEC,

        /// <summary>
        /// 终审待分配
        /// </summary>
        FAPPRDISPWT,

        /// <summary>
        /// 终审已分配
        /// </summary>
        FAPPRDISPED,

        /// <summary>
        /// 终审中
        /// </summary>
        FAPPRING,

        /// <summary>
        /// 终审拒绝
        /// </summary>
        FAPPRDEC,

        /// <summary>
        /// 终审通过
        /// </summary>
        FAPPROK,


        /// <summary>
        /// 已签合同
        /// </summary>
        CONTRACT,

        /// <summary>
        /// 已放款
        /// </summary>
        MADELOAN,

        /// <summary>
        /// 录入待补件
        /// </summary>
        SDENTRYWT,

        /// <summary>
        /// 录入补件中
        /// </summary>
        SDENTRYING,

        /// <summary>
        /// 等待录入
        /// </summary>
        SDENTRYOK,

        /// <summary>
        /// 录入补件超时
        /// </summary>
        SDENTRYOT,

        /// <summary>
        /// 初审补件超时
        /// </summary>
        SDAPPROT,

        /// <summary>
        /// 反欺诈待分配
        /// </summary>
        FRAUDDISPWT,

        /// <summary>
        /// 反欺诈待分配
        /// </summary>
        FFRAUDDISPWT,

        /// <summary>
        /// 等待反欺诈
        /// </summary>
        FFRAUDDISPED,

        /// <summary>
        /// 等待反欺诈
        /// </summary>
        FRAUDDISPED,

        /// <summary>
        /// 反欺诈处理中
        /// </summary>
        FRAUDING,

        /// <summary>
        /// 等待终审
        /// </summary>
        FFRAUDBACK,

        /// <summary>
        /// 终审反欺诈处理中
        /// </summary>
        FFRAUDING,

        /// <summary>
        /// 终审反欺诈已完成
        /// </summary>
        FFRAUDOK,

        /// <summary>
        /// 申请未提交
        /// </summary>
        PENDING,

        /// <summary>
        /// 申请已提交
        /// </summary>
        SUBMIT,

        /// <summary>
        /// 超时系统自动取消
        /// </summary>
        SYSCANCEL,

        /// <summary>
        /// 协议确认
        /// </summary>
        CONTRACT_CONFIRM,

        /// <summary>
        ///  废弃
        /// </summary>
        DISUSED,

        /// <summary>
        /// 已完成放款
        /// </summary>
        MADELOANCOMPLETED,

        /// <summary>
        /// 进件超时废弃
        /// </summary>
        SYSDISUSED,

        /// <summary>
        /// 流标
        /// </summary>
        BIDFAIL,

        /// <summary>
        /// 录入复核
        /// 2015-05-14
        ENTRYCHK,

        /// <summary>
        /// 车贷评估用户取消
        /// </summary>
        CARASSESSCANCEL,

        /// <summary>
        /// 车贷评估用户完成
        /// </summary>
        CARASSESSOK,

        /// <summary>
        /// 车贷待评估
        /// </summary>
        CARASSESSWT,

        /// <summary>
        /// 车贷初审待补件
        /// </summary>
        CARAPPRWT,
        /// <summary>
        /// 零售信贷专员待补件
        /// </summary>
        CARCAPPRWT,
        /// <summary>
        /// 贷后管理专员待补件
        /// </summary>
        CARPLMSAPPRWT,
        /// <summary>
        /// 车贷初审补件完成
        /// </summary>
        CARAPPROK,
        /// <summary>
        /// 零售信贷专员补件完成
        /// </summary>
        CARCAPPROK,
        /// <summary>
        /// 贷后管理专员补件完成
        /// </summary>
        CARPLMSAPPROK,

        /// <summary>
        /// 等待评估主管
        /// </summary>
        CARASSESSUPERWT,
        
        /// <summary>
        /// 评估主管评估中
        /// </summary>
        CARASSESSUPERING,

        /// <summary>
        /// 评估主管完成
        /// </summary>
        CARASSESSUPEROK,

        /// <summary>
        /// 初审反欺诈待处理
        /// </summary>
        FRAUDWT,

        /// <summary>
        /// 终审反欺诈待处理
        /// </summary>
        FFRAUDWT,

        /// <summary>
        /// 初审待分配
        /// </summary>
        APPRDISPWT,

        /// <summary>
        /// 初审取消
        /// </summary>
        APPRCANCEL,

        /// <summary>
        /// 初审拒绝
        /// </summary>
        APPRREJECT,

        /// <summary>
        /// 零售信贷员审批待分配
        /// </summary>
        RCOAAPPRWT,

        /// <summary>
        /// 零售信贷员审批已分配
        /// </summary>
        RCOAAPPRDISED,

        /// <summary>
        /// 零售信贷员审批中
        /// </summary>
        RCOAAPPRING,

        /// <summary>
        /// 零售信贷审批员已分配
        /// </summary>
        RCAPPRDISPED,

        /// <summary>
        /// 零售信贷审批员审批中
        /// </summary>
        RCAPPRING,

        /// <summary>
        /// 零售信贷审批员审批拒绝
        /// </summary>
        RCAPPRREJ,

        /// <summary>
        /// 零售信贷审批员审批通过
        /// </summary>
        RCAPPROK,

        /// <summary>
        /// 零售信贷审批员审批取消
        /// </summary>
        RCAPPRCANCEL,

        /// <summary>
        /// 零售信贷审批员审批回退
        /// </summary>
        RCAPPRBACK,

        /// <summary>
        /// 零售信贷员回退
        /// </summary>
        RCOAAPPRBACK,

        /// <summary>
        /// 零售信贷员审批取消
        /// </summary>
        RCOAAPPRCANCEL,

        /// <summary>
        /// 实地调查待分配
        /// </summary>
        FIELDINVESTWT,

        /// <summary>
        /// 实地调查已分配
        /// </summary>
        FIELDINVESTDISED,

        /// <summary>
        /// 实地调查中
        /// </summary>
        FIELDINVESTING,

        /// <summary>
        /// 实地调查完成
        /// </summary>
        FIELDINVESTOK,

        /// <summary>
        /// 贷后管理专员待分配
        /// </summary>
        PLMSAPPRWT,

        /// <summary>
        /// 贷后管理专员已分配
        /// </summary>
        PLMSAPPRDISED,

        /// <summary>
        /// 贷后管理专员审批中
        /// </summary>
        PLMSAPPRING,

        /// <summary>
        /// 贷后管理专员审批取消
        /// </summary>
        PLMSAPPRCANCEL,

        /// <summary>
        /// 贷后管理审批员已分配
        /// </summary>
        PLMAAPPRDISPED,

        /// <summary>
        /// 贷后管理审批员审批中
        /// </summary>
        PLMAAPPRING,

        /// <summary>
        /// 贷后管理审批员审批拒绝
        /// </summary>
        PLMAAPPRREJ,

        /// <summary>
        /// 贷后管理审批员审批通过
        /// </summary>
        PLMAAPPROK,

        /// <summary>
        /// 贷后管理审批员审批取消
        /// </summary>
        PLMAAPPRCANCEL,

        /// <summary>
        /// 贷后管理审批员审批回退
        /// </summary>
        PLMAAPPRBACK,

        /// <summary>
        /// 房贷初审待分配
        /// </summary>
        HOUSEAPPRDISPWT,

        /// <summary>
        /// 房贷等待初审
        /// </summary>
        HOUSEAPPRDISPED,

        /// <summary>
        /// 房贷初审中
        /// </summary>
        HOUSEAPPRING,

        /// <summary>
        /// 房贷初审待补件
        /// </summary>
        HOUSESDAPPRWT,

        /// <summary>
        /// 房贷初审补件完成
        /// </summary>
        HOUSESDAPPROK,

        /// <summary>
        /// 房贷初审完成
        /// </summary>
        HOUSEAPPROK,

        /// <summary>
        /// 房贷初审拒绝
        /// </summary>
        HOUSEAPPRDEC,

        /// <summary>
        /// 房贷初审取消
        /// </summary>
        HOUSEAPPRCANCLE,

        /// <summary>
        /// 房贷终审待分配
        /// </summary>
        HOUSEFAPPRDISPWT,

        /// <summary>
        /// 房贷等待终审
        /// </summary>
        HOUSEFAPPRDISPED,

        /// <summary>
        /// 房贷终审中
        /// </summary>
        HOUSEFAPPRING,

        /// <summary>
        /// 房贷终审待补件
        /// </summary>
        HOUSESDFAPPRWT,

        /// <summary>
        /// 房贷终审补件补件完成
        /// </summary>
        HOUSESDFAPPROK,

        /// <summary>
        /// 房贷终审通过
        /// </summary>
        HOUSEFAPPROK,

        /// <summary>
        /// 房贷终审拒绝
        /// </summary>
        HOUSEFAPPRDEC,

        /// <summary>
        /// 房贷终审取消
        /// </summary>
        HOUSEFAPPRCANCLE,

        /// <summary>
        /// 零售信贷员审批回退
        /// </summary>
        PLMSAPPRBACK,

        /// <summary>
        /// 北银待签约
        /// </summary>
        BY_AFAPPROK,

        /// <summary>
        /// 夸客待挂标
        /// </summary>
        BY_JFAPPROK,

        /// <summary>
        /// 协议确认待放款
        /// </summary>
        BY_APPRDEC,

        /// <summary>
        /// 渠道已放款
        /// </summary>
        BY_MADELOAN,
        
        /// <summary>
        /// 夸客已放款
        /// </summary>
        BY_QFMADELOAN,

        /// <summary>
        /// 协议超时自动取消
        /// </summary>
        BY_SYSCANCEL,

        /// <summary>
        /// 北银审批中
        /// </summary>
        BY_FAPPRING,

        /// <summary>
        /// 客户取消协议
        /// </summary>
        BY_CUSCANCEL,

        /// <summary>
        /// 风控取消协议
        /// </summary>
        BY_RISKCANCEL,

        /// <summary>
        /// 零售信贷员一审待分配
        /// </summary>
        RCOAAPPRWTFIRST,

        /// <summary>
        /// 零售信贷员一审已分配
        /// </summary>
        RCOAAPPRDISEDFIRST,

        /// <summary>
        /// 零售信贷员一审中
        /// </summary>
        RCOAAPPRINGFIRST,

        /// <summary>
        /// 评估中
        /// </summary>
        CARASSESSING,

        /// <summary>
        /// 零售信贷员二审待审核
        /// </summary>
        RCOAAPPRWTSECOND,

        /// <summary>
        /// 零售信贷员二审中
        /// </summary>
        RCOAAPPRINGSECOND,

        /// <summary>
        /// 零售信贷员一审拒绝
        /// </summary>
        RCOAAPPRREJFIRST,

        /// <summary>
        /// DarkBlue通过
        /// </summary>
        DARKBLUEOK,

        /// <summary>
        /// 评估已分配
        /// </summary>
        CARASSESSDISEDWT,

        /// <summary>
        /// DES自动拒贷
        /// </summary>
        AUTO_DEC,

        /// <summary>
        /// 车贷录入待补件（录入补件状态）
        /// </summary>
        CARENTRYPATCH,

        /// <summary>
        /// 车贷录入中（录入补件后续状态）
        /// </summary>
        CARENTRYING,

        /// <summary>
        /// 车贷录入已分配
        /// </summary>
        CARENTRYDISPED
    }
    /// <summary>
    /// 预申请进件状态
    /// </summary>
    public enum PreEnterStatusType
    {
        /// <summary>
        /// 预申请未提交
        /// </summary>
        PRE_PENDING = 0,

        /// <summary>
        /// 预申请审核中
        /// </summary>
        PRE_APPRING,

        /// <summary>
        /// 预申请通过
        /// </summary>
        PRE_APPROK,

        /// <summary>
        /// 预申请未通过
        /// </summary>
        PRE_APPRDEC,

        /// <summary>
        /// 预申请废弃
        /// </summary>
        PRE_DISUSED

    } 
}
