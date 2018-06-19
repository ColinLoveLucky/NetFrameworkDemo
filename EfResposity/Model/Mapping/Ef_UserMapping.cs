using System.Data.Entity.ModelConfiguration;

namespace EfResposity.Model.Mapping
{
	public class Ef_UserMapping : EntityTypeConfiguration<Ef_User>
	{
		public Ef_UserMapping()
		{
			this.Property(x => x.UserName).HasMaxLength(20);
			this.Property(x => x.Password).HasMaxLength(32);
		}
	}
}
