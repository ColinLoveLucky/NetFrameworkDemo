using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AspNetWebForm
{
    public class UsersResposity
    {
        public IList<User> FindUser()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Id=1,
                    Name="Admin"
                },
                new Role()
                {
                    Id=2,
                    Name="Customer"
                }
            };
            return new List<User>(){
              new User(){
                  Id=1,
                  Name="xianggangzhang",
                  PassWord="Password@1",
                  Roles=roles
              },
              new User()
              {
                  Id=2,
                  Name="lisi",
                  PassWord="000000",
                  Roles=roles
              }
          };
        }

        public bool ValidaterUserIsValid(string userName, string password)
        {
            bool result = false;
            var users = FindUser();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                throw new Exception("Params is Valid");
            else if (users.Select(x => x.Name.ToLower() == userName.ToLower() && x.PassWord.ToLower() == password.ToLower()).FirstOrDefault())
                result = true;
            return result;
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }

        public List<Role> Roles { get; set; }
    }
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}