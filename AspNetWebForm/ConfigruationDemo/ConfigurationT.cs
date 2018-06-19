using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace AspNetWebForm.ConfigruationDemo
{
    public class ConfigurationT
    {
        
        public void Test()
        {
            //ConfigurationElement
            // ConfigurationSection
            //ConfigurationProperty
            //WebConfigurationManager
            //ConfigurationManager
            // ConfigurationSection
            // ConfigurationSectionCollection 
            // ConfigurationSectionGroup 
            //ConfigurationSectionGroupCollection
            //ConfigurationProperty 
            // ConfigurationPropertyAttribute 
            //ConfigurationElement 
            //ConfigurationElementCollection 
            //Configuration
            //Configuration
            // Configuration
            //Configuration
            // ConfigurationManager

            
        }
    }
    public class CustomerConfiguration : ConfigurationSection
    {

        
        //private static CustomerConfiguration setting;
        //public static CustomerConfiguration Setting
        //{
        //    get
        //    {
        //        return setting ?? (setting = (CustomerConfiguration)ConfigurationManager.GetSection("firstCustomConfiguration"));
        //    }
        //}
        [ConfigurationProperty("Id", DefaultValue = "1", IsRequired = true)]
        public long Id
        {
            get { return (long)this["Id"]; }
            set { this["Id"] = value; }
        }
        [ConfigurationProperty("Name", DefaultValue = "Hello", IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("FirstProperty", DefaultValue = "Hello", IsRequired = true)]
        public string FirstProperty
        {
            get { return (string)this["FirstProperty"]; }
            set { this["FirstProperty"] = value; }
        }
    }
    public class UrlConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("Id", DefaultValue = "1", IsRequired = true)]
        public long Id
        {
            get { return (long)this["Id"]; }
            set { this["Id"] = value; }
        }
        [ConfigurationProperty("Name", DefaultValue = "Hello", IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("FirstProperty", DefaultValue = "Hello", IsRequired = true)]
        public string FirstProperty
        {
            get { return (string)this["FirstProperty"]; }
            set { this["FirstProperty"] = value; }
        }

        [ConfigurationProperty("url", DefaultValue = "Hello", IsRequired = true)]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }
    }
    public class CustomConfigurationSecond : ConfigurationSection
    {
        //private static CustomConfigurationSecond setting;
        //public static CustomConfigurationSecond Setting
        //{
        //    get
        //    {
        //        return setting ?? (setting = (CustomConfigurationSecond)ConfigurationManager.GetSection("CustomConfigurationSecond"));
        //    }
        //}
        [ConfigurationProperty("url")]
        public UrlConfigurationElement UrlElement
        {
            get { return (UrlConfigurationElement)this["url"]; }
            set { this["url"] = value; }
        }
    }
    public class UrlConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlConfigurationElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UrlConfigurationElement)element).Name;
        }
    }
    public class CustomConfigurationThird : ConfigurationSection
    {
        //private static CustomConfigurationThird setting;
        //public static CustomConfigurationThird Setting
        //{
        //    get
        //    {
        //        return setting ?? (setting = (CustomConfigurationThird)ConfigurationManager.GetSection("CustomConfigurationThird"));
        //    }
        //}
        [ConfigurationProperty("urls")]
        [ConfigurationCollection(typeof(UrlConfigurationElementCollection), AddItemName = "addUrl",
            ClearItemsName = "clearUrls", RemoveItemName = "RemoveUrl")]
        public UrlConfigurationElementCollection UrlElements
        {
            get { return (UrlConfigurationElementCollection)this["urls"]; }
            set { this["urls"] = value; }
        }
    }
    public class CustomerSectionGroup : ConfigurationSectionGroup
    {
        public CustomerConfiguration FirstSection
        {
            get
            {
                return (CustomerConfiguration)base.Sections["firstCustomConfiguration"];
            }
        }
        public CustomConfigurationSecond SecondSection
        {
            get
            {
                return (CustomConfigurationSecond)base.Sections["CustomConfigurationSecond"];
            }
        }
    }

    public class DelegeteRoleProvider : RoleProvider
    {

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

        public override string[] GetRolesForUser(string username)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
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
}