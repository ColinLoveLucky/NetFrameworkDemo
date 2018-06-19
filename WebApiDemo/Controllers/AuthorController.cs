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
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    /*
    若要为此控制器添加路由，请将这些语句合并到 WebApiConfig 类的 Register 方法中。请注意 OData URL 区分大小写。

    using System.Web.Http.OData.Builder;
    using WebApiDemo.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Author>("Author");
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    //public class AuthorController : ODataController
    //{
    //    private DataBaseContext db = new DataBaseContext();

    //    // GET odata/Author
    //    [Queryable]
    //    public IQueryable<Author> GetAuthor()
    //    {
    //        return db.Authors;
    //    }
    //    //Microsoft.Data.Edm
    //    //Microsoft.Data.OData
    //    // GET odata/Author(5)
    //    [Queryable]
    //    public SingleResult<Author> GetAuthor([FromODataUri] int key)
    //    {
    //        return SingleResult.Create(db.Authors.Where(author => author.AuthorId == key));
    //    }

    //    // PUT odata/Author(5)
    //    public async Task<IHttpActionResult> Put([FromODataUri] int key, Author author)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        if (key != author.AuthorId)
    //        {
    //            return BadRequest();
    //        }

    //        db.Entry(author).State = EntityState.Modified;

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!AuthorExists(key))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return Updated(author);
    //    }

    //    // POST odata/Author
    //    public async Task<IHttpActionResult> Post(Author author)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        db.Authors.Add(author);
    //        await db.SaveChangesAsync();

    //        return Created(author);
    //    }

    //    // PATCH odata/Author(5)
    //    [AcceptVerbs("PATCH", "MERGE")]
    //    public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Author> patch)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        Author author = await db.Authors.FindAsync(key);
    //        if (author == null)
    //        {
    //            return NotFound();
    //        }

    //        patch.Patch(author);

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!AuthorExists(key))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return Updated(author);
    //    }

    //    // DELETE odata/Author(5)
    //    public async Task<IHttpActionResult> Delete([FromODataUri] int key)
    //    {
    //        Author author = await db.Authors.FindAsync(key);
    //        if (author == null)
    //        {
    //            return NotFound();
    //        }

    //        db.Authors.Remove(author);
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

    //    private bool AuthorExists(int key)
    //    {
    //        return db.Authors.Count(e => e.AuthorId == key) > 0;
    //    }
    //}
}
