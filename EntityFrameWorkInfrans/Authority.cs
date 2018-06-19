using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkInfrans
{
    [Table("App_Action")]
    public class Action:BaseModel
    {

        [StringLength(20)]
        [Required]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description{get;set;}

        [ConcurrencyCheck]
        public int SocialSecurityNumber { get; set; }
    }

    [Table("App_Menu")]
    public class Menu:BaseModel
    {
        [StringLength(100)]
        public string MenuIcon { get; set; }

        [StringLength(100)]
        public string MenuUrl { get; set; }

        public int ParentId { get; set; }

        public virtual List<Action> Actions { get; set; }

        public virtual List<Role> Roles{get;set;}
     
    }

    [Table("App_Role")]
    public class Role:BaseModel
    {
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(20)]
        public string Name { get; set; }

        public virtual List<Menu> Menus { get; set; }

        public virtual List<UserGroup> UserGroups { get; set; }

    }


   
    
}
