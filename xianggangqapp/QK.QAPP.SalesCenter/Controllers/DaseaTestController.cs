using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;


namespace QK.QAPP.SalesCenter.Controllers
{
    public class DaseaTestController : Controller
    {
        static Dictionary<long, IUnitOfWork> unitOfWorkColloct;

        static DaseaTestController()
        {
            unitOfWorkColloct = new Dictionary<long, IUnitOfWork>();
        }

        public ActionResult Index()
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(
                string.Format(" APP_CONTACTSERVICE.UnitOfWork.Equals(Ioc.GetService<IUnitOfWork>()) {0}, count {1}"
                , DformFormbuilder.UnitOfWork.Equals(Ioc.GetService<IUnitOfWork>()), unitOfWorkColloct.Count));

            sb.AppendLine(string.Format(" APP_CONTACTSERVICE.UnitOfWork.Equals(APP_CUSTOMERSERVICE.UnitOfWork) {0}",

                DformFormbuilder.UnitOfWork.Equals(DformForminfo.UnitOfWork)));


          

            ViewBag.IocGetServiceTestMessage = sb.ToString();


            Stopwatch watch1 = new Stopwatch();

            if (!unitOfWorkColloct.ContainsKey(DformFormbuilder.UnitOfWork.GetHashCode()))
            {
                unitOfWorkColloct.Add(DformFormbuilder.UnitOfWork.GetHashCode(), DformFormbuilder.UnitOfWork);
            }

            ViewBag.TransactionScopeTestMessage = "分布式事务测试：";
            watch1.Start();
            ViewBag.TransactionScopeTestMessage = ViewBag.TransactionScopeTestMessage+TransactionScopeTest();
            watch1.Stop();
            ViewBag.TransactionScopeTestTime = watch1.ElapsedMilliseconds;
            ViewBag.RepositoryTransactionTestMessage = "仓储事务测试：";
            ViewBag.RepositoryUnitWorkSelectTestMessage = "仓储查询测试：";
            ViewBag.RepositoryBaseTransactionTestMessage = "基于仓储基类测试：";

            if (unitOfWorkColloct.Count % 4 == 1)
            {
                watch1.Reset();
                watch1.Start();
                ViewBag.RepositoryTransactionTestMessage = ViewBag.RepositoryTransactionTestMessage + RepositoryTransactionTest();
                watch1.Stop();
                ViewBag.RepositoryTransactionTestTime = watch1.ElapsedMilliseconds;
            }
            else if (unitOfWorkColloct.Count % 4 == 2)
            {
                ViewBag.RepositoryUnitWorkSelectTestMessage = ViewBag.RepositoryUnitWorkSelectTestMessage + RepositoryUnitWorkSelectTest();
            }
            else if (unitOfWorkColloct.Count % 4 == 3)
            {
                ViewBag.RepositoryBaseTransactionTestMessage = ViewBag.RepositoryBaseTransactionTestMessage + RepositoryBaseTransactionTest();
            }
            else 
            {
                ViewBag.RepositoryBaseTransactionTestMessage = ViewBag.RepositoryBaseTransactionTestMessage + RepositoryBaseTransactionTest();

                ViewBag.RepositoryUnitWorkSelectTestMessage = ViewBag.RepositoryUnitWorkSelectTestMessage + RepositoryUnitWorkSelectTest();

            }

          

        


            //ViewBag.IocGetServiceTestMessage = IocGetServiceTest();


            return View();
        }


        class RepositoryTransactionData
        {
            public int Name;
            public long CreationTime;
            public long HashCode;
            public int ThreadNum;

            public IRepositoryTransaction RepositoryTransaction;
        }

