using System;
using System.Collections.Generic;

namespace EntityFrameWorkInfrans.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<System.DateTime> productDate { get; set; }
    }
}
