using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 实体基类
    /// </summary>
    [Serializable]
    public class BasicEntity
    {
        /// <summary>
        /// 进件菜单的MenuCode，约定从APP_MAIN对象赋值和取值
        /// </summary>
        public string InputMenuCode;

        public BasicEntity() { }

        /// <summary>
        /// 是否生成自增ID
        /// </summary>
        /// <param name="isGenerated"></param>
        public BasicEntity(bool isGenerated)
        {
            if (isGenerated)
            {
                Initializer(this);
            }
        }


        /// <summary>
        /// 初始化Sequence
        /// </summary>
        /// <param name="obj"></param>
        public static void Initializer(object obj)
        {
            try
            {
                var props = from prop in obj.GetType().GetProperties()
                            let attrs = prop.GetCustomAttributes(typeof(SequenceAttribute), false)
                            where attrs.Any()
                            select new { Property = prop, Attr = ((SequenceAttribute)attrs.First()) };
                foreach (var pair in props)
                {
                    var SequenceName = pair.Attr.SequenceName;
                    Int64 SequenceValue = 0;
                    string Connection = ConfigurationManager.ConnectionStrings["APPEntities"].ConnectionString;
                    Connection = Connection.Substring(Connection.IndexOf(@"connection string=") + 19);
                    Connection = Connection.Remove(Connection.Length - 1, 1);
                    using (IDbConnection con = new Oracle.DataAccess.Client.OracleConnection(Connection))
                    {
                        using (IDbCommand cmd = con.CreateCommand())
                        {
                            con.Open();
                            cmd.CommandText = String.Format("SELECT {0}.NEXTVAL FROM DUAL", SequenceName);
                            SequenceValue = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                    Int64 val = SequenceValue;
                    pair.Property.SetValue(obj, val, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
