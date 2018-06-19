using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.DataTableExtensions
{
    public static class DataTableExtession
    {
        private static ConcurrentDictionary<string, List<MapTColumn>> _mapTCahce =
            new ConcurrentDictionary<string, List<MapTColumn>>();

        public static List<T> ConvertToList<T>(this DataTable value) where T : class,new()
        {

            List<T> result = new List<T>();
            T model = new T();
            if (value != null)
                foreach (DataRow item in value.Rows)
                {
                    model = CreateItem<T>(item);
                    result.Add(model);
                }
            return result;
        }

        private static T CreateItem<T>(DataRow row) where T : class ,new()
        {
            var result = Activator.CreateInstance<T>();

            var key = result.GetType().Name;

            if (_mapTCahce.ContainsKey(key))
            {
                var mapKeyValue = _mapTCahce[key];
                foreach (DataColumn column in row.Table.Columns)
                {
                    Type type = null;
                    PropertyInfo property = null;
                    MapTColumn mapTItem = null;
                    if (mapKeyValue.Exists(x => x.RightColumnName.ToString() == column.ColumnName.ToString()))
                    {
                        mapTItem = mapKeyValue.Where(x => x.RightColumnName.ToString() == column.ColumnName.ToString()).First();
                        property = mapTItem.Property;
                        type = mapTItem.PropertyType;
                        if (type == column.ColumnName.GetType())
                            property.SetValue(result, row[column.ColumnName.ToString()], null);
                        else
                            property.SetValue(result, Convert.ChangeType(row[column.ColumnName.ToString()], type), null);
                    }
                }
            }
            else
            {
                var properties = typeof(T).GetProperties();
                ColumnAttribute attri = null;
                Type type = null;
                List<MapTColumn> listMap = new List<MapTColumn>();
                listMap.Clear();
                MapTColumn mapIColumn = null;
                foreach (PropertyInfo item in properties)
                {
                    mapIColumn = new MapTColumn();
                    var attributes = item.GetCustomAttributes(false);
                    if (attributes != null)
                    {
                        attri = attributes[0] as ColumnAttribute;
                        type = attri.Type as Type;
                    }
                    if (row.Table.Columns.Contains(attri.Name.ToString()))
                        if (type == row[attri.Name.ToString()].GetType())
                            item.SetValue(result, row[attri.Name.ToString()], null);
                        else
                            item.SetValue(result, Convert.ChangeType(row[attri.Name.ToString()], type), null);
                    mapIColumn.Property = item;
                    mapIColumn.PropertyType = type;
                    mapIColumn.LeftMappingName = item.Name;
                    mapIColumn.RightColumnName = attri.Name.ToString();
                    listMap.Add(mapIColumn);
                }
                _mapTCahce.TryAdd(result.GetType().Name, listMap);
            }

            return result;
        }


        public static T CreateExpression<T>(DataRow row) where T : class,new()
        {
            T obj = default(T);

            foreach(DataColumn dc in row.Table.Columns)
            {

            }

            return obj;
        }
    }

    public class MapTColumn : ICloneable
    {
        public MapTColumn()
        {

        }

        public string LeftMappingName
        {
            get;
            set;
        }

        public string RightColumnName
        {
            get;
            set;
        }

        public PropertyInfo Property
        {
            get;
            set;
        }

        public Type PropertyType
        {
            get;
            set;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
