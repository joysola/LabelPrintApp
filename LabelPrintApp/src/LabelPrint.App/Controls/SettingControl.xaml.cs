using GalaSoft.MvvmLight.Messaging;
using LabelPrint.Domain;
using LabelPrint.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabelPrint.App.Controls
{
    /// <summary>
    /// SettingControl.xaml 的交互逻辑
    /// </summary>
    public partial class SettingControl : UserControl
    {
        public SettingControl()
        {
            InitializeComponent();
            this.DataContext = new SettingVM();
            this.RegisterMessenger();
        }

        private void RegisterMessenger()
        {
            
        }
    }
}
