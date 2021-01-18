using LabelPrint.ApiClient.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPrint.ViewModel
{
    public class MainWindowVM
    {
        public MainWindowVM()
        {
            var xx = SampleCodeService.Instance.GetSamplebyCode("320830C19210111001");
        }
    }
}
