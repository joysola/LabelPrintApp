using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace LabelPrint.Common
{
    public class AppsettingSerializer
    {
        private static readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        /// <summary>
        /// 将对象序列化为Base64字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            string result = null;
            using (MemoryStream ms = new MemoryStream())
            {
                _binaryFormatter.Serialize(ms, obj);
                var bytes = ms.ToArray();
                result = Convert.ToBase64String(bytes); // 配置字符串
            }
            return result;
        }
        /// <summary>
        /// 将base64字符串解析成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string base64Str)
        {
            T result;
            var bytes = Convert.FromBase64String(base64Str);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                result = (T)_binaryFormatter.Deserialize(ms);
            }
            return result;
        }
    }
}
