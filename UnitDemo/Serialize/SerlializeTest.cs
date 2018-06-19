using EntityFrameWorkInfrans;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace UnitDemo.SerializeT
{
    public class SerlializeTest
    {
        public void TestXmlSerializer()
        {
            SerializeObject student = new SerializeObject()
            {
                Name = "zhangsan",
                Age = 25
            };
            XmlSerializer xs = new XmlSerializer(typeof(SerializeObject));
            using (Stream stream = new FileStream("D:\\student.xml", FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                xs.Serialize(stream, student);
            }
        }

        public void TestXmlDeserializer()
        {
            XmlSerializer xs = new XmlSerializer(typeof(SerializeObject));

            SerializeObject student = null;

            using (FileStream stream = new FileStream("D:\\student.xml", FileMode.Open, FileAccess.Read))
            {
                student = (SerializeObject)xs.Deserialize(stream);
            }
        }

        public void TestBinarySerilizeFormatter()
        {
            //没有考虑到加载到内存的字节流的大小
            SerializeObject student = new SerializeObject()
            {
                Name = "xiao Ming",
                Age = 29
            };

            using (FileStream stream = new FileStream("D:\\student.bat", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(stream, student);
            }
        }

        public void TestBinaryDesrialize()
        {
            //没有考虑到加载到内存的字节流的大小
            SerializeObject student = null;

            using (FileStream stream = new FileStream("D:\\student.bat", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();

                student = (SerializeObject)bf.Deserialize(stream);
            }
        }
        public void WriteFile(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            string filePath = "D:\\student.xml";

            Console.WriteLine("Please input your content.");

            var content = Console.ReadLine();

            FileStream fs = new FileStream(filePath, fileMode, fileAccess, fileShare);

            var buffer = Encoding.Default.GetBytes(content);

            fs.Write(buffer, 0, buffer.Length);

            fs.Flush();
        }

        public void ReadFile(FileAccess fileAccess, FileShare fileShare)
        {
            string filePath = "D:\\student.xml";

            FileStream fs = new FileStream(filePath, FileMode.Open, fileAccess, fileShare);

            var buffer = new byte[fs.Length];

            fs.Position = 0;

            fs.Read(buffer, 0, buffer.Length);

            Console.WriteLine(Encoding.Default.GetString(buffer));
        }

        public void JavaScriptSerialize()
        {
            JavaScriptSerializer jSeria = new JavaScriptSerializer();
            JsObject jsObj = new JsObject()
            {
                Id = 1,
                Name = "zhangsan"
            };
            var str = jSeria.Serialize(jsObj);
            Console.WriteLine(str);

        }

        public void JavaScriptDeserialize()
        {
            JavaScriptSerializer jDesria = new JavaScriptSerializer();

            JsObject jsObj = new JsObject()
            {
                Id = 1,
                Name = "zhangsan"

            };

            var str = jDesria.Serialize(jsObj);

            var dobject = jDesria.Deserialize<JsObject>(str);

            Console.WriteLine(dobject.Name);
        }

        public void JavaScriptDtSerialize()
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer(new JsResoulver());
            DataTable dt = new DataTable("dt");
            dt.Columns.AddRange(new DataColumn[]{
                    new DataColumn("Id", typeof(int)),
                    new DataColumn("Name", typeof(String)),
                    new DataColumn("Price", typeof(decimal))
                });
            for (int i = 0; i < 3; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = i + 1;
                dr["Name"] = "Oger" + i;
                dr["Price"] = 12m + i;
                dt.Rows.Add(dr);
            }

            DtJavaScriptConverter convert = new DtJavaScriptConverter(new Type[] { typeof(DataTable) });

            jsSerializer.RegisterConverters(new JavaScriptConverter[] { convert });
            var json = jsSerializer.Serialize(dt);
            DataTable newDt = jsSerializer.Deserialize<DataTable>(json);
            Console.WriteLine(json);


            var jsonJs = new JavaScriptSerializer(new JsResoulver());
            var jsonStr = jsonJs.Serialize(DateTime.Now);
            Console.WriteLine(jsonStr);
            Console.WriteLine(jsonJs.Deserialize<DateTime>(jsonStr));

            //  JavaScriptSerializer serializer = new JavaScriptSerializer(resolver);

        }
    }

    [Serializable]
    public class SerializeObject : ISerializable
    {
        private string _name;

        [NonSerialized]
        private int _age;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
            }


        }

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
        }
        public SerializeObject() { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public SerializeObject(SerializationInfo info, StreamingContext context)
        {
            _name = info.GetString("Name");
            _age = info.GetInt32("Age");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", _name);

            info.AddValue("Age", _age);
        }
    }

    public class JsObject
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
    public class DtJavaScriptConverter : JavaScriptConverter
    {
        private IEnumerable<Type> _supportedTypes;
        public DtJavaScriptConverter(IEnumerable<Type> supportedTypes)
        {
            this._supportedTypes = supportedTypes;
        }
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (type == typeof(DataTable))
            {
                foreach (KeyValuePair<string, object> item in dictionary)
                {
                    DataTable dt = new DataTable(item.Key);

                    ArrayList rows = item.Value as ArrayList;

                    Dictionary<string, object> schema = serializer.ConvertToType<Dictionary<string, object>>(rows[0]);

                    foreach (string colum in schema.Keys)
                        dt.Columns.Add(colum);

                    foreach (object objRow in rows)
                    {
                        DataRow dr = dt.NewRow();

                        Dictionary<string, object> dicRow = serializer.ConvertToType<Dictionary<string, object>>(objRow);

                        foreach (KeyValuePair<string, object> rowline in dicRow)
                            dr[rowline.Key] = rowline.Value;

                        dt.Rows.Add(dr);
                    }

                    return dt;
                }
            }

            return null;
        }
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            DataTable dt = obj as DataTable;

            Dictionary<string, object> result = new Dictionary<string, object>();

            if (dt != null)
            {
                List<Dictionary<string, object>> arrList = new List<Dictionary<string, object>>();

                foreach (DataRow row in dt.Rows)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    foreach (DataColumn col in dt.Columns)
                        dic.Add(col.ColumnName, row[col.ColumnName]);
                    arrList.Add(dic);
                }

                result[dt.TableName] = arrList;
            }

            return result;
        }
        public override IEnumerable<Type> SupportedTypes
        {
            get { return _supportedTypes; }
        }
    }
    public class JsResoulver : JavaScriptTypeResolver
    {
        public override Type ResolveType(string id)
        {
            //  throw new NotImplementedException();

            return Type.GetType(id);
        }

        public override string ResolveTypeId(Type type)
        {
            //   throw new NotImplementedException();

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return type.AssemblyQualifiedName;

        }
    }

}
