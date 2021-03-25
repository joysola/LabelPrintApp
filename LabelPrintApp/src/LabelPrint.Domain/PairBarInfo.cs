using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPrint.Domain
{
    public class PairBarInfo
    {
        /// <summary>
        /// 条码号
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 患者信息
        /// </summary>
        public string SampleTSCtxt { get; set; }
    }
}
