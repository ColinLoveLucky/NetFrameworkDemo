using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;

namespace QK.QAPP.Services
{
    public partial class APP_CARPRICESERVICE
    {
        [Dependency]
        public IQFUserService UserService { get; set; }

        public ViewListByPage<APP_CARPRICE> GetCarList(CarListSearchPara para, int pageIndex, int pageSize)
        {
            ViewListByPage<APP_CARPRICE> carList = new ViewListByPage<APP_CARPRICE>();
            IQueryable<APP_CARPRICE> query;
            query = Find(c =>
                (c.BRAND.ToLower().IndexOf(para.CarBrand.ToLower()) > -1 || string.IsNullOrEmpty(para.CarBrand))
                && (c.SERIES.ToLower().IndexOf(para.CarSeries.ToLower()) > -1 || string.IsNullOrEmpty(para.CarSeries))
                && (c.STYLE.ToLower().IndexOf(para.CarStyle.ToLower()) > -1 || string.IsNullOrEmpty(para.CarStyle)))
                .OrderBy(c => c.ID);

            carList.SetParameters(query, pageIndex, pageSize);
            return carList;
        }

        public string ImportFromExcel(System.Web.HttpRequestBase request)
        {
            string returnMsg = String.Empty;
            string userAccount = String.Empty;

            var currentUser = UserService.GetCurrentUser();
            if (currentUser != null)
            {
                userAccount = currentUser.Account;
            }

            if (request.Files.Count == 0)
            {
                returnMsg = "抱歉，没有找到可上传的文件！";
            }
            else
            {
                var file = request.Files[0];
                var type = typeof(APP_CARPRICE);

                try
                {
                    var list = ExcelImporter.ReadListFromStream<APP_CARPRICE>(file.FileName, file.InputStream, true,
                        out returnMsg,
                        new PropertyInfo[]
                        {
                            type.GetProperty("BRAND"),
                            type.GetProperty("SERIES"),
                            type.GetProperty("YEAR"),
                            type.GetProperty("STYLE"),
                            type.GetProperty("TRANSMISSION_KIND"),
                            type.GetProperty("GEAR"),
                            type.GetProperty("SHAPE"),
                            type.GetProperty("NEW_PRICE")
                        });

                    foreach (var item in list)
                    {
                        item.CREATED_TIME = DateTime.Now;
                        item.CREATED_USER = userAccount;
                        item.CHANGED_TIME = DateTime.Now;
                        item.CHANGED_USER = userAccount;
                    }

                    if (string.IsNullOrEmpty(returnMsg))
                    {
                        //this.AddMultiple(list.ToList());
                        //this.UnitOfWork.SaveChanges();
                        AddMultipleBySql(list.ToList());
                        returnMsg = string.Format("导入车辆信息数据成功，总条数：{0}", list.Count);
                        Infrastructure.Log4Net.LogWriter.Biz(returnMsg);
                    }
                    else
                    {
                        Infrastructure.Log4Net.LogWriter.Error(returnMsg);
                    }
                }
                catch (Exception ex)
                {
                    returnMsg += "车辆信息导入出错！";
                    Infrastructure.Log4Net.LogWriter.Error(returnMsg, ex);
                }
            }

            return returnMsg;
        }

        public string DeleteAll()
        {
            string msg = String.Empty;
            try
            {
                this.ExecuteSqlCommand("truncate table APP_CARPRICE");
                //this.UnitOfWork.SaveChanges();
                Infrastructure.Log4Net.LogWriter.Biz("清空车辆数据成功！");
            }
            catch (Exception ex)
            {
                msg = "清空车辆信息数据出错！";
                Infrastructure.Log4Net.LogWriter.Error(msg, ex);
            }

            return msg;
        }

        public void AddMultipleBySql(List<APP_CARPRICE> list)
        {
            //计数器
            int count = 1;
            //序列
            Int64 sequence = 1;
            StringBuilder sql = new StringBuilder();
            List<object> paras = new List<object>();
            int totalCount = list.Count;
            foreach (var item in list)
            {
                if (count == 1)
                {
                    sql.Append(" INSERT INTO APP_CARPRICE ");
                    sql.Append(
                        " (ID, BRAND, SERIES, YEAR, SWEPT_VOLUME, STYLE, TRANSMISSION_KIND, GEAR, SHAPE, STYLE_YEAR, AVG_WHOLESALE_PRICE, AVG_RETAIL_PRICE, WELL_WHOLESALE_PRICE, WELL_RETAIL_PRICE, NEW_PRICE, ENABLED, CHANGED_TIME, CHANGED_USER, CREATED_TIME, CREATED_USER) ");
                }
                if (count != 1)
                {
                    sql.Append(" UNION ALL ");
                }
                sql.Append(" SELECT :ID, :BRAND, :SERIES, :YEAR, :SWEPT_VOLUME, :STYLE, :TRANSMISSION_KIND, :GEAR, :SHAPE, :STYLE_YEAR, :AVG_WHOLESALE_PRICE, :AVG_RETAIL_PRICE, :WELL_WHOLESALE_PRICE, :WELL_RETAIL_PRICE, :NEW_PRICE, :ENABLED, :CHANGED_TIME, :CHANGED_USER, :CREATED_TIME, :CREATED_USER FROM DUAL ");

                paras.Add(sequence);
                paras.Add(item.BRAND);
                paras.Add(item.SERIES);
                paras.Add(item.YEAR);
                paras.Add(item.SWEPT_VOLUME);
                paras.Add(item.STYLE);
                paras.Add(item.TRANSMISSION_KIND);
                paras.Add(item.GEAR);
                paras.Add(item.SHAPE);
                paras.Add(item.STYLE_YEAR);
                paras.Add(item.AVG_WHOLESALE_PRICE);
                paras.Add(item.AVG_RETAIL_PRICE);
                paras.Add(item.WELL_WHOLESALE_PRICE);
                paras.Add(item.WELL_RETAIL_PRICE);
                paras.Add(item.NEW_PRICE);
                paras.Add(item.ENABLED);
                paras.Add(item.CHANGED_TIME);
                paras.Add(item.CHANGED_USER);
                paras.Add(item.CREATED_TIME);
                paras.Add(item.CREATED_USER);

                sequence++;

                if (count >= 500 || sequence > totalCount)
                {
                    //每500条数据提交一次
                    ExecuteSqlCommand(sql.ToString(), paras.ToArray());
                    //计数器归零
                    count = 1;
                    //重置参数
                    paras.Clear();
                    sql.Clear();
                }
                else
                {
                    count++;
                }
            }
        }
    }
}
