using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    public class BookDto
    {
        public string Tilte { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }
    }

    public class BookDetailDto
    {
        public string Title { get; set; }

        public string Genre { get; set; }

        public DateTime PublishDate { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Author { get; set; }

    }
}