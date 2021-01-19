using GalaSoft.MvvmLight.Command;
using LabelPrint.ApiClient.Service;
using LabelPrint.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LabelPrint.ViewModel
{
    public class MainWindowVM
    {
        /// <summary>
        /// 打印条码
        /// </summary>
        public ICommand PrintCommand { get; set; } = new RelayCommand(() =>
        {

        });
        public MainWindowVM()
        {
            var xx = SampleCodeService.Instance.GetSamplebyCode("320830C19210111001");
            var xx1 = TSCLibApi.about();
            string code = $"BARCODE ";
        }
    }
}
