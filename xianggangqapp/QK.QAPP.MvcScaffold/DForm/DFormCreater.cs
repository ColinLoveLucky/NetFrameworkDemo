/*********************
 * 作    者:刘成帅
 * 创建时间:2014/9/9
 * 功    能:动态表单对象创建者，可以通过初始化该类创建动态表单对象
**********************/

using QK.QAPP.Infrastructure;
using QK.QAPP.Entity;
using System.Collections.Generic;
using Antlr3.ST;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Cache;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace QK.QAPP.MvcScaffold.DForm
{
    /// <summary>
    /// 动态表单对象创建者，可以通过初始化该类创建动态表单对象
    /// </summary>
    public class DFormCreater
    {

        public string StringTemplatePath = GlobalSetting.DFormPath + @"\DFromContainer\FormInfoContainer.html";
        public long ID { get; set; }

        /// <summary>
        /// 表单名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表单提交位置（编辑）
        /// </summary>
        public string Action_Edit { get; set; }
        /// <summary>
        /// 表单提交位置（只读）
        /// </summary>
        public string Action_Read { get; set; }

        /// <summary>
        /// 子表单集合
        /// </summary>
        public List<DFormSubform> SubformList { get; set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly { get; set; }

        private string DFormCacheKeyStr = "QAPP_DFORMHTML_{0}_{1}_{2}";

        public ENUM_FormOperation Operation { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formCode">表单名字</param>
        /// <param name="version">版本</param>
        public DFormCreater(string formCode, string version, ENUM_FormOperation operation)
        {
            var dbReader = Ioc.GetService<DFormDBReader>();


            var entity = dbReader.GetFormBuilderByName(formCode, version);
            InitDFormCreater(entity, operation);


        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ID"></param>
        public DFormCreater(long ID, ENUM_FormOperation operation)
        {
            var dbReader = Ioc.GetService<DFormDBReader>();
            var entity = dbReader.GetFormBuilderByID(ID);
            InitDFormCreater(entity, operation);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="version"></param>
        /// <param name="operation"></param>
        /// <param name="cacheKey"></param>
        public DFormCreater(string formCode, string version, ENUM_FormOperation operation, string cacheKey)
        {
            var dbReader = Ioc.GetService<DFormDBReader>();
            var entity = dbReader.GetFormBuilderByName(formCode, version);
            InitDFormCreater(entity, operation, cacheKey);
        }

        /// <summary>
        /// 通过获取的实体
        /// </summary>
        /// <param name="info"></param>
        private void InitDFormCreater(APP_DFORM_FORMBUILDER info, ENUM_FormOperation operation)
        {
            var dbReader = Ioc.GetService<DFormDBReader>();
            var entity = info;

            if (entity != null)
            {
                this.ID = entity.ID;
                this.Action_Edit = entity.ACTION_EDIT;
                this.Action_Read = entity.ACTION_READ;
                this.Name = entity.NAME;
                this.ReadOnly = operation != ENUM_FormOperation.READONLY;
                this.Operation = operation;
                this.SubformList = new List<DFormSubform>();
                var cacheService = Ioc.GetService<ICacheProvider>();
                //如果存在对应缓存  不去实例化子类
                if (cacheService.GetALLKey().All(c => !c.StartsWith("QAPP_DFORMHTML_" + this.ID + "_" + this.Operation + "_")))
                {
                    var subformList = dbReader.GetSubFormList(entity);
                    foreach (var item in subformList)
                    {
                        if (item != null)
                        {
                            this.SubformList.Add(new DFormSubform(item.ID, operation));
                        }
                    }
                    this.SubformList = this.SubformList.OrderBy(c => c.Sort).ToList();
                }


            }

        }

        /// <summary>
        /// 通过获取的实体（增加缓存附加键）
        /// </summary>
        /// <param name="info"></param>
        /// <param name="operation"></param>
        /// <param name="cacheKey"></param>
        private void InitDFormCreater(APP_DFORM_FORMBUILDER info, ENUM_FormOperation operation, string cacheKey)
        {
            var dbReader = Ioc.GetService<DFormDBReader>();
            var entity = info;

            if (entity != null)
            {
                this.ID = entity.ID;
                this.Action_Edit = entity.ACTION_EDIT;
                this.Action_Read = entity.ACTION_READ;
                this.Name = entity.NAME;
                this.ReadOnly = operation != ENUM_FormOperation.READONLY;
                this.Operation = operation;
                this.SubformList = new List<DFormSubform>();
                var cacheService = Ioc.GetService<ICacheProvider>();

                string key = string.Format(DFormCacheKeyStr, ID, Operation, cacheKey);
                //如果存在对应缓存  不去实例化子类
                //if (cacheService.GetALLKey().All(c => !c.StartsWith("QAPP_DFORMHTML_" + this.ID + "_" + this.Operation + "_" + cacheKey)))
                if(!cacheService.Contains(key))
                {
                    var subformList = dbReader.GetSubFormList(entity);
                    foreach (var item in subformList)
                    {
                        if (item != null)
                        {
                            this.SubformList.Add(new DFormSubform(item.ID, operation));
                        }
                    }
                    this.SubformList = this.SubformList.OrderBy(c => c.Sort).ToList();
                }
            }

        }

        /// <summary>
        /// 创建HTML
        /// </summary>
        /// <returns></returns>
        public string CreateDForm()
        {
            return CreateDForm("");

        }
        /// <summary>
        /// 为缓存添加附加键
        /// </summary>
        /// <returns></returns>
        public string CreateDForm(string cacheKey)
        {
            var cacheService = Ioc.GetService<ICacheProvider>();
            string key = string.Format(DFormCacheKeyStr, ID, Operation, cacheKey);
            return cacheService.GetFromCacheOrProxy<string>(key, () =>
            {
                var template = FileReader.GetTmpString(this.StringTemplatePath);
                StringTemplate st = new StringTemplate(template);
                st.SetAttribute("DFormInfo", this);
                return st.ToString();
            });

        }
    }
}
