using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    //[RoutePrefix("api/books")]
    //public class BooksController : ApiController
    //{
    //    private DataBaseContext db = new DataBaseContext();

    //    private static readonly Expression<Func<Book, BookDto>> AsBookDto = x => new BookDto
    //    {
    //        Tilte = x.Title,
    //        Author = x.Author.Name,
    //        Genre = x.Genre
    //    };

    //    // GET api/Default1
    //    [Route("")]
    //    public IQueryable<BookDto> GetBooks()
    //    {
    //        return db.Books.Include(b => b.Author).Select(AsBookDto);
    //    }

    //    // GET api/Default1/5
    //    [Route("{id:int}")]
    //    [ResponseType(typeof(BookDto))]
    //    public async Task<IHttpActionResult> GetBook(int id)
    //    {
    //        BookDto book = await db.Books.Include(b => b.Author).
    //            Where(b => b.BookId == id).Select(AsBookDto).FirstOrDefaultAsync();
    //        if (book == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(book);
    //    }

    //    // PUT api/Default1/5
    //    public async Task<IHttpActionResult> PutBook(int id, Book book)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        if (id != book.BookId)
    //        {
    //            return BadRequest();
    //        }

    //        db.Entry(book).State = EntityState.Modified;

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!BookExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return StatusCode(HttpStatusCode.NoContent);
    //    }

    //    // POST api/Default1
    //    [ResponseType(typeof(Book))]
    //    public async Task<IHttpActionResult> PostBook(Book book)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        db.Books.Add(book);
    //        await db.SaveChangesAsync();

    //        return CreatedAtRoute("DefaultApi", new { id = book.BookId }, book);
    //    }

    //    // DELETE api/Default1/5
    //    [ResponseType(typeof(Book))]
    //    public async Task<IHttpActionResult> DeleteBook(int id)
    //    {
    //        Book book = await db.Books.FindAsync(id);
    //        if (book == null)
    //        {
    //            return NotFound();
    //        }

    //        db.Books.Remove(book);
    //        await db.SaveChangesAsync();

    //        return Ok(book);
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    private bool BookExists(int id)
    //    {
    //        return db.Books.Count(e => e.BookId == id) > 0;
    //    }
    //}
}