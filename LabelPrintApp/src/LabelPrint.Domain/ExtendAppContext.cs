using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPrint.Domain
{
    public class ExtendAppContext
    {
        public static ExtendAppContext Current { get; } = new ExtendAppContext();
        /// <summary>
        /// 配置信息
        /// </summary>
        public SettingModel AppSettingModel { get; set; }
    }
}
