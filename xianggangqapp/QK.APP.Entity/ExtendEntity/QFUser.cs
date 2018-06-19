using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 用户对象
    /// </summary>
    [Serializable]
    public class QFUser
    {
        private string _UserId = null;
        /// <summary>
        /// 用户主键
        /// </summary>
        /// <returns></returns>
        [Description("用户主键")]
        public string UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                this._UserId = value;
            }
        }
        private string _Code = null;
        /// <summary>
        /// 用户编号
        /// </summary>
        /// <returns></returns>
        [Description("用户编号")]
        public string Code
        {
            get
            {
                return this._Code;
            }
            set
            {
                this._Code = value;
            }
        }
        private string _Account = null;
        /// <summary>
        /// 登录账户
        /// </summary>
        /// <returns></returns>
        [Description("登录账户")]
        public string Account
        {
            get
            {
                return this._Account;
            }
            set
            {
                this._Account = value;
            }
        }
        private string _Password = null;
        /// <summary>
        /// 登录密码
        /// </summary>
        /// <returns></returns>
        [Description("登录密码")]
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }
        private string _Secretkey = null;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Description("")]
        public string Secretkey
        {
            get
            {
                return this._Secretkey;
            }
            set
            {
                this._Secretkey = value;
            }
        }
        private string _RealName = null;
        /// <summary>
        /// 真实姓名
        /// </summary>
        /// <returns></returns>
        [Description("真实姓名")]
        public string RealName
        {
            get
            {
                return this._RealName;
            }
            set
            {
                this._RealName = value;
            }
        }
        private string _Spell = null;
        /// <summary>
        /// 真实姓名拼音
        /// </summary>
        /// <returns></returns>
        [Description("真实姓名拼音")]
        public string Spell
        {
            get
            {
                return this._Spell;
            }
            set
            {
                this._Spell = value;
            }
        }
        private string _Alias = null;
        /// <summary>
        /// 别名
        /// </summary>
        /// <returns></returns>
        [Description("别名")]
        public string Alias
        {
            get
            {
                return this._Alias;
            }
            set
            {
                this._Alias = value;
            }
        }
        private string _RoleId = null;
        /// <summary>
        /// 默认角色
        /// </summary>
        /// <returns></returns>
        [Description("默认角色")]
        public string RoleId
        {
            get
            {
                return this._RoleId;
            }
            set
            {
                this._RoleId = value;
            }
        }
        private string _Gender = null;
        /// <summary>
        /// 性别
        /// </summary>
        /// <returns></returns>
        [Description("性别")]
        public string Gender
        {
            get
            {
                return this._Gender;
            }
            set
            {
                this._Gender = value;
            }
        }
        private string _Mobile = null;
        /// <summary>
        /// 手机号码
        /// </summary>
        /// <returns></returns>
        [Description("手机号码")]
        public string Mobile
        {
            get
            {
                return this._Mobile;
            }
            set
            {
                this._Mobile = value;
            }
        }
        private string _Telephone = null;
        /// <summary>
        /// 固定电话
        /// </summary>
        /// <returns></returns>
        [Description("固定电话")]
        public string Telephone
        {
            get
            {
                return this._Telephone;
            }
            set
            {
                this._Telephone = value;
            }
        }
        private DateTime? _Birthday = null;
        /// <summary>
        /// 出生日期
        /// </summary>
        /// <returns></returns>
        [Description("出生日期")]
        public DateTime? Birthday
        {
            get
            {
                return this._Birthday;
            }
            set
            {
                this._Birthday = value;
            }
        }
        private string _Email = null;
        /// <summary>
        /// 电子邮件
        /// </summary>
        /// <returns></returns>
        [Description("电子邮件")]
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = value;
            }
        }
        private string _OICQ = null;
        /// <summary>
        /// QQ号码
        /// </summary>
        /// <returns></returns>
        [Description("QQ号码")]
        public string OICQ
        {
            get
            {
                return this._OICQ;
            }
            set
            {
                this._OICQ = value;
            }
        }
        private string _DutyId = null;
        /// <summary>
        /// 岗位
        /// </summary>
        /// <returns></returns>
        [Description("岗位")]
        public string DutyId
        {
            get
            {
                return this._DutyId;
            }
            set
            {
                this._DutyId = value;
            }
        }
        private string _TitleId = null;
        /// <summary>
        /// 职称
        /// </summary>
        /// <returns></returns>
        [Description("职称")]
        public string TitleId
        {
            get
            {
                return this._TitleId;
            }
            set
            {
                this._TitleId = value;
            }
        }
        private string _CompanyId = null;
        /// <summary>
        /// 公司主键
        /// </summary>
        /// <returns></returns>
        [Description("公司主键")]
        public string CompanyId
        {
            get
            {
                return this._CompanyId;
            }
            set
            {
                this._CompanyId = value;
            }
        }
        private string _DepartmentId = null;
        /// <summary>
        /// 部门主键
        /// </summary>
        /// <returns></returns>
        [Description("部门主键")]
        public string DepartmentId
        {
            get
            {
                return this._DepartmentId;
            }
            set
            {
                this._DepartmentId = value;
            }
        }
        private string _WorkgroupId = null;
        /// <summary>
        /// 工作组主键
        /// </summary>
        /// <returns></returns>
        [Description("工作组主键")]
        public string WorkgroupId
        {
            get
            {
                return this._WorkgroupId;
            }
            set
            {
                this._WorkgroupId = value;
            }
        }
        private string _Description = null;
        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        [Description("描述")]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
            }
        }
        private string _SecurityLevel = null;
        /// <summary>
        /// 安全级别
        /// </summary>
        /// <returns></returns>
        [Description("安全级别")]
        public string SecurityLevel
        {
            get
            {
                return this._SecurityLevel;
            }
            set
            {
                this._SecurityLevel = value;
            }
        }
        private DateTime? _ChangePasswordDate = null;
        /// <summary>
        /// 最后修改密码日期
        /// </summary>
        /// <returns></returns>
        [Description("最后修改密码日期")]
        public DateTime? ChangePasswordDate
        {
            get
            {
                return this._ChangePasswordDate;
            }
            set
            {
                this._ChangePasswordDate = value;
            }
        }
        private int? _OpenId = null;
        /// <summary>
        /// 单点登录标识
        /// </summary>
        /// <returns></returns>
        [Description("单点登录标识")]
        public int? OpenId
        {
            get
            {
                return this._OpenId;
            }
            set
            {
                this._OpenId = value;
            }
        }
        private string _IPAddress = null;
        /// <summary>
        /// IP地址
        /// </summary>
        /// <returns></returns>
        [Description("IP地址")]
        public string IPAddress
        {
            get
            {
                return this._IPAddress;
            }
            set
            {
                this._IPAddress = value;
            }
        }
        private string _MACAddress = null;
        /// <summary>
        /// MAC地址
        /// </summary>
        /// <returns></returns>
        [Description("MAC地址")]
        public string MACAddress
        {
            get
            {
                return this._MACAddress;
            }
            set
            {
                this._MACAddress = value;
            }
        }
        private int? _LogOnCount = null;
        /// <summary>
        /// 登录次数
        /// </summary>
        /// <returns></returns>
        [Description("登录次数")]
        public int? LogOnCount
        {
            get
            {
                return this._LogOnCount;
            }
            set
            {
                this._LogOnCount = value;
            }
        }
        private DateTime? _FirstVisit = null;
        /// <summary>
        /// 第一次访问时间
        /// </summary>
        /// <returns></returns>
        [Description("第一次访问时间")]
        public DateTime? FirstVisit
        {
            get
            {
                return this._FirstVisit;
            }
            set
            {
                this._FirstVisit = value;
            }
        }
        private DateTime? _PreviousVisit = null;
        /// <summary>
        /// 上一次访问时间
        /// </summary>
        /// <returns></returns>
        [Description("上一次访问时间")]
        public DateTime? PreviousVisit
        {
            get
            {
                return this._PreviousVisit;
            }
            set
            {
                this._PreviousVisit = value;
            }
        }
        private DateTime? _LastVisit = null;
        /// <summary>
        /// 最后访问时间
        /// </summary>
        /// <returns></returns>
        [Description("最后访问时间")]
        public DateTime? LastVisit
        {
            get
            {
                return this._LastVisit;
            }
            set
            {
                this._LastVisit = value;
            }
        }
        private int? _Enabled = null;
        /// <summary>
        /// 有效：1-有效，0-无效
        /// </summary>
        /// <returns></returns>
        [Description("有效：1-有效，0-无效")]
        public int? Enabled
        {
            get
            {
                return this._Enabled;
            }
            set
            {
                this._Enabled = value;
            }
        }
        private int? _SortCode = null;
        /// <summary>
        /// 排序吗
        /// </summary>
        /// <returns></returns>
        [Description("排序吗")]
        public int? SortCode
        {
            get
            {
                return this._SortCode;
            }
            set
            {
                this._SortCode = value;
            }
        }
        private int? _DeleteMark = null;
        /// <summary>
        /// 删除标记:1-正常，0-删除
        /// </summary>
        /// <returns></returns>
        [Description("删除标记:1-正常，0-删除")]
        public int? DeleteMark
        {
            get
            {
                return this._DeleteMark;
            }
            set
            {
                this._DeleteMark = value;
            }
        }
        private DateTime? _CreateDate = null;
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Description("创建时间")]
        public DateTime? CreateDate
        {
            get
            {
                return this._CreateDate;
            }
            set
            {
                this._CreateDate = value;
            }
        }
        private string _CreateUserId = null;
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [Description("创建用户主键")]
        public string CreateUserId
        {
            get
            {
                return this._CreateUserId;
            }
            set
            {
                this._CreateUserId = value;
            }
        }
        private string _CreateUserName = null;
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Description("创建用户")]
        public string CreateUserName
        {
            get
            {
                return this._CreateUserName;
            }
            set
            {
                this._CreateUserName = value;
            }
        }
        private DateTime? _ModifyDate = null;
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Description("修改时间")]
        public DateTime? ModifyDate
        {
            get
            {
                return this._ModifyDate;
            }
            set
            {
                this._ModifyDate = value;
            }
        }
        private string _ModifyUserId = null;
        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        [Description("修改用户主键")]
        public string ModifyUserId
        {
            get
            {
                return this._ModifyUserId;
            }
            set
            {
                this._ModifyUserId = value;
            }
        }
        private string _ModifyUserName = null;
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Description("修改用户")]
        public string ModifyUserName
        {
            get
            {
                return this._ModifyUserName;
            }
            set
            {
                this._ModifyUserName = value;
            }
        }
        private int? _FromAD = null;
        /// <summary>
        /// 是否来自域账号导入 1-是 0-否
        /// </summary>
        /// <returns></returns>
        [Description("是否来自域账号导入 1-是 0-否")]
        public int? FromAD
        {
            get
            {
                return this._FromAD;
            }
            set
            {
                this._FromAD = value;
            }
        }

        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string TitleName { get; set; }

        /// <summary>
        /// 进件城市
        /// </summary>
        public APP_CITY City { get; set; }

        public List<string> DataPermission { get; set; }

        public List<QFRole> RoleList { get; set; }

        public QFUserAuth DataPermissionOrg { get; set; }

    }
    [Serializable]
    public class QFRole
    {
        /// <summary>
        /// 角色主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 角色编码    
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 角色名字
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        ///// <summary>
        ///// 角色类型
        ///// </summary>
        //public string Category { get; set; }
    }

    [Serializable]
    public class QFUserAuth
    {
        public bool IsSelectAll { get; set; }

        public List<string> AccountList { get; set; }

        public List<string> ParentIdList { get; set; }

        public List<string> CompanyList { get; set; }
    }

}
