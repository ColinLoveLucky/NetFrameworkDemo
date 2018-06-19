/*********************
 * 作者：刘成帅
 * 时间：2014/9/10
 * 功能：字段基类，用来保存基本的信息
**********************/
using QK.QAPP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.MvcScaffold.DForm
{
    public abstract class AFieldType
    {
        public abstract string StringTemplatePath { get; }
        /// <summary>
        /// 字段的ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 字段提交的主键
        /// </summary>
        public string Field_Key { get; set; }
        /// <summary>
        /// 字段的值
        /// </summary>
        public string Field_DisplayName { get; set; }
        /// <summary>
        /// 占用列数
        /// </summary>
        public short Field_RowSpan { get; set; }
        /// <summary>
        /// 是否占用两行
        /// </summary>
        public bool Field_DoubleCol { get { return Field_RowSpan == 2; } }
        /// <summary>
        /// 字段对应表
        /// </summary>
        public string Mapper_Table { get; set; }
        /// <summary>
        /// 字段对应表字段
        /// </summary>
        public string Mapper_TableField { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string Field_Group { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public short Field_Sort { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Field_Required { get; set; }
        /// <summary>
        /// 是否禁用（不参与表单提交）
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 获取前端HTML
        /// </summary>
        /// <param name="readOnly">是否自读</param>
        /// <returns>HTML字符串</returns>
        public abstract string GetHTML(bool readOnly);
        /// <summary>
        /// 验证
        /// </summary>
        /// <returns>是否验证成功</returns>
        public abstract bool Vaidate(string value, out string error);

        /// <summary>
        /// 构造函数 通过字段ID初始化
        /// </summary>
        /// <param name="id"></param>
        public AFieldType(long id)
        {
            var service = Ioc.GetService<DFormDBReader>();
            var entity = service.GetFieldById(id);
            this.ID = id + "";
            this.Field_DisplayName = entity.FIELD_DISPLAYNAME;
            this.Field_Group = entity.FIELD_GROUP;
            this.Field_Key = entity.FIELD_KEY;
            this.Field_RowSpan = entity.FIELD_ROWSPAN.HasValue ? entity.FIELD_ROWSPAN.Value : (short)0;
            this.Field_Sort = entity.FIELD_SORT.HasValue ? entity.FIELD_SORT.Value : (short)0;
            this.Mapper_TableField = entity.MAPPER_TABLEFIELD;
            this.Mapper_Table = entity.MAPPER_TABLE;
            this.Field_Required = entity.FIELD_REQUIRED.ToBoolean();
            this.Type = entity.FIELD_TYPE;
            this.IsDisabled = entity.ISDISABLED.ToBoolean();
        }
        public AFieldType() { }
    }
}
