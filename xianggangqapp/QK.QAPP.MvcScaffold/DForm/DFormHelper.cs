/*********************
 * 作    者:刘成帅
 * 创建时间:2014/9/9
 * 功    能:将前端表单变成对象或将后端对象变化为前端键值对的类
**********************/
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Reflection;
using System.Diagnostics;

namespace QK.QAPP.MvcScaffold.DForm
{
    /// <summary>
    /// 将前端表单变成对象或将后端对象变化为前端键值对的类
    /// </summary>
    public class DFormHelper
    {
        /// <summary>
        /// 通过验证初始化对象 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="form"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static T InitObjVaidate<T>(FormCollection form, out string error) where T : BasicEntity, new()
        {
            return InitObjVaidate<T>(form, new T(), out error);
        }

        /// <summary>
        /// 对现有实体进行变更 
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="form">表单字段集合</param>
        /// <param name="error">out 验证错误信息</param>
        /// <returns>实体类型</returns>
        public static T InitObjVaidate<T>(FormCollection form, T entity, out string error) where T : BasicEntity, new()
        {
            error = "";
            var obj = entity;
            var dfromId = form.AllKeys.Contains("subformID") ? form["subformID"] : "";
            if (!string.IsNullOrEmpty(dfromId))
            {
                var subform = new DFormSubform(dfromId.ToLong(), ENUM_FormOperation.READONLY);
                Type type = typeof(T);
                var thisObjFieldList = subform.FieldList.Where(c => c.Mapper_Table == type.Name);
                foreach (AFieldType field in thisObjFieldList)
                {

                    if (field is Adress)
                    {
                        var adress = field as Adress;
                        if (!string.IsNullOrEmpty(adress.Mapper_Province))
                        {
                            var pProvince = type.GetProperty(adress.Mapper_Province);
                            if (pProvince != null)
                            {
                                SetValue<T>(form, adress, adress.Province, pProvince, ConvertToPropType, obj, ref error);
                            }
                        }
                        if (!string.IsNullOrEmpty(adress.Mapper_City))
                        {
                            var pCity = type.GetProperty(adress.Mapper_City);
                            if (pCity != null)
                            {
                                SetValue<T>(form, adress, adress.City, pCity, ConvertToPropType, obj, ref error);
                            }
                        }

                        if (!string.IsNullOrEmpty(adress.Mapper_Detail))
                        {
                            var pDetail = type.GetProperty(adress.Mapper_Detail);
                            if (pDetail != null)
                            {
                                SetValue<T>(form, adress, adress.Detail, pDetail, ConvertToPropType, obj, ref error);
                            }
                        }
                    }
                    else if (field is StaffPicker)
                    {
                        var staffPicker = field as StaffPicker;
                        if (!string.IsNullOrEmpty(staffPicker.Mapper_StaffCode))
                        {
                            var pCode = type.GetProperty(staffPicker.Mapper_StaffCode);
                            if (pCode != null)
                            {
                                string thisError = "";
                                if (form[staffPicker.Field_Key + "_hidden"] == null)
                                {
                                    //Trace.Write(new NullReferenceException(), "error");
                                    Infrastructure.Log4Net.LogWriter.Error(string.Format("表单中不存在键：{0}", staffPicker.Field_Key + "_hidden"));
                                }
                                else
                                {
                                    SetValue<T>(form, staffPicker, staffPicker.Field_Key + "_hidden", pCode,
                                        (v, p) => (ConvertToPropType(v, p) + string.Empty).TrimEnd(';'), obj, ref error);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(staffPicker.Mapper_StaffName))
                        {
                            var p_name = type.GetProperty(staffPicker.Mapper_StaffName);
                            if (p_name != null)
                            {
                                SetValue<T>(form, staffPicker, staffPicker.Field_Key, p_name,
                                    (v, p) => (ConvertToPropType(v, p) + string.Empty).TrimEnd(';'), obj, ref error);
                            }
                        }
                    }
                    else if (field is CompanyPicker)
                    {
                        var companyPicker = field as CompanyPicker;

                        //COM_NAME
                        if (!string.IsNullOrEmpty(companyPicker.Mapper_TableField))
                        {
                            var pName = type.GetProperty(companyPicker.Mapper_TableField);
                            if (pName != null)
                            {
                                SetValue<T>(form, companyPicker, companyPicker.Field_Key, pName, ConvertToPropType, obj, ref error);
                            }
                        }

                        //AEO_CODE
                        if (!string.IsNullOrEmpty(companyPicker.Mapper_AEO_Code))
                        {
                            var pCode = type.GetProperty(companyPicker.Mapper_AEO_Code);
                            if (pCode != null)
                            {
                                SetValue<T>(form, companyPicker, companyPicker.Field_Key + "_AEOCode", pCode, ConvertToPropType, obj, ref error);
                            }
                        }

                        //AEO_TYPE
                        if (!string.IsNullOrEmpty(companyPicker.Mapper_AEO_Type))
                        {
                            var pType = type.GetProperty(companyPicker.Mapper_AEO_Type);
                            if (pType != null)
                            {
                                SetValue<T>(form, companyPicker, companyPicker.Field_Key + "_AEOType", pType, ConvertToPropType, obj, ref error);
                            }
                        }
                    }
                    else if (field is CarAgencyPicker)
                    {
                        var carAgencyPicker = field as CarAgencyPicker;

                        //COM_NAME
                        if (!string.IsNullOrEmpty(carAgencyPicker.Mapper_TableField))
                        {
                            var pName = type.GetProperty(carAgencyPicker.Mapper_TableField);
                            if (pName != null)
                            {
                                SetValue<T>(form, carAgencyPicker, carAgencyPicker.Field_Key, pName, ConvertToPropType, obj, ref error);
                            }
                        }

                        //MOTO_NO
                        if (!string.IsNullOrEmpty(carAgencyPicker.Mapper_CarAgency_Code))
                        {
                            var pCode = type.GetProperty(carAgencyPicker.Mapper_CarAgency_Code);
                            if (pCode != null)
                            {
                                SetValue<T>(form, carAgencyPicker, carAgencyPicker.Field_Key + "_MOTOCode", pCode, ConvertToPropType, obj, ref error);
                            }
                        }
                    }
                    else if (field is CarPicker)
                    {
                        var carPicker = field as CarPicker;
                        //CAR_BRAND
                        if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Brand))
                        {
                            var pBrand = type.GetProperty(carPicker.Mapper_Car_Brand);
                            if (pBrand != null)
                            {
                                SetValue<T>(form, carPicker, carPicker.Field_Key + "_Brand", pBrand, ConvertToPropType, obj, ref error);
                            }
                        }
                        //CAR_SERIES
                        if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Series))
                        {
                            var pSeries = type.GetProperty(carPicker.Mapper_Car_Series);
                            if (pSeries != null)
                            {
                                SetValue<T>(form, carPicker, carPicker.Field_Key + "_Series", pSeries, ConvertToPropType, obj, ref error);
                            }
                        }
                        //CAR_STYLE
                        if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Style))
                        {
                            var pStyle = type.GetProperty(carPicker.Mapper_Car_Style);
                            if (pStyle != null)
                            {
                                SetValue<T>(form, carPicker, carPicker.Field_Key + "_Style", pStyle, ConvertToPropType, obj, ref error);
                            }
                        }
                        //CAR_PRICE
                        if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Price))
                        {
                            var pPrice = type.GetProperty(carPicker.Mapper_Car_Price);
                            if (pPrice != null)
                            {
                                SetValue<T>(form, carPicker, carPicker.Field_Key + "_Price", pPrice, ConvertToPropType, obj, ref error);
                            }
                        }
                        //PRODUCT_DATE
                        if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Year))
                        {
                            var pYear = type.GetProperty(carPicker.Mapper_Car_Year);
                            if (pYear != null)
                            {
                                SetValue<T>(form, carPicker, carPicker.Field_Key + "_Year", pYear, ConvertToPropType, obj, ref error);
                            }
                        }
                        //CARINFO_TYPE
                        if (!string.IsNullOrEmpty(carPicker.Mapper_CarInfo_Type))
                        {
                            var pCarType = type.GetProperty(carPicker.Mapper_CarInfo_Type);
                            if (pCarType != null)
                            {
                                SetValue<T>(form, carPicker, carPicker.Field_Key + "_CarType", pCarType, ConvertToPropType, obj, ref error);
                            }
                        }
                    }
                    else
                    {
                        var prop = type.GetProperty(field.Mapper_TableField);
                        if (prop != null)
                        {
                            SetValue<T>(form, field, field.Field_Key, prop, ConvertToPropType, obj, ref error);
                        }
                    }
                }

            }
            return obj;
        }

        /// <summary>
        /// 验证并设置属性的值
        /// </summary>
        /// <typeparam name="T">BasicEntity或其子类</typeparam>
        /// <param name="form">表单数据</param>
        /// <param name="field">当前字段类型</param>
        /// <param name="formKey">表单中的Key</param>
        /// <param name="prop">需要设置值的属性</param>
        /// <param name="obj">T类型的对象</param>
        /// <param name="convertFun">转换类型的方法</param>
        /// <param name="error"></param>
        private static void SetValue<T>(FormCollection form, AFieldType field, string formKey, PropertyInfo prop, Func<object, PropertyInfo, object> convertFun, T obj, ref string error)
            where T : BasicEntity, new()
        {
            string thisError;
            string value = string.Empty; //form[field.Field_Key] != null ? form[field.Field_Key].Trim() : null;
            //Contains的原因：对于车贷有授薪和经营两类人群，表单中对应的字段不同，如果表单中不包含字段，则不进行Validate
            if (form.AllKeys.Contains(formKey))
            {
                value = form[formKey];
                if (field.Vaidate(value, out thisError))
                {
                    if (value != null)
                    {
                        prop.SetValue(obj, !string.IsNullOrWhiteSpace(value) ? convertFun(value.Trim(), prop) : null, null);
                    }
                }
                else
                {
                    error += thisError;
                }
            }
        }

        /// <summary>
        /// 将值转化为属性可以接受的对象
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        public static object ConvertToPropType(object value, PropertyInfo prop)
        {
            object ret = null;
            try
            {
                var ctype = prop.PropertyType;
                //判断是否是泛型
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    ctype = prop.PropertyType.GetGenericArguments()[0];
                }
                if (value != null)
                {
                    ret = Convert.ChangeType(value, ctype);
                }
            }
            catch (Exception ex)
            {
                //Trace.Write(ex, "error");
                Infrastructure.Log4Net.LogWriter.Error("类型转换错误", ex);
            }
            return ret;
        }
        /// <summary>
        /// (反射)通过反射，将后端实体变成前端对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="subformID"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetFormJson<T>(T obj, long subformID) where T : BasicEntity, new()
        {
            var subform = new DFormSubform(subformID, ENUM_FormOperation.READONLY);
            var ret = new Dictionary<string, object>();
            var type = typeof(T);
            if (subform != null)
            {
                foreach (AFieldType item in subform.FieldList)
                {
                    var key = item.Field_Key;
                    if (item.Mapper_Table == type.Name)
                    {
                        //地址字段的特殊赋值
                        if (item is Adress)
                        {
                            var adress = item as Adress;
                            if (!string.IsNullOrEmpty(adress.Mapper_Province))
                            {
                                var p_province = type.GetProperty(adress.Mapper_Province);
                                if (p_province != null)
                                {
                                    ret.Add(adress.Province, p_province.GetValue(obj));
                                }
                            }
                            if (!string.IsNullOrEmpty(adress.Mapper_City))
                            {
                                var p_city = type.GetProperty(adress.Mapper_City);
                                if (p_city != null)
                                {
                                    ret.Add(adress.City, p_city.GetValue(obj));
                                }
                            }

                            if (!string.IsNullOrEmpty(adress.Mapper_Detail))
                            {
                                var p_detail = type.GetProperty(adress.Mapper_Detail);
                                if (p_detail != null)
                                {
                                    ret.Add(adress.Detail, p_detail.GetValue(obj));
                                }
                            }
                        }
                        else if (item is StaffPicker)
                        {
                            var staffPicker = item as StaffPicker;
                            if (!string.IsNullOrEmpty(staffPicker.Mapper_StaffCode))
                            {
                                var p_code = type.GetProperty(staffPicker.Mapper_StaffCode);
                                if (p_code != null)
                                {
                                    var p_v = p_code.GetValue(obj) + "";
                                    if (!string.IsNullOrEmpty(p_v))
                                    {
                                        p_v += ";";
                                    }
                                    ret.Add(staffPicker.Field_Key + "_hidden", p_v);
                                }
                            }
                            if (!string.IsNullOrEmpty(staffPicker.Mapper_StaffName))
                            {
                                var p_name = type.GetProperty(staffPicker.Mapper_StaffName);
                                if (p_name != null)
                                {
                                    var p_v = p_name.GetValue(obj) + "";
                                    if (!string.IsNullOrEmpty(p_v))
                                    {
                                        p_v += ";";
                                    }
                                    ret.Add(staffPicker.Field_Key, p_v);
                                }
                            }
                        }
                        else if (item is DatePicker)
                        {
                            var dp = item as DatePicker;
                            var prop = type.GetProperty(item.Mapper_TableField);
                            if (prop != null)
                            {
                                var value = prop.GetValue(obj);
                                if (!string.IsNullOrEmpty(value + ""))
                                {
                                    var dateVlaue = DateTime.Parse(value + "");

                                    ret.Add(key, dateVlaue.ToString("yyyy/MM/dd"));
                                }

                            }
                        }
                        //企业选择特殊赋值
                        else if (item is CompanyPicker)
                        {
                            var companyPicker = item as CompanyPicker;
                            if (!string.IsNullOrEmpty(companyPicker.Mapper_TableField))
                            {
                                var pName = type.GetProperty(companyPicker.Mapper_TableField);
                                if (pName != null)
                                {
                                    ret.Add(companyPicker.Field_Key, pName.GetValue(obj));
                                }
                            }
                            if (!string.IsNullOrEmpty(companyPicker.Mapper_AEO_Code))
                            {
                                var pCode = type.GetProperty(companyPicker.Mapper_AEO_Code);
                                if (pCode != null)
                                {
                                    ret.Add(companyPicker.Field_Key + "_AEOCode", pCode.GetValue(obj));
                                }
                            }
                            if (!string.IsNullOrEmpty(companyPicker.Mapper_AEO_Type))
                            {
                                var pType = type.GetProperty(companyPicker.Mapper_AEO_Type);
                                if (pType != null)
                                {
                                    ret.Add(companyPicker.Field_Key + "_AEOType", pType.GetValue(obj));
                                }
                            }
                        }
                        //经销商选择特殊赋值
                        else if (item is CarAgencyPicker)
                        {
                            var carAgencyPicker = item as CarAgencyPicker;
                            if (!string.IsNullOrEmpty(carAgencyPicker.Mapper_TableField))
                            {
                                var pName = type.GetProperty(carAgencyPicker.Mapper_TableField);
                                if (pName != null)
                                {
                                    ret.Add(carAgencyPicker.Field_Key, pName.GetValue(obj));
                                }
                            }
                            if (!string.IsNullOrEmpty(carAgencyPicker.Mapper_CarAgency_Code))
                            {
                                var pCode = type.GetProperty(carAgencyPicker.Mapper_CarAgency_Code);
                                if (pCode != null)
                                {
                                    ret.Add(carAgencyPicker.Field_Key + "_MOTOCode", pCode.GetValue(obj));
                                }
                            }
                        }
                        //车辆选择特殊字段复制
                        else if (item is CarPicker)
                        {
                            var carPicker = item as CarPicker;
                            //CAR_BRAND
                            if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Brand))
                            {
                                var pBrand = type.GetProperty(carPicker.Mapper_Car_Brand);
                                if (pBrand != null)
                                {
                                    ret.Add(carPicker.Field_Key + "_Brand", pBrand.GetValue(obj));
                                }
                            }
                            //CAR_STYLE
                            if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Style))
                            {
                                var pStyle = type.GetProperty(carPicker.Mapper_Car_Style);
                                if (pStyle != null)
                                {
                                    ret.Add(carPicker.Field_Key + "_Style", pStyle.GetValue(obj));
                                }
                            }
                            //CAR_SERIES
                            if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Series))
                            {
                                var pSeries = type.GetProperty(carPicker.Mapper_Car_Series);
                                if (pSeries != null)
                                {
                                    ret.Add(carPicker.Field_Key + "_Series", pSeries.GetValue(obj));
                                }
                            }
                            //CAR_PRICE
                            if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Price))
                            {
                                var pPrice = type.GetProperty(carPicker.Mapper_Car_Price);
                                if (pPrice != null)
                                {
                                    ret.Add(carPicker.Field_Key + "_Price", pPrice.GetValue(obj));
                                }
                            }
                            //PRODUCT_DATE
                            if (!string.IsNullOrEmpty(carPicker.Mapper_Car_Year))
                            {
                                var pYear = type.GetProperty(carPicker.Mapper_Car_Year);
                                if (pYear != null)
                                {
                                    ret.Add(carPicker.Field_Key + "_Year", pYear.GetValue(obj));
                                }
                            }
                            //CARINFO_TYPE
                            if (!string.IsNullOrEmpty(carPicker.Mapper_CarInfo_Type))
                            {
                                var pCarType = type.GetProperty(carPicker.Mapper_CarInfo_Type);
                                if (pCarType != null)
                                {
                                    ret.Add(carPicker.Field_Key + "_CarType", pCarType.GetValue(obj));
                                }
                            }
                        }
                        else
                        {
                            var prop = type.GetProperty(item.Mapper_TableField);
                            if (prop != null)
                            {
                                var value = prop.GetValue(obj);
                                ret.Add(key, value);
                            }
                        }
                    }



                }
            }

            return ret;
        }


    }
}
