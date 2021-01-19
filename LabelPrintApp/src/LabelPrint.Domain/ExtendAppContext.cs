using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPrint.Domain
{
    public class ExtendAppContext
    {
        public static ExtendAppContext Current { get; } = new ExtendAppContext();
        public SettingModel AppSettingModel { get; set; }
    }
}
