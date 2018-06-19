using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkInfrans
{
    public class ViewContext : DbContext
    {
        public ViewContext()
            : base("DbContext")
        {
            
        }

        public DbSet<Dictionary> Dictionarys { get; set; }

        public DbSet<DictionaryChild> DictionaryChilds { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Company> Companys { get; set; }

        public DbSet<Dept> Depts { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<Action> Actions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
