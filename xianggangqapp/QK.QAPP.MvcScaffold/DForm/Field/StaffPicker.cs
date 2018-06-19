using Antlr3.ST;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QK.QAPP.MvcScaffold.DForm
{
    /// <summary>
    /// 员工选择控件
    /// </summary>
    public class StaffPicker : AFieldType
    {
        public override string StringTemplatePath
        { 
            get { return GlobalSetting.DFormPath + @"\StaffPicker.html"; } 
        }

        /// <summary>
        /// 目标员工所属角色Id
        /// </summary>
        [DisplayName("目标员工所属角色Id（视图v_org_role_user中的RoleCode）")]
        public string RoleId { get; set; }

        /// <summary>
        /// 目标员工所属公司ID
        /// </summary>
        [DisplayName("目标员工所属公司ID（视图v_org_role_user中的某公司对应的ObjectId）")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 是否多选
        /// </summary>
        [DisplayName("是否多选（true：能选多人；false：只能选择一人）")]
        public string MultiSelect { get; set; }

        /// <summary>
        /// 员工代码匹配字段
        /// </summary>
        [DisplayName("员工代码匹配字段（视图v_org_role_user中匹配ObjectValue）")]
        public string Mapper_StaffCode { get; set; }

        /// <summary>
        /// 员工名匹配字段
        /// </summary>
        [DisplayName("员工名匹配字段（视图v_org_role_user中匹配ObjectName）")]
        public string Mapper_StaffName { get; set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        [DisplayName("是否只读(1-是,0-否)")]
        public bool IsReadOnly { get; set; }

        public StaffPicker(long id)
            : base(id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var dic = service.GetFieldAttr(id);
            if (dic.ContainsKey("TargetRoleId"))
            {
                this.RoleId = dic["TargetRoleId"] + "";
            }
            if (dic.ContainsKey("MultiSelect"))
            {
                this.MultiSelect = dic["MultiSelect"] + "";
            }
            if (dic.ContainsKey("CompanyId"))
            {
                this.MultiSelect = dic["CompanyId"] + "";
            }
            if (dic.ContainsKey("Mapper_StaffCode"))
            {
                this.Mapper_StaffCode = dic["Mapper_StaffCode"] + "";
            }
            if (dic.ContainsKey("Mapper_StaffName"))
            {
                this.Mapper_StaffName = dic["Mapper_StaffName"] + "";
            }
            if (dic.ContainsKey("IsReadOnly"))
            {
                this.IsReadOnly = dic["IsReadOnly"].ToBoolean();
            }
            else
            {
                this.IsReadOnly = false;
            }

            //this.CompanyId = Ioc.GetService<IQFUserService>().GetCurrentUser().CompanyId;
        }

        public override string GetHTML(bool readOnly)
        {
            var template = FileReader.GetTmpString(this.StringTemplatePath);
            StringTemplate st = new StringTemplate(template);
            st.SetAttribute("FieldType", this);
            st.SetAttribute("ReadOnly", readOnly);

            return st.ToString();
        }

        public override bool Vaidate(string value, out string error)
        {
            var errorTemp = GlobalSetting.DFormErrorTemp;
            error = "";
            //判断必填
            if (this.Field_Required)
            {
                if (string.IsNullOrEmpty(value))
                {
                    error += string.Format(errorTemp, this.Field_Group, this.Field_DisplayName, "不能为空");
                    return false;
                }
            }
            return true;
        }
    }
}
