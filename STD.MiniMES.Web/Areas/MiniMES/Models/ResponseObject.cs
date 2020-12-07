using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    public class ResponseObject
    {
        public ResponseObject()
        {
            status = false;
            message = "失败";
            pageCount = 1;
            page = 1;
            total = 1;
            rows = new List<dynamic>();
        }
        public ResponseObject(bool status)
        {
            this.status = status;
            message = "失败";
            pageCount = 1;
            page = 1;
            total = 1;
            rows = new List<dynamic>();
        }
        public ResponseObject(bool status, string message, int pageCount, int page, int total, dynamic rows)
        {
            this.status = status;
            this.message = message;
            this.pageCount = pageCount;
            this.page = page;
            this.total = total;
            this.rows = rows;
        }
        public ResponseObject(bool status, string message, int page, int total, dynamic rows)
        {
            this.status = status;
            this.message = message;
            this.pageCount = (int)Math.Ceiling(total * 1.0 / rows.Count);
            this.page = page;
            this.total = total;
            this.rows = rows;
        }
        public ResponseObject(bool status, string message)
        {
            this.status = status;
            this.message = message;
            this.pageCount = 1;
            this.page = 1;
            this.total = 0;
            this.rows = new List<dynamic>();
        }
        public ResponseObject(bool status, dynamic rows)
        {
            this.status = status;
            this.rows = rows;
            if (status)
                message = "成功";
            else
                message = "错误";
            pageCount = 1;
            page = 1;
            total = 1;
        }
        /// <summary>
        /// 返回状态
        /// </summary>
        public bool status { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 当前页数据
        /// </summary>
        public dynamic rows { get; set; }

    }
}