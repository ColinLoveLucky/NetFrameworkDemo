using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace UnitDemo.MemBaseCopy
{
    [Serializable]
    public class DeepClone : ICloneable
    {
        public string Name
        {
            get;
            set;
        }

        public BirthDay Birthday { get; set; }

        public Company Company { get; set; }

        //public object Clone()
        //{
        //    return this.MemberwiseClone();
        //}

        ///// <summary>
        ///// Serilize BinaryFormatter
        ///// </summary>
        ///// <returns></returns>
        //public object Clone()
        //{
        //    return SerializeHelpher.Clone<DeepClone>(this);
        //}

        public object Clone()
        {
            return ReflectorHelpher.Clone<DeepClone>(this);
            // IFormatterConverter
            //  ICustomFormatter
            // IFormatter
            //IFormattable
            // IFormatProvider

            //string.Format
        }
    }

    [Serializable]
    public class BirthDay
    {
        public DateTime Birth { get; set; }
    }

    [Serializable]
    public class Company
    {
        public DateTime? InCompanyTime { get; set; }

        public DateTime? OutCompanyTime { get; set; }

        public string CompanyName { get; set; }
    }

    public class SerializeHelpher
    {
        public static object Clone<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.All));

                formatter.Serialize(stream, obj);

                stream.Seek(0, SeekOrigin.Begin);

                return formatter.Deserialize(stream);
            }
        }
    }

    public class ReflectorHelpher
    {
        public static object Clone<T>(T obj)
        {
            Type targetType = obj.GetType();

            var instance = Activator.CreateInstance(targetType);

            var members = targetType.GetMembers();

            foreach (MemberInfo member in members)
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = member as PropertyInfo;

                    if (property.CanWrite)
                    {
                        Type type = property.PropertyType;

                        var propertyValue = property.GetValue(obj);

                        if (property.PropertyType.IsEnum || property.PropertyType.IsValueType || property.PropertyType.Equals(typeof(System.String)))
                        {
                            property.SetValue(instance, propertyValue, null);
                        }
                        else
                        {
                            property.SetValue(instance, Clone(propertyValue), null);
                        }
                    }

                }
            }
            return instance;
        }
    }
}
