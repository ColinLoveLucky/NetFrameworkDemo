

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LinqToEntity
{
    class Program
    {

   
        static void Main(string[] args)
        {
            //TestEntity entity = new TestEntity();
            //entity.GetMultiFromStore();
            // Test test = new Test();
            // test.GetMetaEdmItem();
            using (var db = new LinqEntityCodeFirstDbContext())
            {
                var metadata = ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace;

                var storeItemCollection = metadata.GetItemCollection(DataSpace.CSpace);
            }

        }

        [ContractInvariantMethod]
        private static void ObjectInvariant()
        {
            Contract.Invariant(true, "hello");
        }
    }
}
