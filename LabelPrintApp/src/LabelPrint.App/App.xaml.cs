using DST.Common.Helper.Factory;
using DST.Controls;
using DST.Controls.Base;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Exceptions;
using LabelPrint.Common;
using LabelPrint.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LabelPrint.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }

        private void InitApiClient()
        {
            var url = ConfigurationManager.AppSettings["SampleCodeApi"];
            if (string.IsNullOrEmpty(url))
            {
                ConfirmMessageBox.Show("请配置Api地址！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            HttpClientEx.InitApiClient(url);
            Action<dynamic> action = data =>
            {
                if (!data.success)
                {
                    throw new HttpClientException($"WebApi访问失败！原因：{data.msg}");
                }
            };
            HttpClientEx.SetPrePorcess(typeof(ApiResponse<object>), action);
        }

        /// <summary>
        /// 重写Startup事件响应
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 添加全局异常捕获
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DispatcherHelper.Initialize();
            this.Register();
            this.InitApiClient();
            this.InitExtendAppContext();
        }
        /// <summary>
        /// 初始化全局属性
        /// </summary>
        private void InitExtendAppContext()
        {
            var settingStr = ConfigurationManager.AppSettings["BarCodeSetting"];
            if (!string.IsNullOrEmpty(settingStr))
            {
                ExtendAppContext.Current.AppSettingModel = AppsettingSerializer.Deserialize<SettingModel>(settingStr);
            }
            else
            {
                ExtendAppContext.Current.AppSettingModel = new SettingModel();
            }
        }

        private void Register()
        {
            // 注册消息，弹出的对话框需要要主线程中弹出
            Messenger.Default.Register<ShowMessageBoxMessage>(this, message =>
            {
                if (message.IsAsyncShow)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var result = ConfirmMessageBox.Show(message.Text, message.SubMessage, message.Button, message.Icon, message.IsAutoClose, message.AutoCloseTime);
                        if (message.CallBack != null)
                        {
                            message.CallBack(result);
                        }
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        var result = ConfirmMessageBox.Show(message.Text, message.SubMessage, message.Button, message.Icon, message.IsAutoClose, message.AutoCloseTime);
                        if (message.CallBack != null)
                        {
                            message.CallBack(result);
                        }
                    }));
                }
            });

            ///注册各个页面类型
            Messenger.Default.Register<ShowContentWindowMessage>(this, message =>
            {
                Type type = ShowContentWindowMessageFactory.CreateContent(message.ContentName); // 由ContentWindow工厂生产对应类型
                if (type != null)
                {
                    message.Content = type;
                    var action = new ShowContentWindowAction(message);
                    action.CallInvoke();
                }
            });
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    Logger.Error("非UI线程全局异常", exception);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("不可恢复的非UI线程全局异常", ex);
                MessageBox.Show("应用程序发生不可恢复的异常，即将退出！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Logger.Error("UI线程全局异常", e.Exception);
                // 针对Api访问的请求处理
                if (e.Exception.InnerException is HttpClientException || e.Exception is HttpClientException)
                {
                    var exception = e.Exception.InnerException ?? e.Exception;
                    MessageBox.Show(exception.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Logger.Error("不可恢复的UI线程全局异常", ex);
                MessageBox.Show("应用程序发生不可恢复的异常，即将退出！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
