using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPrint.Domain
{
    /// <summary>
    /// 返回实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 是否成功请求
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string msg { get; set; }
    }

    /// <summary>
    /// 分页实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponsePage<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int current { get; set; }

        /// <summary>
        /// 当前分页条件下共多少页
        /// </summary>
        public int pages { get; set; }

        /// <summary>
        /// 每页记录的数量
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 具体实例
        /// </summary>
        public List<T> records { get; set; }
    }
}
