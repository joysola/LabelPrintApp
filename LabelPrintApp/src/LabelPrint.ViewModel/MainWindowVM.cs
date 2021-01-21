using DST.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LabelPrint.ApiClient.Service;
using LabelPrint.Common;
using LabelPrint.Domain;
using System;
using System.Collections.Generic;
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
        private string _barcode;
        private string _sampleTSCtxt;
        private int? _printModel = 1;
        private static readonly object _locker = new object();
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
                }
                RaisePropertyChanged("Samplecode");
            }
        }
        /// <summary>
        /// 实验室编号（条码号）
        /// </summary>
        public string Barcode
        {
            get { return _barcode; }
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

        public int? PrintModel
        {
            get => _printModel;
            set { _printModel = value; RaisePropertyChanged("PrintModel"); }
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
                if (!string.IsNullOrEmpty(Barcode))
                {
                    try
                    {
                        Monitor.Enter(_locker); // 加锁，防止端口被占用
                        TSCLibApi.OpenPort("TSC TTP-244 Pro"); // 打开端口
                        TSCLibApi.Setup("51", "17.2", "5", "15", "0", "2", "0");
                        TSCLibApi.SendCommand("SET TEAR ON"); // The label gap will stop at the tear off position after print.
                        TSCLibApi.ClearBuffer(); // 清除缓存
                        var setting = ExtendAppContext.Current.AppSettingModel;
                        string barcodeCommandStr = $"{setting.Code} {setting.X},{setting.Y},\"{setting.CodeType}\",{setting.Height},{setting.HumanReadable},{setting.Rotation},{setting.Narrow},{setting.Width},{setting.Alignment},\"{Barcode}\"";
                        TSCLibApi.SendCommand(barcodeCommandStr);
                        //string test = "BARCODE 400,50, \"TELEPENN\",60,2,0,2,6,2, \"20210112108\"";
                        //TSCLibApi.SendCommand(test);

                        // TSCLibApi.WindowsFont(20, 0, 15, 0, 0, 0, "SimSun", Barcode);
                        TSCLibApi.PrintLabel("1", "1");
                    }
                    finally
                    {
                        TSCLibApi.ClosePort(); // 关闭端口
                        Monitor.Exit(_locker);
                    }
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
                WhirlingControlManager.ShowWaitingForm();
                var sample = SampleCodeService.Instance.GetSamplebyCode(samplecode);
                if (!string.IsNullOrEmpty(sample.laboratoryCode))
                {
                    this.ShowSampeInfo(sample); // 显示样本信息
                    Barcode = sample.laboratoryCode; // 获取实验室编号
                }
                WhirlingControlManager.CloseWaitingForm();
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
                var tbx = sender as TextBox;
                tbx.SelectAll(); // 文本全选
                Barcode = null; // 文本变化，先清空显示的Barcode
                SampleTSCtxt = null; // 文本框变化，清空样本信息
                if (!string.IsNullOrEmpty(tbx.Text))
                {
                    await this.SearchSampleTSC(tbx.Text); // 搜索Sample实例
                    if (PrintModel == 3)
                    {
                        // 停顿
                    }
                    else
                    {
                        this.PrintCommand.Execute(null); // 打印条码
                    }
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
        private void ShowSampeInfo(SampleTSC sampleTSC)
        {
            Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                this.SampleTSCtxt = $"患者姓名: {sampleTSC.patientName};   患者年龄: {sampleTSC.age};\r\n医院: {sampleTSC.hospitalName};\r\n地址: {sampleTSC.provinceName}/{sampleTSC.cityName}/{sampleTSC.areaName};";
            });
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
