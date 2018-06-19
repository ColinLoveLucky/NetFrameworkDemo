using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToEntity
{
    public class Lodging
    {
        [Key]
        public int LodgingId { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }
    }

    public class Resort:Lodging
    {
        public string Activities { get; set; }

        public string Entertainment { get; set; }
    }

    //public class LodgingMap : EntityTypeConfiguration<Lodging>
    //{
    //    public LodgingMap()
    //    {
    //        this.Map(m =>
    //        {
    //            m.ToTable("Lodging");
    //        }).Map<Resort>(m =>
    //        {
    //            m.ToTable("Resort");
    //            m.MapInheritedProperties();
    //        });
    //    }
    //}
}
