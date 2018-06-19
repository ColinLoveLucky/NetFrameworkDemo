using System;
using System.ComponentModel.DataAnnotations;

namespace LinqToEntity
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public decimal ? Price { get; set; }

        public DateTime ? productDate { get; set; }
    }
}