using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using  System.Collections.Generic;
using System.Collections.Specialized;

namespace QK.QAPP.Infrastructure
{
    public static class Serializer
    {
        public static string ToJson(object entity)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(entity);            
        }

        public static T FromJson<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);            
        }

        public static string ToXml<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof (T));
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                var reader = new StreamReader(stream);
                var xml = reader.ReadToEnd();
                reader.Close();
                return xml;
            }
        }

        public static T FromXml<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(xml);
                writer.Flush();
                stream.Position = 0;
                T instance = (T)serializer.Deserialize(stream);
                writer.Close();
                return instance;
            }
        }
        public static T ToDictionary<T>(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
           return jss.Deserialize<T>(json);
        }
        public static NameValueCollection DicToNameValueCollection(Dictionary<string,string> dic)
        {
            NameValueCollection collection=new NameValueCollection();
            foreach (var item in dic)
	        {
                 collection.Add(item.Key,item.Value);
            }
            return collection;
        }
        /// <summary>
        /// 将单个对象转换为NameValueCollection
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static NameValueCollection ObjToNameValueCollection(object entity)
        {
          return  Serializer.DicToNameValueCollection(Serializer.ToDictionary<Dictionary<string, string>>(Serializer.ToJson(entity)));
        }
        /// <summary>
        /// 将单个对象转换为Dictionary
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ObjToDictionary(object entity)
        {
            return Serializer.ToDictionary<Dictionary<string, string>>(Serializer.ToJson(entity));
        }
        /// <summary>
        /// 将List字符串集合转换为带符号分隔的string
        /// </summary>
        /// <param name="strList">List字符串集合</param>
        /// <param name="splitO">分隔符号</param>
        /// <returns></returns>
        public static string ListToString(List<string> strList, string splitO)
        {
            if (strList.Any())
            {
                return string.Join(splitO, strList);
            }
            return string.Empty;
        }
    }
}
