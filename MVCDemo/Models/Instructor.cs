using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCDemo.Models
{
    public partial class Instructor
    {
        [Key]
        public int ID { get; set; }

        public string LastName { get; set; }

        public string FirstMidName { get; set; }

        public DateTime? HireDate { get; set; }

        [NotMapped]
        public int CourseId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }

    [MetadataType(typeof(InstructorMetadata))]
    public partial class Instructor
    {

    }


    public class InstructorMetadata
    {
        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string FirstMidName { get; set; }
    }
}