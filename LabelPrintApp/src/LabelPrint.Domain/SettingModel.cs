using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPrint.Domain
{
    /// <summary>
    /// 条码设置实体
    /// </summary>
    [Serializable]
    public class SettingModel
    {
        /// <summary>
        /// 打印码号（BARCODE 条形码、QRCODE 二维码）
        /// </summary>
        public string Code { get; set; } = "BARCODE";
        /// <summary>
        /// 起始x坐标
        /// </summary>
        public string X { get; set; } = "115";
        /// <summary>
        /// 起始x坐标
        /// </summary>
        public string X_Other { get; set; } = "315";
        /// <summary>
        /// 起始y坐标
        /// </summary>
        public string Y { get; set; } = "17";
        /// <summary>
        /// 字串型別
        /// </summary>
        public string CodeType { get; set; } = "128";
        /// <summary>
        /// 条码高度
        /// </summary>
        public string Height { get; set; } = "55";
        /// <summary>
        /// 条码文字对齐方式（0 无、1 左对齐、2 居中、3 右对齐）
        /// </summary>
        public string HumanReadable { get; set; } = "2";
        /// <summary>
        /// 旋转角度（顺时针）
        /// </summary>
        public string Rotation { get; set; } = "0";
        /// <summary>
        /// 
        /// </summary>
        public string Narrow { get; set; } = "1";
        /// <summary>
        /// 条码宽度
        /// </summary>
        public string Width { get; set; } = "1";
        /// <summary>
        /// 条码的对齐方式（0 默认左对齐、1 左对齐、2 居中、3 右对齐）
        /// </summary>
        public string Alignment { get; set; } = "2";
        ///// <summary>
        ///// 条码内容
        ///// </summary>
        //public string Content { get; set; }
    }
}
