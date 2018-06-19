using System;
using System.Collections.Generic;

namespace EntityFrameWorkInfrans.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<int> BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