        private string IocGetServiceTest()
        {
            var sb = new StringBuilder();
            var taskArray = new Task<RepositoryTransactionData>[10000];
            var dataCollection = new List<RepositoryTransactionData>();
            for (int i = 0; i < taskArray.Length; i++)
            {
                int i1 = i;
                taskArray[i] = Task.Factory.StartNew<RepositoryTransactionData>(obj =>
                {
                    var repositoryTransaction = Ioc.GetService<IRepositoryTransaction>();
                    var data = new RepositoryTransactionData();
                    if (repositoryTransaction != null)
                    {
                        data = new RepositoryTransactionData
                        {
                            Name = i1,
                            CreationTime = DateTime.Now.Ticks,
                            HashCode = repositoryTransaction.GetHashCode(),
                            RepositoryTransaction = repositoryTransaction,
                            ThreadNum = Thread.CurrentThread.ManagedThreadId
                        };
                        //sb.AppendLine(string.Format(" #Task# #{0} created at {1} on thread #{2} hashcode# {3}",data.Name, data.CreationTime, data.ThreadNum, data.HashCode));
                        return data;
                    }
                    else
                    {
                        return data;
                    }

                },
                                                     i);
            }

            Task.WaitAll(taskArray);

            for (int i = 0; i < taskArray.Length; i++)
            {
                dataCollection.Add(taskArray[i].Result);
            }


            var lookup = (Lookup<long, RepositoryTransactionData>)dataCollection.ToLookup(p => p.HashCode, p => p);
            foreach (IGrouping<long, RepositoryTransactionData> packageGroup in lookup)
            {
                if (packageGroup.ToArray().Length > 1)
                {
                    sb.AppendLine(string.Format("同一个 IRepositoryTransaction Hashcode{0} 产生 {1} 条,dataCollection条数{2} ", packageGroup.Key.ToString(), packageGroup.ToArray<RepositoryTransactionData>().Length, dataCollection.Count));

                    RepositoryTransactionData data1 = null;
                    foreach (RepositoryTransactionData data in packageGroup)
                    {
                        sb.AppendLine(string.Format(" #Task# #{0} created at {1} on thread #{2} hashcode# {3}", data.Name, data.CreationTime, data.ThreadNum, data.HashCode));
                        if (data1 == null)
                        {
                            data1 = data;
                        }
                        else
                        {
                            if (data1.RepositoryTransaction != null && data.RepositoryTransaction != null)
                            {
                                if (data1.RepositoryTransaction.Equals(data.RepositoryTransaction))
                                {
                                    sb.AppendLine(string.Format(" 是同一个对象 "));
                                }
                                else
                                {
                                    sb.AppendLine(string.Format(" 不是同一个对象 "));
                                }
                            }
                        }


                    }
                    sb.AppendLine();
                }
            }

            foreach (IGrouping<long, long> packageGroup in (Lookup<long, long>)dataCollection.ToLookup(p => p.CreationTime, p => p.HashCode))
            {


                if (packageGroup.ToArray<long>().Length > 1)
                {

                    sb.AppendLine("同一时间创建的IOC.Create IRepositoryTransaction 对象实例时间：" + packageGroup.Key + "#条数# " + packageGroup.ToArray<long>().Length.ToString());
                    sb.AppendLine();
                    foreach (long hashcode in packageGroup)
                    {
                        sb.Append(hashcode + ",");
                    }

                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 分布式事务测试，性能
        /// </summary>
        /// <returns></returns>
        private string TransactionScopeTest()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    APP_DFORM_FORMBUILDER dformFormbuilder = new APP_DFORM_FORMBUILDER(false)
                    {
                        ID = 5,
                        NAME = "dasea 测试"
                    };

                    APP_DFORM_FORMINFO dformForminfo = new APP_DFORM_FORMINFO(false)
                    {
                        ID = 100,
                        FB_ID = 100,
                        NAME = "dasea 测试"
                    };


                    var formbuilderservice = Ioc.GetService<IAPP_DFORM_FORMBUILDERSERVICE>();
                    formbuilderservice.Add(dformFormbuilder);

                    var forminfoservice = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();
                    forminfoservice.Add(dformForminfo);

                    formbuilderservice.UnitOfWork.SaveChanges();

                    forminfoservice.UnitOfWork.SaveChanges();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


            return "执行成功";


        }

        /// <summary>
        /// 仓储事务测试
        /// </summary>
        /// <returns></returns>
        private string RepositoryTransactionTest()
        {
            try
            {
                using (IRepositoryTransaction repositoryTransaction = Ioc.GetService<IRepositoryTransaction>())
                {
                    APP_DFORM_FORMBUILDER dform_formbuilder = new APP_DFORM_FORMBUILDER(false)
                    {
                        ID = 4,
                        NAME = "dasea 测试"
                    };

                    APP_DFORM_FORMINFO dform_forminfo = new APP_DFORM_FORMINFO(false)
                    {
                        ID = 99,
                        FB_ID = 100,
                        NAME = "dasea 测试"
                    };

                    var formbuilderResponse = repositoryTransaction.GetRepository<APP_DFORM_FORMBUILDER>();
                    formbuilderResponse.Add(dform_formbuilder);

                    var dformforminfoResponse = repositoryTransaction.GetRepository<APP_DFORM_FORMINFO>();
                    dformforminfoResponse.Add(dform_forminfo);

                    repositoryTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "执行成功";

        }


        /// <summary>
        ///  基于仓储基类事务测试
        /// </summary>
        /// <returns></returns>
        private string RepositoryBaseTransactionTest()
        {
            try
            {
                //using (DFORM_FORMBUILDER.UnitOfWork)
                //{
                    APP_DFORM_FORMBUILDER dformFormbuilder = new APP_DFORM_FORMBUILDER(false)
                    {
                        ID = 4,
                        NAME = "dasea 测试"
                    };

                    APP_DFORM_FORMINFO dformForminfo = new APP_DFORM_FORMINFO(false)
                    {
                        ID = 99,
                        FB_ID = 100,
                        NAME = "dasea 测试"
                    };
                    DformFormbuilder.Add(dformFormbuilder);
                    DformForminfo.Add(dformForminfo);
                    DformFormbuilder.UnitOfWork.SaveChanges();

                //}
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "执行成功";

        }

        [Dependency]
        public IAPP_DFORM_FORMBUILDERSERVICE DformFormbuilder
        {
            get;
            set;
        }

        [Dependency]
        public IAPP_DFORM_FORMINFOSERVICE DformForminfo
        {
            get;
            set;
        }

        /// <summary>
        /// 仓储查询测试1
        /// </summary>
        /// <returns></returns>
        private string RepositoryUnitWorkSelectTest()
        {

            StringBuilder sb = new StringBuilder();
            //一旦 使用 using(APP_CONTACTSERVICE.UnitOfWork),using 后面的语句在使用 数据操作动作，将出错。

            try
            {

                var data = DformForminfo.Find(p => p.ID > 1 && p.ID < 1000).ToList < APP_DFORM_FORMINFO>();
                var data2 = DformFormbuilder.Find(p => p.ID > 1 && p.ID < 1000).ToList<APP_DFORM_FORMBUILDER>();
             
                sb.AppendLine(string.Format(" 第一次使用 IRepositoryBase正常"));
            }
            catch (Exception ex)
            {
                sb.AppendLine(string.Format(" 第一次使用 IRepositoryBase，错误信息：{0}", ex));
            }

            try
            {
                var data3 = DformFormbuilder.Find(p => p.ID == 1);
                sb.AppendLine(string.Format(" 第二次使用 IRepositoryBase正常"));
            }
            catch (Exception ex)
            {
                sb.AppendLine(string.Format(" 第二次使用 IRepositoryBase，错误信息：{0}", ex));
            }


            return sb.ToString();
        }



    }
}