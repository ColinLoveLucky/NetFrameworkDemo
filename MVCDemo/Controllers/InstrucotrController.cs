using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCDemo.Models;
using System.Web.Routing;
using System.ComponentModel;
using System.Diagnostics;

namespace MVCDemo.Controllers
{
    /// <summary>
    /// Azure 部署 网站 部署 SQLSERVER
    /// Microsoft Azure
    ///  https://www.asp.net/mvc/overview/getting-started/recommended-resources-for-mvc
    /// </summary>
    public class InstrucotrController : Controller
    {
		private DataBaseContext db = new DataBaseContext();

        // GET: /Instrucotr/
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.Instructors.ToListAsync());
        //}

     
        public ActionResult Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = db.Instructors
                .OrderBy(i => i.LastName);

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(
                    i => i.ID == id.Value).Single().Courses;
            }

            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                viewModel.Enrollments = viewModel.Courses.Where(
                    x => x.CourseId == courseID).Single().Enrollments;
            }
            return View(viewModel);
        }


        // GET: /Instrucotr/Details/5
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Commenting out original code to show how to use a raw SQL query.
            //Department department = await db.Departments.FindAsync(id);

            // Create and execute raw SQL query.
            string query = "SELECT * FROM Instructors WHERE ID = @p0";
            Instructor instructor = await db.Instructors.SqlQuery(query, id).SingleOrDefaultAsync();

            //     IQueryable<Course> courses = db.Courses
            //.Where(c => !SelectedDepartment.HasValue || c.DepartmentID == departmentID)
            //.OrderBy(d => d.CourseID)
            //.Include(d => d.Department);

            //db.Database.ExecuteSqlCommand("SELECT * FROM Instructors WHERE ID = @p0", id);

            //Instructor instructor = await db.Instructors.FindAsync(id);
            //DbSet.Find
            //DbSet.Local
            //DbSet.Remove
            //DbSet.Add
            //DbSet.Attach
            //DbContext.SaveChanges
            //DbContext.GetValidationErrors
            //DbContext.Entry
            //DbChangeTracker.Entries

            if (instructor == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", instructor.Courses.First().CourseId);
            return View(instructor);
        }

        // GET: /Instrucotr/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title");

            return View();
        }

        // POST: /Instrucotr/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,LastName,FirstMidName,HireDate,CourseId")] Instructor instructor)
        {
            var errors = DataAnnotationsValidationRunner.GetErrors(instructor);
            if (errors.Any())
            {
                errors.ToList().ForEach(m =>
                    {
                        ModelState.AddModelError(m.p1, m.p2);
                    });
            }
            else
            {
                var course = db.Courses.Find(new object[] { instructor.CourseId });
                instructor.Courses = new List<Course>(){
                    db.Courses.Find(new object[]{instructor.CourseId})
                };
                db.Instructors.Add(instructor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            //HttpContextBase
            //HttpContext
            /*Route
            //Route
            //DynamicDataRoute
            //RouteBase
            //RouteTable
            //RouteCollection
            //RouteCollectionExtensions
            //RouteData
            //RequestContext
            //StopRoutingHandler
            //PageRouteHandler
            //MvcRouteHandler
            //RouteValueDictionary
            //VirtualPathData
            //UrlRoutingModule
            //MvcHandler
            //Route
            //RouteValueDictionary
            //RouteCollection 
            //RouteData
            //RouteBase
            //Route
            //RouteTable
            //StopRoutingHandler 
            //routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
            //RouteCollection
            //MvcRouteHandler
            //PageRouteHandler */

            //MVC Filter
            // IAuthorizationFilter
            // AuthorizationContext
            //ControllerContext
            //RequestContext
            //ControllerBase
            //  ControllerContext
            // ViewDataDictionary
            //ModelMetadata
            //ModelMetadataProvider
            // ViewDataDictionary
            // ViewDataInfo
            //PropertyDescriptor
            // MemberDescriptor
            // AuthorizeAttribute 
            //FilterAttribute
            // RequireHttpsAttribute 

            // ActionFilterAttribute
            // IActionFilter 
            //ActionExecutedContext
            //ParameterDescriptor
            //ActionDescriptor
            //ActionResult
            //ActionExecutingContext

            //IResultFilter
            //ResultExecutedContext
            //ResultExecutingContext
            // OutputCacheAttribute

            //IExceptionFilter
            //ExceptionContext
            //HandleErrorAttribute

            //FilterProviders
            //FilterProviderCollection
            //Filter
            // FilterScope

            //Identity
            //https://www.asp.net/mvc/overview/security

            //Async Method
            //https://www.asp.net/mvc/overview/performance/using-asynchronous-methods-in-aspnet-mvc-4

            //Builder
            //https://www.asp.net/mvc/overview/performance/bundling-and-minification

            //GlobalFilterCollection 

            //OAuthWebSecurity
            //https://www.asp.net/mvc/overview/getting-started/recommended-resources-for-mvc Security
            //OwinStartup
            //https://www.asp.net/mvc/overview/getting-started/recommended-resources-for-mvc OwinStartup

            //Globalization, Internationalization and Localization
            //http://afana.me/post/aspnet-mvc-internationalization.aspx

            //Views
            //Razor
            //https://www.asp.net/web-pages/overview/getting-started/introducing-razor-syntax-c

            //WebForm 区别 MVC
            //https://www.asp.net/web-pages/overview/getting-started/aspnet-web-pages-razor-faq

            //MVC SendMial
            //https://www.asp.net/web-pages/overview/getting-started/11-adding-email-to-your-web-site

            //Link Site
            //https://www.asp.net/web-pages/overview/getting-started/13-adding-social-networking-to-your-web-site

            //WebMatrix
            //https://www.asp.net/web-pages/overview/getting-started/introducing-aspnet-web-pages-2/getting-started

            //Using Page Inspector in ASP.NET MVC
            //https://www.asp.net/mvc/overview/views/using-page-inspector-in-aspnet-mvc

            //Dynamic v. Strongly Typed Views
            //https://www.asp.net/mvc/overview/views/dynamic-v-strongly-typed-views

            //Bootstrap
            //30 Days of Bootstrap with the MVC Framework

            //Debug
            //https://docs.microsoft.com/en-us/azure/app-service-web/web-sites-dotnet-troubleshoot-visual-studio

            //Realease
            //https://www.asp.net/mvc/overview/releases/whats-new-in-aspnet-mvc-52

            //MVC Old Version


            if (ModelState.IsValid)
            {
                var course = db.Courses.Find(new object[] { instructor.CourseId });
                instructor.Courses = new List<Course>(){
                    db.Courses.Find(new object[]{instructor.CourseId})
                };
                db.Instructors.Add(instructor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title");
            return View(instructor);
        }

        // GET: /Instrucotr/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = await db.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", instructor.Courses.First().CourseId);
            return View(instructor);
        }

        // POST: /Instrucotr/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,LastName,FirstMidName,HireDate,CourseId")] Instructor instructor)
        {
            ;
            if (ModelState.IsValid)
            {
                var entry = db.Instructors.Find(instructor.ID);
                db.Instructors.Remove(entry);

                var course = db.Courses.Find(new object[] { instructor.CourseId });
                instructor.Courses = new List<Course>(){
                    db.Courses.Find(new object[]{instructor.CourseId})
                };
                db.Instructors.Add(instructor);

                // db.Instructors.Find(instructor.ID).Courses.First().CourseId = instructor.CourseId;
                //  db.Entry(instructor).State = EntityState.Modified;


                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", instructor.Courses.First().CourseId);
            return View(instructor);
        }

        // GET: /Instrucotr/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = await db.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: /Instrucotr/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Instructor instructor = await db.Instructors.FindAsync(id);
            db.Instructors.Remove(instructor);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
