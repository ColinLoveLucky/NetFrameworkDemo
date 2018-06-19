
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
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace MVCDemo.Controllers
{
    public class CourseController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        private async Task DelayAsync()
        {
            // await Task.Delay(1000).ConfigureAwait(continueOnCapturedContext: false);
            await Task.Delay(1000);
        }

        // This method causes a deadlock when called in a GUI or ASP.NET context.
        public async void Test()
        {
            // Start the delay.
            var delayTask = DelayAsync();
            //Task.Run(() =>
            //{
            //    var contextUrl = System.Web.HttpContext.Current.Request.Url;     
            //}).ConfigureAwait(true).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                await DelayAsync();
                //var contextUrl = System.Web.HttpContext.Current.Request.Url;
            }).Wait();


        }

        async Task DoSthSync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        void FirstThing()
        {
            Task task = DoSthSync();

            task.Wait();
        }

        // GET: /Course/
        public async Task<ActionResult> Index()
        {

            IQueryable t = from item in db.Courses
                           select item;

            var provider = t.Provider;

            var expression = t.Expression;

            var expType = t.ElementType;

            return View(await db.Courses.ToListAsync());
        }
        // GET: /Course/Details/5
      
        public async Task<ActionResult> Details(int? id)
        {
            //int age = 100;

           

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        // GET: /Course/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: /Course/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseId,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(course);
        }
        // GET: /Course/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        // POST: /Course/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseId,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(course);
        }
        // GET: /Course/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Course course = await db.Courses.FindAsync(id);
            db.Courses.Remove(course);
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
