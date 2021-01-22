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
namespace LabelPrint.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.RegisterCommand();
            var vm = new MainWindowVM();
            this.DataContext = vm;
            this.Loaded += (sender, e) =>
            {
                this.codetxt.Focus();
                vm.Count2 = 0; // 防止badge不更新
            };
        }

        private void RegisterCommand()
        {
            Messenger.Default.Register<bool>(this, MessengerKeyEnum.CloseWin, data =>
            {
                this.DrawerRight.IsOpen = false;
            });
        }

        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            this.DrawerRight.IsOpen = true;
            //SettingWin settingWin = new SettingWin();
            //settingWin.Owner = this;
            //settingWin.ShowDialog();
        }
    }
}
