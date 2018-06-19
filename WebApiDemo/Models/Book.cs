using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Genre { get; set; }

        public DateTime PublishDate { get; set; }

        public string Description { get; set; }

        public virtual Author Author { get; set; }
    }
}