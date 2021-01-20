using DST.Controls.Base;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LabelPrint.Common;
using LabelPrint.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Expression = System.Linq.Expressions.Expression;

namespace LabelPrint.ViewModel
{
    public class SettingVM : ViewModelBase
    {
        private static readonly List<PropertyInfo> _settingModelProps = typeof(SettingModel).GetProperties().ToList();
        private SettingModel _settingModel = ExtendAppContext.Current.AppSettingModel;
        private static readonly string appsettingStr = "BarCodeSetting";
        private Lazy<Func<string, SettingModel, dynamic>> _getPropFunc = new Lazy<Func<string, SettingModel, dynamic>>(() =>
        {
             var param_propName = Expression.Parameter(typeof(string), "propName");
             var param_model = Expression.Parameter(typeof(SettingModel), "model");
             var parm_prop = Expression.Parameter(typeof(PropertyInfo), "prop");
             var cons_props = Expression.Constant(_settingModelProps, typeof(List<PropertyInfo>));
             var methodInfo = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(x => x.GetParameters().Length == 2);
             var binary_equal = Expression.Equal(param_propName, parm_prop);
             Func<PropertyInfo, bool> func = Expression.Lambda<Func<PropertyInfo, bool>>(binary_equal).Compile();
             var xx = Expression.Call(cons_props, methodInfo, Expression.Lambda<Func<PropertyInfo, bool>>(binary_equal));
             //var cases = new List<SwitchCase>();

             //foreach (var prop in _settingModelProps)
             //{
             //    var property = Expression.Property(param_model, prop.Name);
             //    var propertyHash = Expression.Constant(propertyInfo.Name.GetHashCode(), typeof(int));

             //    cases.Add(Expression.SwitchCase(Expression.Convert(property, typeof(object)), propertyHash));

             //}
             //Expression.Property(param_model, param_propName)
             return null;
         });
        public SettingModel SettingModel
        {
            get => _settingModel;
            set { _settingModel = value; RaisePropertyChanged("SettingModel"); }
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public ICommand SaveCommand { get; set; }
        /// <summary>
        /// 重置
        /// </summary>
        public ICommand ResetCommand { get; set; }

        public SettingVM()
        {
            //var xx = _getPropFunc.Value;
            this.RegisterCommand();
        }

        private void RegisterCommand()
        {
            // 保存
            this.SaveCommand = new RelayCommand(() =>
            {
                ExtendAppContext.Current.AppSettingModel = SettingModel;
                var settingStr = AppsettingSerializer.Serialize(SettingModel); // 配置字符串
                ConfigHelper.SaveAppsetting(appsettingStr, settingStr);

                Messenger.Default.Send(true, MessengerKeyEnum.CloseWin); // 关闭窗口
            });
            // 重置
            this.ResetCommand = new RelayCommand(() =>
            {
                var dialogRes = MessageBox.Show("是否需要配置初始化？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (dialogRes == MessageBoxResult.OK)
                {
                    SettingModel = new SettingModel();
                    var settingStr = AppsettingSerializer.Serialize(SettingModel); // 配置字符串
                    ConfigHelper.SaveAppsetting(appsettingStr, settingStr);
                }
            });
        }
        private bool CheckData()
        {
            return false;
        }
    }
}
