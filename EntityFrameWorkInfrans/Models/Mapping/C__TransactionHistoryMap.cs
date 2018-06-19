using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EntityFrameWorkInfrans.Models.Mapping
{
    public class C__TransactionHistoryMap : EntityTypeConfiguration<C__TransactionHistory>
    {
        public C__TransactionHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("__TransactionHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreationTime).HasColumnName("CreationTime");
        }
    }
}
