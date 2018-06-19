namespace LinqToEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Posts
    {
        [Key]
        public int PostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public virtual Blogs Blogs { get; set; }

        public virtual Person CreatedBy { get; set; }
        public virtual Person UpdatedBy { get; set; }
    }
}
