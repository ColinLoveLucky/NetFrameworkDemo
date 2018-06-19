/*********************
 * 作者：刘成帅
 * 时间：2014/9/9
 * 功能：用于从数据库中读取动态表单的一些方法
**********************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.Global;
using QK.QAPP.Services;
using QK.QAPP.IServices;
using Microsoft.Practices.Unity;

namespace QK.QAPP.MvcScaffold.DForm
{
    public class DFormDBReader
    {
        [Dependency]
        public IAPP_DFORM_FORMBUILDERSERVICE FormBuilderService { get; set; }
        [Dependency]
        public IAPP_DFORM_FORMINFOSERVICE FormInfoService { get; set; }
        [Dependency]
        public IAPP_DFORM_FORMFIELDSERVICE FormFieldService { get; set; }
        [Dependency]
        public IAPP_DFORM_FIELDATTRIBUTESERVICE FormAttrService { get; set; }

        /// <summary>
        /// 通过Name和Version获取表单
        /// </summary>
        /// <param name="formName">名字</param>
        /// <param name="version">表单版本</param>
        /// <returns></returns>
        public APP_DFORM_FORMBUILDER GetFormBuilderByName(string formCode, string version)
        {
            var enity = FormBuilderService.Find(c => c.CODE == formCode && c.VERSION == version).FirstOrDefault();
            return enity;
        }
        /// <summary>
        /// 通过ID获取表单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APP_DFORM_FORMBUILDER GetFormBuilderByID(long id)
        {
            var enity = FormBuilderService.Find(c => c.ID == id).FirstOrDefault();
            return enity;
        }
        /// <summary>
        /// 获取表单的子表
        /// </summary>
        /// <param name="formBuilder"></param>
        /// <returns></returns>
        public List<APP_DFORM_FORMINFO> GetSubFormList(APP_DFORM_FORMBUILDER formBuilder)
        {
            List<APP_DFORM_FORMINFO> entityList = null;
            var _entityList = FormInfoService.Find(c => c.FB_ID == formBuilder.ID);
            if (_entityList != null)
            {
                entityList = _entityList.ToList();
            }
            return entityList;
        }
        /// <summary>
        /// 通过ID获取子表单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APP_DFORM_FORMINFO GetSubFormByID(long id)
        {
            var enity = FormInfoService.Find(c => c.ID == id).FirstOrDefault();
            return enity;
        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="fromInfo"></param>
        /// <returns></returns>
        public List<APP_DFORM_FORMFIELD> GetFieldList(APP_DFORM_FORMINFO fromInfo)
        {
            var entityList = FormFieldService.Find(c => c.FORMINFO_ID == fromInfo.ID).ToList();
            return entityList;
        }
        /// <summary>
        /// 通过ID获取字段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APP_DFORM_FORMFIELD GetFieldById(long id)
        {
            var entity = FormFieldService.Find(c => c.ID == id).FirstOrDefault();
            return entity;
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="fieldID"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldAttr(long fieldID)
        {
            var dic = new Dictionary<string, string>();
            var entityList = FormAttrService.Find(c => c.FORMFIELD_ID == fieldID);
            foreach (var item in entityList)
            {
                dic.Add(item.ATTRIBUTE_KEY, item.ATTRIBUTE_VALUE);
            }
            return dic;
        }
    }
}
