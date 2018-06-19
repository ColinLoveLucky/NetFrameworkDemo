using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EntityFrameWorkInfrans.Models.Mapping
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            // Primary Key
            this.HasKey(t => t.PostId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Posts");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.BlogId).HasColumnName("BlogId");

            // Relationships
            this.HasOptional(t => t.Blog)
                .WithMany(t => t.Posts)
                .HasForeignKey(d => d.BlogId);

        }
    }
}
