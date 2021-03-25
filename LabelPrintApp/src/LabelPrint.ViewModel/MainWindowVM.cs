using DST.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LabelPrint.ApiClient.Service;
using LabelPrint.Common;
using LabelPrint.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace LabelPrint.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        private string _samplecode;
        private string _tempBarcode;
        private string _tempSampleTSCtxt;

        private string _barcode;
        private string _sampleTSCtxt;
        private string _barcode2;
        private string _sampleTSCtxt2;

        private int? _printModel = 1;
        private int _count = 1;
        private int pairCount = 0;
        private static readonly SemaphoreSlim _locker2 = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 样本编号
        /// </summary>
        public string Samplecode
        {
            get => _samplecode;
            set
            {
                _samplecode = value;
                if (string.IsNullOrEmpty(_samplecode))
                {
                    Barcode = null;
                    SampleTSCtxt = null;
                    Barcode2 = null;
                    SampleTSCtxt2 = null;
                    _tempBarcode = null;
                    _tempSampleTSCtxt = null;
                }
                RaisePropertyChanged("Samplecode");
            }
        }
        /// <summary>
        /// 实验室编号（条码号）2
        /// </summary>
        public string Barcode2
        {
            get => _barcode2;
            set { _barcode2 = value; RaisePropertyChanged("Barcode2"); }
        }
        /// <summary>
        /// 患者信息2
        /// </summary>
        public string SampleTSCtxt2
        {
            get => _sampleTSCtxt2;
            set { _sampleTSCtxt2 = value; RaisePropertyChanged("SampleTSCtxt2"); }
        }
        /// <summary>
        /// 实验室编号（条码号）
        /// </summary>
        public string Barcode
        {
            get => _barcode;
            set { _barcode = value; RaisePropertyChanged("Barcode"); }
        }
        /// <summary>
        /// 患者信息
        /// </summary>
        public string SampleTSCtxt
        {
            get => _sampleTSCtxt;
            set { _sampleTSCtxt = value; RaisePropertyChanged("SampleTSCtxt"); }
        }
        /// <summary>
        /// 打印模式（1 单打，2 连打，3 扫描）
        /// </summary>
        public int? PrintModel
        {
            get => _printModel;
            set
            {
                _printModel = value;
                // 切模式的时候清空历史数据
                Barcode = null;
                Barcode2 = null;
                SampleTSCtxt = null;
                SampleTSCtxt2 = null;
                _tempBarcode = null;
                _tempSampleTSCtxt = null;
                RaisePropertyChanged("PrintModel");
            }
        }
        /// <summary>
        /// 成功打印的次数
        /// </summary>
        public int Count2
        {
            get => _count;
            set { _count = value; RaisePropertyChanged("Count2"); }
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        public ICommand PrintCommand { get; set; }
        /// <summary>
        /// 根据样本编码，显示实验室编码
        /// </summary>
        public ICommand CodeChangedCommand { get; set; }
        /// <summary>
        /// TSC的dll版本
        /// </summary>
        public ICommand TSCVerCommand { get; set; }

        public MainWindowVM()
        {
            //var xx = SampleCodeService.Instance.GetSamplebyCode("320830C19210111001");
            this.RegisterCommand();
        }
        /// <summary>
        /// 注册命令
        /// </summary>
        private void RegisterCommand()
        {
            // 打印条码
            this.PrintCommand = new RelayCommand(() =>
            {

                try
                {

                    Logger.Info($"条码开始打印!");
                    TSCLibApi.OpenPort("TSC TTP-244 Pro"); // 打开端口
                    TSCLibApi.Setup("51", "17.2", "5", "15", "0", "2", "0");
                    //TSCLibApi.Setup("101", "17.2", "5", "15", "0", "2", "0");
                    TSCLibApi.SendCommand("SET TEAR ON"); // The label gap will stop at the tear off position after print.
                    TSCLibApi.ClearBuffer(); // 清除缓存
                    var setting = ExtendAppContext.Current.AppSettingModel;
                    // 第一个
                    if (!string.IsNullOrEmpty(Barcode))
                    {
                        Logger.Info($"条码{Barcode}开始打印！");
                        string barcodeCommandStr = $"{setting.Code} {setting.X},{setting.Y},\"{setting.CodeType}\",{setting.Height},{setting.HumanReadable},{setting.Rotation},{setting.Narrow},{setting.Width},{setting.Alignment},\"{Barcode}\"";
                        TSCLibApi.SendCommand(barcodeCommandStr);
                        Count2++;
                    }
                    // 第二个
                    if (!string.IsNullOrEmpty(Barcode2))
                    {
                        Logger.Info($"条码{Barcode2}开始打印！");
                        string barcodeCommandStr2 = $"{setting.Code} {setting.X_Other},{setting.Y},\"{setting.CodeType}\",{setting.Height},{setting.HumanReadable},{setting.Rotation},{setting.Narrow},{setting.Width},{setting.Alignment},\"{Barcode2}\"";
                        TSCLibApi.SendCommand(barcodeCommandStr2);
                        Count2++;
                    }

                    // TSCLibApi.WindowsFont(20, 0, 15, 0, 0, 0, "SimSun", Barcode);
                    TSCLibApi.PrintLabel("1", "1");
                    //Count2++;
                }
                finally
                {
                    TSCLibApi.ClosePort(); // 关闭端口
                    Logger.Info($"条码{Barcode},{Barcode2}打印完成!");
                }

            });
            // TSC的dll版本
            this.TSCVerCommand = new RelayCommand(() =>
            {
                TSCLibApi.About();
            });
            // 根据样本编码，显示实验室编码
            this.CodeChangedCommand = new RelayCommand<string>(code =>
            {
                //Barcode = null; // 文本变化，先清空显示的Barcode
                //if (!string.IsNullOrEmpty(code))
                //{
                //    this.SearchSampleTSC(code); // 搜索Sample实例
                //}
            });
        }
        /// <summary>
        /// 搜索sample实例
        /// </summary>
        /// <param name="samplecode"></param>
        private DispatcherOperation SearchSampleTSC(string samplecode)
        {
            return Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var sample = SampleCodeService.Instance.GetSamplebyCode(samplecode);
                    if (!string.IsNullOrEmpty(sample.laboratoryCode))
                    {
                        _tempSampleTSCtxt = this.ShowSampeInfo(sample); // 显示样本信息
                        _tempBarcode = sample.laboratoryCode; // 获取实验室编号

                        Task.Run(() =>
                        {
                            Logger.Info($"样本信息:{JsonConvert.SerializeObject(sample)}");
                        });
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            });
        }
        /// <summary>
        /// 样本编码回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SampleCode_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    //Monitor.Enter(_locker); // 加锁，防止端口被占用
                    await _locker2.WaitAsync(); // 加锁
                    var tbx = sender as TextBox;
                    tbx.SelectAll(); // 文本全选

                    if (!string.IsNullOrEmpty(tbx.Text))
                    {
                        _tempBarcode = null;
                        _tempSampleTSCtxt = null;
                        await this.SearchSampleTSC(tbx.Text); // 搜索Sample实例

                        if (PrintModel == 2) // 双打
                        {
                            pairCount++;
                            if (pairCount == 1)// 第一次
                            {
                                // 先清空第二个
                                Barcode2 = null;
                                SampleTSCtxt2 = null;

                                Barcode = _tempBarcode;
                                SampleTSCtxt = _tempSampleTSCtxt;
                            }
                            else if (pairCount == 2)
                            {
                                Barcode2 = _tempBarcode;
                                SampleTSCtxt2 = _tempSampleTSCtxt;
                                this.PrintCommand.Execute(null); // 打印条码
                                pairCount = 0;// 打印完成
                            }
                        }
                        else // 单打和扫描
                        {
                            Barcode = _tempBarcode;
                            SampleTSCtxt = _tempSampleTSCtxt;
                            if (PrintModel == 1)
                            {
                                this.PrintCommand.Execute(null); // 打印条码
                            }
                        }
                    }
                }
                finally
                {
                    //Monitor.Exit(_locker);
                    _locker2.Release();
                }
            }
        }
        /// <summary>
        /// 样本编码获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SampleCode_GotFocus(object sender, RoutedEventArgs e)
        {
            var tbx = sender as TextBox;
            if (!string.IsNullOrEmpty(tbx.Text))
            {
                // 必须使用此触发方式，内部事件先执行，后执行此方法
                Dispatcher.CurrentDispatcher.InvokeAsync(() => tbx.SelectAll());
            }
        }
        /// <summary>
        /// 显示样本信息
        /// </summary>
        /// <param name="sampleTSC"></param>
        private string ShowSampeInfo(SampleTSC sampleTSC)
        {
            return $"患者姓名: {sampleTSC.patientName};   患者年龄: {sampleTSC.age};\r\n医院: {sampleTSC.hospitalName};\r\n地址: {sampleTSC.provinceName}/{sampleTSC.cityName}/{sampleTSC.areaName};";
        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        private void TestQ()
        {
            TSCLibApi.OpenPort("TSC TTP-244 Pro"); // 打开端口
            TSCLibApi.Setup("51", "17.2", "5", "15", "0", "2", "0");
            TSCLibApi.SendCommand("SET TEAR ON"); // The label gap will stop at the tear off position after print.
            TSCLibApi.ClearBuffer(); // 清除缓存
            string qstr = "QRCODE 150,20,L,2,A,0,M2,S3,\"320830C19210111001\"";
            TSCLibApi.SendCommand(qstr);
            TSCLibApi.PrintLabel("1", "1");
            TSCLibApi.ClosePort(); // 关闭端口
        }

    }
}
