using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using OData.Models;

namespace OData.Controllers
{
    /*
    若要为此控制器添加路由，请将这些语句合并到 WebApiConfig 类的 Register 方法中。请注意 OData URL 区分大小写。

    using System.Web.Http.OData.Builder;
    using OData.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Department>("Department");
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    //public class DepartmentController : ODataController
    //{
    //    private DbModelContext db = new DbModelContext();

    //    // GET odata/Department
    //    [Queryable]
    //    public IQueryable<Department> GetDepartment()
    //    {
    //        return db.Departments;
    //    }

    //    // GET odata/Department(5)
    //    [Queryable]
    //    public SingleResult<Department> GetDepartment([FromODataUri] int key)
    //    {
    //        return SingleResult.Create(db.Departments.Where(department => department.Id == key));
    //    }

    //    // PUT odata/Department(5)
    //    public async Task<IHttpActionResult> Put([FromODataUri] int key, Department department)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        if (key != department.Id)
    //        {
    //            return BadRequest();
    //        }

    //        db.Entry(department).State = EntityState.Modified;

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!DepartmentExists(key))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return Updated(department);
    //    }

    //    // POST odata/Department
    //    public async Task<IHttpActionResult> Post(Department department)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        db.Departments.Add(department);
    //        await db.SaveChangesAsync();

    //        return Created(department);
    //    }

    //    // PATCH odata/Department(5)
    //    [AcceptVerbs("PATCH", "MERGE")]
    //    public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Department> patch)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        Department department = await db.Departments.FindAsync(key);
    //        if (department == null)
    //        {
    //            return NotFound();
    //        }

    //        patch.Patch(department);

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!DepartmentExists(key))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return Updated(department);
    //    }

    //    // DELETE odata/Department(5)
    //    public async Task<IHttpActionResult> Delete([FromODataUri] int key)
    //    {
    //        Department department = await db.Departments.FindAsync(key);
    //        if (department == null)
    //        {
    //            return NotFound();
    //        }

    //        db.Departments.Remove(department);
    //        await db.SaveChangesAsync();

    //        return StatusCode(HttpStatusCode.NoContent);
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    private bool DepartmentExists(int key)
    //    {
    //        return db.Departments.Count(e => e.Id == key) > 0;
    //    }
    //}
}
