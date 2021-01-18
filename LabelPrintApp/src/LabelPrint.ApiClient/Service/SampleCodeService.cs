using HttpClientExtension.Service;
using LabelPrint.ApiClient.Api;
using LabelPrint.Domain;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LabelPrint.ApiClient.Service
{
    public class SampleCodeService : BaseService<SampleCodeService>
    {
        public SampleTSC GetSamplebyCode(string code)
        {
            var dateTime = DateTime.Now.ToString("yyyyMMdd");
            var sb = new StringBuilder();
            using (MD5 md5 = MD5.Create()) //实例化一个md5对像
            {
                var md5Bytes = md5.ComputeHash(Encoding.UTF8.GetBytes("deepsight" + dateTime));
                foreach (var item in md5Bytes)
                {
                    // 大写用X，小写用x
                    sb.Append(item.ToString("x2"));
                }
            }
            var result = SampleCodeApi.Client.GetSamplebyCode(code, sb.ToString());
            return result.data;
        }
    }
}
