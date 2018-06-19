using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkInfrans
{
    [Table("App_User")]
    public class User:BaseModel
    {
      
        [Required]
        [StringLength(16)]
        public string UserName { get; set; }

        [Required]
        [StringLength(12)]
        public string PassWord { get; set; }


        public virtual List<Company> Companys { get; set; }

        public virtual List<Dept> Depts { get; set; }
    }

    [Table("App_Comoany")]
    public class Company:BaseModel
    {

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }

        [Required]
        public DateTime JobInTime { get; set; }

    }

    [Table("App_Dept")]
    public class Dept:BaseModel
    {

        [StringLength(20)]
        [Required]
        public string Code { get; set; }

        [StringLength(20)]
        [Required]
        public string Name { get; set; }

        public virtual List<User> Users { get; set; }
    }

    [Table("App_User_Group")]
    public class UserGroup:BaseModel
    {
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        public virtual List<User> Users { get; set; }

        public virtual List<Role> Roles { get; set; }
    }

   
}
