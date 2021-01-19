using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LabelPrint.Common
{
    public class TSCLibApi
    {
        /// <summary>
        /// dll名称
        /// </summary>
        private const string dllName = "TSCLIB.dll";
        /// <summary>
        /// 是加载指定的dll文件
        /// </summary>
        /// <param name="dllToLoad">dll文件的路径</param>
        /// <returns>返回的为该dll的实例(指针)</returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);
        /// <summary>
        /// 静态构造器用于动态加载dll
        /// </summary>
        static TSCLibApi()
        {
            var path = new Uri(typeof(TSCLibApi).Assembly.CodeBase).LocalPath; // 当前程序集路径
            var folder = Path.GetDirectoryName(path); // 程序集文件夹
            var subfolder = Environment.Is64BitProcess ? "64bit" : @"32bit"; // 选择64或32位dll文件夹
            LoadLibrary($@"{folder}\TSCDLL\{subfolder}\{dllName}");
        }
        /// <summary>
        /// 顯示 DLL 版本號碼
        /// </summary>
        [DllImport(dllName)]
        public static extern int about();
        /// <summary>
        /// 指定電腦端的輸出埠
        /// </summary>
        /// <param name="pirnterName">字串型別
        ///  (1) 單機列印時，請指定印表機驅動程式名稱 例如: TSC CLEVER TTP-243
        ///  (2) 若連接印表機伺服器，請指定伺服器路徑及共用印表機名稱 例如: \\SERVER\TTP243
        ///  (3) 直接指定平行傳輸介面，請指定輸出埠名稱為 LPT1 到 LPT4
        ///  (4) 直接指定 USB 傳輸介面，請指定輸出埠名稱為 USB
        ///  </param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int openport(string pirnterName);
        /// <summary>
        /// 關閉指定的電腦端輸出埠
        /// </summary>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int closeport();
        /// <summary>
        /// 送內建指令到條碼印表機
        /// </summary>
        /// <param name="printerCommand">詳細指令請參考 TSPL</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int sendcommand(string printerCommand);
        /// <summary>
        /// 設定標籤的寬度、高度、列印速度、列印濃度、感應器類別、 gap/black mark 垂直間距、 gap/black mark 偏移距離
        /// </summary>
        /// <param name="width">字串型別，設定標籤寬度，單位 mm</param>
        /// <param name="height">字串型別，設定標籤高度，單位 mm</param>
        /// <param name="speed">字串型別，設定列印速度， (列印速度隨機型不同而有不同的選項)
        ///  1.0: 每秒 1.0 吋列印速度
        ///  1.5: 每秒 1.5 吋列印速度
        ///  2.0: 每秒 2.0 吋列印速度
        ///  3.0: 每秒 3.0 吋列印速度
        ///  4.0: 每秒 4.0 吋列印速度
        ///  6.0: 每秒 6.0 吋列印速度
        ///  8.0: 每秒 8.0 吋列印速度
        ///  10.0: 每秒 10.0 吋列印速度
        /// </param>
        /// <param name="density">字串型別，設定列印濃度，0~15，數字愈大列印結果愈黑</param>
        /// <param name="sensor">字串型別，設定使用感應器類別
        ///  0 表示使用垂直間距感測器(gap sensor)
        ///  1 表示使用黑標感測器(black mark sensor)
        /// </param>
        /// <param name="vertical">字串型別，設定 gap/black mark 垂直間距高度，單位: mm</param>
        /// <param name="offset">字串型別，設定 gap/black mark 偏移距離，單位: mm，此參數若使用一般標籤時均設為 0</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int setup(string width, string height, string speed, string density, string sensor, string vertical, string offset);
        /// <summary>
        /// 下載單色 PCX 格式圖檔至印表機
        /// </summary>
        /// <param name="filename">字串型別，檔案名(可包含路徑)</param>
        /// <param name="image_name">字串型別，下載至印表機記憶體內之檔名(請使用大寫檔名)</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int downloadpcx(string filename, string image_name);
        /// <summary>
        /// 使用條碼機內建條碼列印
        /// </summary>
        /// <param name="x">字串型別，條碼 X 方向起始點，以點(point)表示。(200 DPI， 1 點=1/8 mm, 300 DPI， 1 點=1/12 mm)</param>
        /// <param name="y">字串型別，條碼 Y 方向起始點，以點(point)表示。(200 DPI， 1 點=1/8 mm, 300 DPI， 1 點=1/12 mm)</param>
        /// <param name="type">字串型別，
        ///  128 Code 128, switching code subset A, B, C automatically
        ///  128M Code 128, switching code subset A, B, C manually.
        ///  EAN128 Code 128, switching code subset A, B, C automatically
        ///  25 Interleaved 2 of 5
        ///  25C Interleaved 2 of 5 with check digits
        ///  39 Code 39
        ///  39C Code 39 with check digits
        ///  93 Code 93
        ///  EAN13 EAN 13
        ///  EAN13+2 EAN 13 with 2 digits add-on
        ///  EAN13+5 EAN 13 with 5 digits add-on
        ///  EAN8 EAN 8
        ///  EAN8+2 EAN 8 with 2 digits add-on
        ///  EAN8+5 EAN 8 with 5 digits add-on
        ///  CODA Codabar
        ///  POST PostnetUPCA UPC-A
        ///  UPCA+2 UPC-A with 2 digits add-on
        ///  UPCA+5 UPC-A with 5 digits add-on
        ///  UPCE UPC-E
        ///  UPCE+2 UPC-E with 2 digits add-on
        ///  UPCE+5 UPC-E with 5 digits add-on
        /// </param>
        /// <param name="height">字串型別，設定條碼高度，高度以點來表示 </param>
        /// <param name="readable">字串型別，設定是否列印條碼碼文
        ///  0: 不列印碼文
        ///  1: 列印碼文
        /// </param>
        /// <param name="rotation">字串型別，設定條碼旋轉角度
        ///  0: 旋轉 0 度
        ///  90: 旋轉 90 度
        ///  180: 旋轉 180 度
        ///  270: 旋轉 270 度
        /// </param>
        /// <param name="narrow">字串型別，設定條碼窄 bar 比例因子，請參考 TSPL 使用手冊</param>
        /// <param name="wide">字串型別，設定條碼窄 bar 比例因子，請參考 TSPL 使用手冊</param>
        /// <param name="code">字串型別，條碼內容</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int barcode(string x, string y, string type, string height, string readable, string rotation, string narrow, string wide, string code);
        /// <summary>
        /// 使用條碼機內建文字列印
        /// </summary>
        /// <param name="x">字串型別，文字 X 方向起始點，以點(point)表示。(200 DPI， 1 點=1/8 mm, 300 DPI， 1 點=1/12 mm)</param>
        /// <param name="y">字串型別，文字 Y 方向起始點，以點(point)表示。(200 DPI， 1 點=1/8 mm, 300 DPI， 1 點=1/12 mm)</param>
        /// <param name="fonttype">字串型別，內建字型名稱，共 12 種。
        ///  1: 8*/12 dots
        ///  2: 12*20 dots
        ///  3: 16*24 dots
        ///  4: 24*32 dots
        ///  5: 32*48 dots
        ///  TST24.BF2: 繁體中文 24*24
        ///  TST16.BF2: 繁體中文 16*16
        ///  TTT24.BF2: 繁體中文 24*24 (電信碼)
        ///  TSS24.BF2: 簡體中文 24*24
        ///  TSS16.BF2: 簡體中文 16*16K: 韓文 24*24
        ///  L: 韓文 16*16
        /// </param>
        /// <param name="rotation">字串型別，設定文字旋轉角度
        ///  0: 旋轉 0 度
        ///  90: 旋轉 90 度
        ///  180: 旋轉 180 度
        ///  270: 旋轉 270 度
        /// </param>
        /// <param name="xmul">字串型別，設定文字 X 方向放大倍率， 1~8</param>
        /// <param name="ymul">字串型別，設定文字 X 方向放大倍率， 1~8</param>
        /// <param name="text">字串型別，列印文字內容</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int printerfont(string x, string y, string fonttype, string rotation, string xmul, string ymul, string text);
        /// <summary>
        /// 清除
        /// </summary>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int clearbuffer();
        /// <summary>
        /// 列印標籤內容
        /// </summary>
        /// <param name="set">字串型別，設定列印標籤式數(set)</param>
        /// <param name="copy">字串型別，設定列印標籤份數(copy)</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int printlabel(string set, string copy);
        /// <summary>
        /// 跳頁，該函式需在 setup 後使用
        /// </summary>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int formfeed();
        /// <summary>
        /// 設定紙張不回吐
        /// </summary>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int nobackfeed();
        /// <summary>
        /// 使用 Windows TTF 字型列印文字
        /// </summary>
        /// <param name="x">整數型別，文字 X 方向起始點，以點(point)表示</param>
        /// <param name="y">整數型別，文字 Y 方向起始點，以點(point)表示</param>
        /// <param name="fontheight">整數型別，字體高度，以點(point)表示</param>
        /// <param name="rotation">整數型別，旋轉角度，逆時鐘方向旋轉
        ///  0 -> 0 degree
        ///  90-> 90 degree
        ///  180-> 180 degree
        ///  270-> 270 degree
        /// </param>
        /// <param name="fontstyle">整數型別，字體外形
        ///  0-> 標準(Normal)
        ///  1-> 斜體(Italic)
        ///  2-> 粗體(Bold)
        ///  3-> 粗斜體(Bold and Italic)
        /// </param>
        /// <param name="fontunderline">整數型別, 底線
        ///  0-> 無底線
        ///  1-> 加底線
        /// </param>
        /// <param name="szFaceName">字串型別，字體名稱。如: Arial, Times new Roman, 細名體, 標楷體</param>
        /// <param name="content">字串型別，列印文字內容。</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int windowsfont(int x, int y, int fontheight, int rotation, int fontstyle, int fontunderline, string szFaceName, string content);

    }
}
