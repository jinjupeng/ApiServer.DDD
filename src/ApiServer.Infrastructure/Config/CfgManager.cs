using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServer.Infrastructure.Config
{
    /// <summary>
    /// 配置文件管理器
    /// </summary>
    public static class CfgManager
    {

        /// <summary>
        ///
        /// </summary>
        public static IConfiguration Configuration { get; }

        static CfgManager()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();
        }
    }
}
