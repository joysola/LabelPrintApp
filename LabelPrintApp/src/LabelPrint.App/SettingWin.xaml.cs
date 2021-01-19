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
using System.Windows.Shapes;

namespace LabelPrint.App
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWin : Window
    {
        public SettingWin()
        {
            InitializeComponent();
            this.DataContext = new SettingVM();
            this.RegisterMessenger();
        }

        private void RegisterMessenger()
        {
            Messenger.Default.Register<bool>(this,MessengerKeyEnum.CloseWin, data =>
            {
                this.DialogResult = data;
                this.Close();
            });
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
