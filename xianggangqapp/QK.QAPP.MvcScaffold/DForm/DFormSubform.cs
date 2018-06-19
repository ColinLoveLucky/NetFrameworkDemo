/*********************
 * 作    者:刘成帅
 * 创建时间:2014/9/9
 * 功    能:子表单类，可以通过构造通过数据库生成相应对象
**********************/
using Antlr3.ST;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.MvcScaffold.DForm
{
    /// <summary>
    /// 子表单类，可以通过构造通过数据库生成相应对象
    /// </summary>
    public class DFormSubform
    {
        public string StringTemplatePath = GlobalSetting.DFormPath + @"\DFromContainer\FieldContainer.html";
        public string StringTemplatePath_AddMore = GlobalSetting.DFormPath + @"\DFromContainer\FieldContainer_AddMore.html";
        /// <summary>
        /// 子表单ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 子表单名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表单提交位置（编辑）
        /// </summary>
        public string Action_Edit { get; set; }
        /// <summary>
        /// 表单提交位置
        /// </summary>
        public string Action_Read { get; set; }
        /// <summary>
        /// 是否允许Readonly
        /// </summary>
        public bool Readonly { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否为一对多关系
        /// </summary>
        public bool AddMore { get; set; }
        /// <summary>
        /// 添加更多key
        /// </summary>
        public string AddMoreKey { get; set; }
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<AFieldType> FieldList { get; set; }
        /// <summary>
        /// 构造函数 通过ID获取子表单
        /// </summary>
        /// <param name="id">子表单ID</param>
        public DFormSubform(long id, ENUM_FormOperation operation)
        {
            var dbReader = Ioc.GetService<DFormDBReader>();
            var entity = dbReader.GetSubFormByID(id);
            switch (operation)
            {
                case ENUM_FormOperation.ADD:
                    Readonly = false;
                    break;
                case ENUM_FormOperation.EDIT:
                    Readonly = entity.READONLY.ToBoolean();
                    break;
                case ENUM_FormOperation.READONLY:
                    Readonly = true;
                    break;
                default:
                    break;
            }

            this.ID = entity.ID;
            this.Action_Edit = entity.ACTION_EDIT;
            this.Action_Read = entity.ACTION_READ;
            this.Name = entity.NAME;
            this.Sort = entity.Sort.HasValue ? entity.Sort.Value : -1;
            this.AddMore = entity.ADDMORE.ToBoolean();
            this.AddMoreKey = entity.ADDMOREKEYWORD;
            this.FieldList = new List<AFieldType>();
            var filedlist = dbReader.GetFieldList(entity);
            foreach (var item in filedlist)
            {
                FieldList.Add(FieldTypeFactory.CreateFieldType(item.FIELD_TYPE, item.ID));
            }
            this.FieldList = this.FieldList.OrderBy(c => c.Field_Sort).ToList();
        }

        /// <summary>
        /// 用来数据返回的HTML 方便模板引擎使用
        /// </summary>
        public string FieldListHTML
        {
            get
            {
                List<GroupReturn> obj = new List<GroupReturn>();
                var path = this.AddMore ? this.StringTemplatePath_AddMore : this.StringTemplatePath;
                var template = FileReader.GetTmpString(path);
                StringTemplate st = new StringTemplate(template);

                var fieldGroup = FieldList.OrderBy(c => c.Field_Sort).Select(c => c.Field_Group).Distinct().ToList();

                foreach (var item in fieldGroup)
                {
                    var retEntity = new GroupReturn();
                    retEntity.GroupName = item;
                    retEntity.GroupHTML = FieldTypeFactory.FieldJoiner(FieldList.Where(c => c.Field_Group == item).ToList(), this.Readonly);
                    retEntity.GroupSort = FieldList.Where(c => c.Field_Group == item).OrderByDescending(c => c.Field_Sort).FirstOrDefault().Field_Sort;
                    obj.Add(retEntity);
                }
                st.SetAttribute("Group", obj.OrderBy(c=>c.GroupSort));
                st.SetAttribute("thisObj", this);
                return st.ToString();
            }
        }
    }

    /// <summary>
    /// 用来数据返回的实体 方便模板引擎使用
    /// </summary>
    public class GroupReturn
    {
        public string GroupName { get; set; }
        public string GroupHTML { get; set; }
        public int GroupSort { get; set; }
    }
}
