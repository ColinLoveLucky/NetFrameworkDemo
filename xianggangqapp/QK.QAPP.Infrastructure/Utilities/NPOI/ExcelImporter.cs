using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QK.QAPP.Entity;

namespace QK.QAPP.Infrastructure
{
    public class ExcelImporter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="stream">文件流</param>
        /// <param name="ignoreFirstLine">是否忽略首行</param>
        /// <param name="message">提示信息</param>
        /// <param name="propertys">与Excel中列对应的属性集合</param>
        /// <returns></returns>
        public static IList<T> ReadListFromStream<T>(string fileName, Stream stream, bool ignoreFirstLine, out string message, params PropertyInfo[] propertys)
    where T : new()
        {
            message = String.Empty;
            string ext = Path.GetExtension(fileName);
            string extendsion = ext != null ? ext.TrimStart('.') : String.Empty;
            string parseMsg = "【第{0}行第{1}列数据类型转换错误！类型：{2}】";

            IWorkbook workBook = null;
            //根据Excel版本创建Workbook
            switch (extendsion)
            {
                case "xls":
                    workBook = new HSSFWorkbook(stream);
                    break;
                case "xlsx":
                    workBook = new XSSFWorkbook(stream);
                    break;
            }

            if (workBook == null || workBook.NumberOfSheets <= 0)
            {
                throw new Exception("Excel表格工作簿为空！");
            }

            IList<T> list = new List<T>();
            for (int i = 0; i < workBook.NumberOfSheets; i++)
            {
                //第i个Sheet
                ISheet sheet = workBook.GetSheetAt(i);

                if (sheet.PhysicalNumberOfRows > 0)
                {
                    if (!ignoreFirstLine)
                    {
                        //检查列是否与ExcelAttribute定义的一致
                        ValidTableHeader<T>(sheet);
                    }

                    for (int j = ignoreFirstLine ? 1 : 0; j < sheet.PhysicalNumberOfRows; j++)
                    {
                        //第j行
                        var row = sheet.GetRow(j);

                        T entity = new T();

                        //var propertys = typeof(T).GetProperties();

                        for (int k = 0; k < propertys.Length; k++)
                        {
                            //var excel = Attribute.GetCustomAttribute(p, typeof(ExcelAttribute)) as ExcelAttribute;
                            //第k个单元格
                            var cellValue = row.GetCell(k);

                            if (cellValue == null || string.IsNullOrEmpty(cellValue.ToString()))
                            {
                                message += string.Format("【第{0}行第{1}列数据为空！】", j + 1, k + 1);
                                //return list;
                                continue;
                            }

                            string cellValueStr = cellValue.ToString();
                            if (propertys[k].PropertyType == typeof(string))
                            {
                                propertys[k].SetValue(entity, cellValueStr, null);
                            }
                            else if (propertys[k].PropertyType == typeof(int) || propertys[k].PropertyType == typeof(int?))
                            {
                                int temp;
                                if (!int.TryParse(cellValueStr, out temp))
                                {
                                    message += string.Format(parseMsg, j + 1, k + 1, "int");
                                    //return list;
                                    continue;
                                }
                                propertys[k].SetValue(entity, propertys[k].PropertyType == typeof(int) ? temp : (int?)temp, null);
                            }
                            else if (propertys[k].PropertyType == typeof(decimal) || propertys[k].PropertyType == typeof(decimal?))
                            {
                                decimal temp;
                                if (!decimal.TryParse(cellValueStr, out temp))
                                {
                                    message += string.Format(parseMsg, j + 1, k + 1, "decimal");
                                    //return list;
                                    continue;
                                }
                                propertys[k].SetValue(entity, propertys[k].PropertyType == typeof(decimal) ? temp : (decimal?)temp, null);
                            }
                            else if (propertys[k].PropertyType == typeof(DateTime) || propertys[k].PropertyType == typeof(DateTime?))
                            {
                                DateTime temp;
                                if (!DateTime.TryParse(cellValueStr, out temp))
                                {
                                    message += string.Format(parseMsg, j + 1, k + 1, "DateTime");
                                    //return list;
                                    continue;
                                }
                                propertys[k].SetValue(entity, propertys[k].PropertyType == typeof(DateTime) ? temp : (DateTime?)temp, null);
                            }
                            else if (propertys[k].PropertyType == typeof(bool) || propertys[k].PropertyType == typeof(bool?))
                            {
                                bool temp;
                                if (!bool.TryParse(cellValueStr, out temp))
                                {
                                    message += string.Format(parseMsg, j + 1, k + 1, "bool");
                                    //return list;
                                    continue;
                                }
                                propertys[k].SetValue(entity, propertys[k].PropertyType == typeof(bool) ? temp : (bool?)temp, null);
                            }
                            else
                            {
                                message += string.Format(parseMsg, j + 1, k + 1, "未知");
                                //return list;
                                continue;
                            }
                        }
                        list.Add(entity);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 检查表头与定义是否匹配
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private static void ValidTableHeader<T>(ISheet sheet) where T : new()
        {
            var firstRow = sheet.GetRow(0);

            var propertys = typeof(T).GetProperties();

            foreach (var p in propertys)
            {
                var excel = Attribute.GetCustomAttribute(p, typeof(ExcelAttribute)) as ExcelAttribute;

                if (excel != null)
                {
                    if (!firstRow.GetCell(excel.ColumnIndex).StringCellValue.Trim().Equals(excel.ColumnName))
                    {
                        //todo:throw new NPOIException(string.Format("Excel表格第{0}列标题应为{1}", excel.ColumnIndex + 1, excel.ColumnName));
                    }
                }
            }
        }

        public static IList<T> ReadListAllowEmptyCell<T>(ExcelImportPara para, out string message)
            where T : new()
        {
            message = String.Empty;
            string ext = Path.GetExtension(para.FileName);
            string extendsion = ext != null ? ext.TrimStart('.') : String.Empty;
            string parseMsg = "【第{0}行第{1}列数据类型转换错误！类型：{2}】";

            IWorkbook workBook = null;
            //根据Excel版本创建Workbook
            switch (extendsion)
            {
                case "xls":
                    workBook = new HSSFWorkbook(para.Stream);
                    break;
                case "xlsx":
                    workBook = new XSSFWorkbook(para.Stream);
                    break;
            }

            if (workBook == null || workBook.NumberOfSheets <= 0)
            {
                throw new Exception("Excel表格工作簿为空！");
            }

            IList<T> list = new List<T>();
            //按工作簿中的Sheet依次处理
            for (int i = 0; i < workBook.NumberOfSheets; i++)
            {
                ISheet sheet = workBook.GetSheetAt(i);
                if (sheet.PhysicalNumberOfRows > 0)
                {
                    for (int j = para.IgnoreFirstLine ? 1 : 0; j < sheet.PhysicalNumberOfRows; j++)
                    {
                        //当前行
                        var row = sheet.GetRow(j);
                        T entity = new T();
                        for (int k = 0; k < para.Propertys.Length; k++)
                        {
                            var cell = row.GetCell(k);

                            if (cell == null || string.IsNullOrEmpty(cell.ToString()))
                                continue;

                            string cellValueStr = String.Empty;
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                case CellType.String:
                                case CellType.Unknown:
                                    cellValueStr = cell.StringCellValue;
                                    break;
                                case CellType.Boolean:
                                    cellValueStr = cell.BooleanCellValue.ToString();
                                    break;
                                case CellType.Error:
                                    cellValueStr = cell.ErrorCellValue.ToString();
                                    break;
                                case CellType.Formula:
                                        cellValueStr = cell.CellFormula;
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(cell))
                                        cellValueStr = cell.DateCellValue.ToString(CultureInfo.InvariantCulture);
                                    else
                                        cellValueStr = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                            }
                            if (para.Propertys[k].PropertyType == typeof(string))
                            {
                                para.Propertys[k].SetValue(entity, cellValueStr, null);
                            }
                            else if (para.Propertys[k].PropertyType == typeof(int) || para.Propertys[k].PropertyType == typeof(int?))
                            {
                                int temp;
                                if (!int.TryParse(cellValueStr, out temp))
                                {
                                    message += string.Format(parseMsg, j + 1, k + 1, "int");
                                    //return list;
                                    continue;
                                }
                                para.Propertys[k].SetValue(entity, para.Propertys[k].PropertyType == typeof(int) ? temp : (int?)temp, null);
                            }
                            else if (para.Propertys[k].PropertyType == typeof(decimal) || para.Propertys[k].PropertyType == typeof(decimal?))
                            {
                                decimal temp;
                                if (!decimal.TryParse(cellValueStr, out temp))
                                {
                                    message += string.Format(parseMsg, j + 1, k + 1, "decimal");
                                    //return list;
                                    continue;
                                }
                                para.Propertys[k].SetValue(entity, para.Propertys[k].PropertyType == typeof(decimal) ? temp : (decimal?)temp, null);
                            }
                            else if (para.Propertys[k].PropertyType == typeof(DateTime) || para.Propertys[k].PropertyType == typeof(DateTime?))
                            {
                                DateTime temp;
                                if (!DateTime.TryParse(cellValueStr, out temp))
                                {
                                    message += string.Format(parseMsg, j + 1, k + 1, "DateTime");
                                    //return list;
                                    continue;
                                }
                                para.Propertys[k].SetValue(entity, para.Propertys[k].PropertyType == typeof(DateTime) ? temp : (DateTime?)temp, null);
                            }
                            else if (para.Propertys[k].PropertyType == typeof(bool) || para.Propertys[k].PropertyType == typeof(bool?))
                            {
                                bool temp;
                                if (!bool.TryParse(cellValueStr, out temp))
                                {
                                    message += string.Format(parseMsg, j + 1, k + 1, "bool");
                                    //return list;
                                    continue;
                                }
                                para.Propertys[k].SetValue(entity, para.Propertys[k].PropertyType == typeof(bool) ? temp : (bool?)temp, null);
                            }
                            else
                            {
                                message += string.Format(parseMsg, j + 1, k + 1, "未知");
                                //return list;
                                continue;
                            }
                        }
                        list.Add(entity);
                    }
                }
            }
            return list;
        }

    }
}
