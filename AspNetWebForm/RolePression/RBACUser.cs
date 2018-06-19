using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AspNetWebForm.RolePression
{
    public class RBACUser
    {
        public string UserName { get; set; }

        public ICollection<RBACRole> Roles { get; set; }
    }
    public class RBACRole
    {
        public string RoleName { get; set; }

        public ICollection<RBACPermission> Permissions { get; set; }
    }
    public class RBACPermission
    {
        public string PerssionName { get; set; }
    }
    public static class RBACContext
    {
        private static RBACUser _User;

        private static Func<string, RBACUser> _SetRBACUser;
        public static void SetRBACUser(Func<string, RBACUser> setRBACUser)
        {
            _SetRBACUser = setRBACUser;
        }
        public static RBACUser GetRBACUser(string userName)
        {
            return _User ?? (_User = _SetRBACUser(userName));
        }
        public static void Clear()
        {
            _SetRBACUser = null;
        }
    }
    public class DelegateRoleProvider : RoleProvider
    {
        private static Func<string, string[]> _GetRolesForUser;
        private static Func<string, string, bool> _IsUserInRole;
        public static void SetGetRolesForUser(Func<string, string[]> getRolesForUser)
        {
            _GetRolesForUser = getRolesForUser;
        }
        public static void SetIsUserInRole(Func<string, string, bool> isUserInRole)
        {
            _IsUserInRole = isUserInRole;
        }
        public override string[] GetRolesForUser(string username)
        {
            return _GetRolesForUser(username);
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            return _IsUserInRole(username, roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
    public class ApplicationInitClass
    {
        //Configuration
        //ConfigurationManager
        //ConfigurationSection
        //ProviderCollection
        //Roles
        //Roles
        public static void Init()
        {
            RBACContext.SetRBACUser(u =>
            {
                return new RBACUser
                {
                    UserName = u,
                    Roles = new List<RBACRole>
                   {
                       new RBACRole
                       {
                           RoleName="admin",
                           Permissions=new List<RBACPermission>
                           {
                               new RBACPermission
                               {
                                   PerssionName="admin"
                               }
                           }
                       }
                   }
                };
            });
        }
    }
}