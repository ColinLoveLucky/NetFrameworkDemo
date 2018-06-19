using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC5AutoFac.Controllers
{
    public class AutoFacController : Controller
    {
        private IDataSource _datasource;
        //public AutoFacController(IDataSource datasource)
        //{
        //    _datasource = datasource;
        //}
        public IDataSource DataSource { get; set; }
        // GET: AutoFac
        public ActionResult Index()
        {
            return View();
        }
        public string ReturnContent()
        {
            var datasource = AutofacDependencyResolver.Current.GetService<IDataSource>().Write();
            return DataSource.Write();
        }
    }
    public class StreamControl : IController
    {
        public string Write()
        {
            return "Hello World!";
        }
        public void Execute(RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }
    public interface IDependency
    {
    }
    public interface IDataSource : IDependency
    {
        string Write();
    }
    public class XmlDataSource : IDataSource
    {
        public string Write()
        {
            return "xml datasource";
        }
    }
}