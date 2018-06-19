using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesTest.App_Start
{
    /// <summary>
    /// 初始化依赖注入
    /// </summary>
    public class Bootstrapers
    {
        public static void Init()
        {
            var container = new UnityContainer();
            //不再用以依赖注入
            ICacheProvider CacheInstance = new RedisCacheProvider(GlobalSetting.CacheSeverConfiguration);
            container.RegisterInstance<ICacheProvider>(CacheInstance);
            //Load Config
            var fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = HttpContext.Current.Server.MapPath("~/XmlConfig/UnityCF.config")
            };
            Configuration configuration =
                ConfigurationManager.OpenMappedExeConfiguration(
                fileMap,
                ConfigurationUserLevel.None);
            var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");
            unitySection.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToApplication;
            unitySection.SectionInformation.UnprotectSection();
            unitySection.SectionInformation.ForceSave = true;
            container.LoadConfiguration(unitySection);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}