using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace QK.QAPP.MvcScaffold.DForm
{
    public class DFormHandler
    {
        private IAPP_DFORM_FORMBUILDERSERVICE FormBuilderService { get; set; }
        private IAPP_DFORM_FORMINFOSERVICE FormInfoService { get; set; }
        private IAPP_DFORM_FORMFIELDSERVICE FormFieldService { get; set; }
        private IAPP_DFORM_FIELDATTRIBUTESERVICE FormAttrService { get; set; }
        private IQFUserService UserService { get; set; }
        /// <summary>
        /// 以指定的表单为模板创建表单
        /// </summary>
        /// <param name="formCode_old">原表单代码</param>
        /// <param name="version_old">原表单版本号</param>
        /// <param name="formCode_new">新表单代码</param>
        /// <param name="version_new">新表单版本号</param>
        /// <returns></returns>
        public bool DFormCopy(string formCode_old, string version_old, string formCode_new, string version_new)
        {
            FormBuilderService = Ioc.GetService<IAPP_DFORM_FORMBUILDERSERVICE>();
            FormInfoService = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();
            FormFieldService = Ioc.GetService<IAPP_DFORM_FORMFIELDSERVICE>();
            FormAttrService = Ioc.GetService<IAPP_DFORM_FIELDATTRIBUTESERVICE>();
            UserService = Ioc.GetService<IQFUserService>();

            var formBuilderEntity = FormBuilderService.Find(b => b.CODE == formCode_old && b.VERSION == version_old).FirstOrDefault();

            var formBuilderNew = CreateNewFormBuilder(formCode_old, version_old, formCode_new, version_new, formBuilderEntity);

            var infoIDList = CreateNewFormInfo(formBuilderEntity.ID, formBuilderNew);

            foreach (var infoID in infoIDList)
            {
                var fieldIDList = CreateNewFormField(infoID.Key, infoID.Value);

                foreach (var fieldID in fieldIDList)
                {
                    CreateNewFormAttr(fieldID.Key, fieldID.Value);
                }
            }

            //FormBuilderService.Add(formBuilderNew);
            FormBuilderService.UnitOfWork.SaveChanges();

            //日志
            Infrastructure.Log4Net.LogWriter.Biz("动态表单复制", formBuilderNew.ID + String.Empty, new Dictionary<string, string>()
            {
                {"原表单ID",formBuilderEntity.ID+String.Empty},
                {"原表单版本",formBuilderEntity.VERSION},
                {"新表单ID",formBuilderNew.ID+String.Empty},
                {"新表单版本",formBuilderNew.VERSION}
            });

            return true;
        }

        /// <summary>
        /// 创建新的 DFormBuilder 对象
        /// </summary>
        /// <param name="formCode_old">原表单代码</param>
        /// <param name="version_old">原表单版本号</param>
        /// <param name="formCode_new">新表单代码</param>
        /// <param name="version_new">新表单版本号</param>
        /// <param name="formBuilderEntity">原DFormBuilder实体</param>
        /// <returns></returns>
        private APP_DFORM_FORMBUILDER CreateNewFormBuilder(string formCode_old, string version_old, string formCode_new, string version_new, APP_DFORM_FORMBUILDER formBuilderEntity)
        {
            var formBuilderNew = new APP_DFORM_FORMBUILDER();

            CloneObject(formBuilderEntity, formBuilderNew);

            formBuilderNew.ID = 0;
            formBuilderNew.CODE = formCode_new;
            formBuilderNew.VERSION = version_new;
            formBuilderNew.CREATED_TIME = DateTime.Now;
            formBuilderNew.CREATED_USER = UserService.GetCurrentUser().Code;
            formBuilderNew.CHANGED_TIME = formBuilderNew.CREATED_TIME;
            formBuilderNew.CHANGED_USER = formBuilderNew.CREATED_USER;

            FormBuilderService.Add(formBuilderNew);

            return formBuilderNew;
        }

        /// <summary>
        /// 创建 DFormInfo 对象
        /// </summary>
        /// <param name="FB_ID">原父级数据的ID</param>
        /// <param name="formBuilderNew">新的父级对象</param>
        private Dictionary<long, APP_DFORM_FORMINFO> CreateNewFormInfo(long FB_ID, APP_DFORM_FORMBUILDER formBuilderNew)
        {
            var formInfoList = FormInfoService.Find(i => i.FB_ID == FB_ID);
            var infoIDList = new Dictionary<long, APP_DFORM_FORMINFO>();
            foreach (var formInfo in formInfoList)
            {
                var formInfoNew = new APP_DFORM_FORMINFO();
                CloneObject(formInfo, formInfoNew);

                formInfoNew.ID = 0;
                //formInfoNew.APP_DFORM_FORMBUILDER = formBuilderNew;
                //formBuilderNew.APP_DFORM_FORMINFO.Add(formInfoNew);
                formInfoNew.FB_ID = formBuilderNew.ID;
                formInfoNew.CREATED_TIME = formBuilderNew.CREATED_TIME;
                formInfoNew.CREATED_USER = formBuilderNew.CREATED_USER;
                formInfoNew.CHANGED_TIME = formBuilderNew.CHANGED_TIME;
                formInfoNew.CHANGED_USER = formBuilderNew.CHANGED_USER;

                FormInfoService.Add(formInfoNew);

                infoIDList.Add(formInfo.ID, formInfoNew);
            }

            return infoIDList;
        }

        /// <summary>
        /// 创建 DFormField 对象
        /// </summary>
        /// <param name="FORMINFO_ID">原父级数据的ID</param>
        /// <param name="formInfoNew">新的父级对象</param>
        private Dictionary<long, APP_DFORM_FORMFIELD> CreateNewFormField(long FORMINFO_ID, APP_DFORM_FORMINFO formInfoNew)
        {
            var formFieldList = FormFieldService.Find(f => f.FORMINFO_ID == FORMINFO_ID);
            var fieldIDList = new Dictionary<long, APP_DFORM_FORMFIELD>();
            foreach (var formField in formFieldList)
            {
                var formFieldNew = new APP_DFORM_FORMFIELD();
                CloneObject(formField, formFieldNew);

                formFieldNew.ID = 0;
                //formFieldNew.APP_DFORM_FORMINFO = formInfoNew;
                //formInfoNew.APP_DFORM_FORMFIELD.Add(formFieldNew);
                formFieldNew.FORMINFO_ID = formInfoNew.ID;
                FormFieldService.Add(formFieldNew);

                fieldIDList.Add(formField.ID, formFieldNew);
            }


            return fieldIDList;
        }

        /// <summary>
        /// 创建 DFormAttr 对象
        /// </summary>
        /// <param name="FORMFIELD_ID">原父级数据的ID</param>
        /// <param name="formFieldNew">新的父级对象</param>
        private Dictionary<long, APP_DFORM_FIELDATTRIBUTE> CreateNewFormAttr(long FORMFIELD_ID, APP_DFORM_FORMFIELD formFieldNew)
        {
            var formAttrList = FormAttrService.Find(a => a.FORMFIELD_ID == FORMFIELD_ID);
            var attrIDList = new Dictionary<long, APP_DFORM_FIELDATTRIBUTE>();
            foreach (var formAttr in formAttrList)
            {
                var formAttrNew = new APP_DFORM_FIELDATTRIBUTE();

                CloneObject(formAttr, formAttrNew);

                formAttrNew.ID = 0;
                //formAttrNew.APP_DFORM_FORMFIELD = formFieldNew;
                //formFieldNew.APP_DFORM_FIELDATTRIBUTE.Add(formAttrNew);
                formAttrNew.FORMFIELD_ID = formFieldNew.ID;
                FormAttrService.Add(formAttrNew);

                attrIDList.Add(formAttr.ID, formAttrNew);
            }

            return attrIDList;
        }

        /// <summary>
        /// 复制对象属性值(限同类型对象间)
        /// </summary>
        /// <param name="sourceObj">原对象</param>
        /// <param name="targetObj">新对象</param>
        private void CloneObject(object sourceObj, object targetObj)
        {
            Type type = targetObj.GetType();
            var properties = type.GetProperties();
            foreach (var p in properties)
            {
                if (p.CanRead && p.CanWrite && !p.PropertyType.IsInterface && !(p.PropertyType.BaseType.Name == "BasicEntity"))
                {
                    p.SetValue(targetObj, p.GetValue(sourceObj, null), null);
                }
            }
        }



        #region DForm配置页面
        /// <summary>
        /// 获取所有字段类型  效率不高
        /// </summary>
        /// <returns></returns>
        public List<Type> GetALLFieldType()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeList = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.BaseType == typeof(AFieldType))
                {
                    typeList.Add(type);
                }
            }

            return typeList;
        }
        /// <summary>
        /// 获取所有所有实体
        /// </summary>
        /// <returns></returns>
        public List<Type> GetALLEntityType()
        {
            var assembly = Assembly.GetAssembly(typeof(BasicEntity));
            var typeList = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.BaseType == typeof(BasicEntity))
                {
                    typeList.Add(type);
                }
            }

            return typeList;
        }

        public List<string> GetFieldName(string tableName)
        {
            var assembly = Assembly.GetAssembly(typeof(BasicEntity));
            var typeList = new List<string>();
            var Type = assembly.GetType(typeof(BasicEntity).Namespace + "." + tableName);
            if (Type != null)
            {

                return Type.GetProperties().Select(c => c.Name).ToList();
            }

            return typeList;
        }

        /// <summary>
        /// 获取属性键值对
        /// </summary>
        /// <param name="fieldID"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFieldAttr(string typeName)
        {

            var dformDBReader = Ioc.GetService<DFormDBReader>();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            Type type = GetTypeByTypeName(typeName);
            //获取字段
            if (type != null)
            {
                foreach (var item in type.GetProperties())
                {
                    var attr = item.GetCustomAttributes(true);
                    var attrEntity = attr.FirstOrDefault(c => (c as DisplayNameAttribute) != null);
                    if (attrEntity != null)
                    {
                        var displayNameAttr = attrEntity as DisplayNameAttribute;
                        var key = item.Name;
                        var value = displayNameAttr.DisplayName;
                        dic.Add(key, value);

                    }

                }
            }
            return dic;

        }
        private Type GetTypeByTypeName(string typeName)
        {
            var namesplace = typeof(AFieldType).Namespace;
            return Type.GetType(namesplace + "." + typeName);
            //Type type = null;
            //foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    foreach (var t in assembly.GetTypes())
            //    {
            //        if (t.Name == typeName)
            //        {
            //            type = t;
            //            break;
            //        }
            //    }
            //}
            //return type;
        }

        /// <summary>
        /// 获取所有父表单
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllSubForm()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var formService = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();
            var formList = formService.GetAll();
            foreach (var item in formList)
            {
                dic.Add(
                    item.ID + "",
                    item.APP_DFORM_FORMBUILDER.NAME + "_" + item.APP_DFORM_FORMBUILDER.CODE + "_" + item.NAME
                    );
            }
            return dic;
        }

        public string GetFieldEntity(long id)
        {
            var formService = Ioc.GetService<IAPP_DFORM_FORMFIELDSERVICE>();
            var entity = formService.Find(c => c.ID == id).Select(c => new
            {
                c.ID,
                c.FORMINFO_ID,
                c.FIELD_KEY,
                c.FIELD_DISPLAYNAME,
                c.FIELD_TYPE,
                c.FIELD_ROWSPAN,
                c.MAPPER_TABLE,
                c.MAPPER_TABLEFIELD,
                c.FIELD_GROUP,
                c.FIELD_SORT,
                c.FIELD_REQUIRED,
                c.ISDISABLED
            }).FirstOrDefault();
            var jsonStr = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                //这句是解决问题的关键,也就是json.net官方给出的解决配置选项.                 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return jsonStr;
        }

        public string GetFormInfoEntity(long id)
        {
            var infoService = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();
            var entity = infoService.Find(i => i.ID == id).Select(i => new
            {
                i.ID,
                i.FB_ID,
                i.NAME,
                i.VERSION,
                i.READONLY,
                i.ACTION_EDIT,
                i.ACTION_READ,
                i.CREATED_USER,
                i.CREATED_TIME,
                i.CHANGED_USER,
                i.CHANGED_TIME,
                i.Sort,
                i.ADDMORE,
                i.ADDMOREKEYWORD
            }).FirstOrDefault();
            var jsonStr = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return jsonStr;
        }

        public Dictionary<string, string> GetFieldTypeAttr(long fieldId)
        {
            var formService = Ioc.GetService<DFormDBReader>();
            return formService.GetFieldAttr(fieldId);
        }
        public string SaveFieldBaseInfo(APP_DFORM_FORMFIELD entity)
        {
            try
            {
                var fieldService = Ioc.GetService<IAPP_DFORM_FORMFIELDSERVICE>();
                //var e = fieldService.FirstOrDefault(c => c.ID == entity.ID);
                //fieldService.UnitOfWork.BeginTransaction();
                fieldService.Update(entity);
                fieldService.UnitOfWork.SaveChanges();

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("更新动态表单字段", entity);

                return "";
            }
            catch (Exception ex)
            {
                return ex + "";
            }

        }

        public string SaveBuilderInfo(APP_DFORM_FORMBUILDER entity)
        {
            try
            {
                var builderService = Ioc.GetService<IAPP_DFORM_FORMBUILDERSERVICE>();

                builderService.Update(entity);
                builderService.UnitOfWork.SaveChanges();

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("更新动态表单Builder", entity.ID + string.Empty,
                    new
                    {
                        entity.ID,
                        entity.NAME,
                        entity.VERSION,
                        entity.CODE,
                        entity.CHANGED_TIME,
                        entity.CHANGED_USER
                    });

                return "";
            }
            catch (Exception ex)
            {
                return ex + "";
            }
        }

        public string SaveFormInfo(APP_DFORM_FORMINFO entity)
        {
            try
            {
                var infoService = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();

                infoService.Update(entity);
                infoService.UnitOfWork.SaveChanges();

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("更新动态表单FormInfo",entity.ID + string.Empty,new
                {
                    entity.ID,
                    entity.NAME,
                    entity.VERSION,
                    entity.READONLY,
                    entity.ACTION_EDIT,
                    entity.ACTION_READ,
                    entity.CHANGED_USER,
                    entity.CHANGED_TIME,
                    entity.Sort,
                    entity.ADDMORE,
                    entity.ADDMOREKEYWORD
                });

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex + string.Empty;
            }
        }

        public string SaveFieldAttrInfo(System.Web.Mvc.FormCollection form)
        {
            try
            {
                var id = form["ID"].ToLong();
                var fieldService = Ioc.GetService<IAPP_DFORM_FIELDATTRIBUTESERVICE>();
                // fieldService.UnitOfWork.BeginTransaction();
                //删除原数据
                var oldAttr = fieldService.Find(c => c.FORMFIELD_ID == id).ToList();
                fieldService.DeleteMultiple(oldAttr);

                List<APP_DFORM_FIELDATTRIBUTE> entities = new List<APP_DFORM_FIELDATTRIBUTE>();

                foreach (var item in form.AllKeys)
                {
                    if (item != "ID")
                    {
                        var value = form[item];
                        if (value != null)
                        {
                            value = form[item].Trim();
                        }
                        var entity = new APP_DFORM_FIELDATTRIBUTE()
                        {
                            FORMFIELD_ID = id,
                            ATTRIBUTE_KEY = item,
                            ATTRIBUTE_VALUE = form[item]
                        };
                        entities.Add(entity);
                        //fieldService.Add(entity);
                    }
                }
                // fieldService.UnitOfWork.CommitTransaction();
                fieldService.AddMultiple(entities);
                fieldService.UnitOfWork.SaveChanges();

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("更新动态表单字段属性", id + String.Empty, entities);

                return "";
            }
            catch (Exception ex)
            {

                return ex + "";
            }
        }
        #endregion
    }
}
