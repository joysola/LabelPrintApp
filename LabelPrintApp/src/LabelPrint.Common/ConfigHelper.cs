using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace LabelPrint.Common
{
    public class ConfigHelper
    {
        /// <summary>
        /// 更新app.config的appSettings节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveAppsetting(string key,string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value); // 新增此节点
            config.Save(ConfigurationSaveMode.Modified); // 保存
            ConfigurationManager.RefreshSection("appSettings"); // 刷新配置
        }
    }
}
