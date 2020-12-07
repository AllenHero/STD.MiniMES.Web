using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace STD.MiniMES.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpHelper
    {
        public static List<KeyValuePair<string, string>> SaveFilesAndReturnFilePath(HttpContext context, string basePath)
        {
            var filePathList = new List<KeyValuePair<string, string>>();

            var appPath = context.Server.MapPath("~/");

            string path = Path.Combine(
                appPath, @"upload",
                basePath,
                DateTime.Now.ToString("yyyyMM"));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            for (int iFile = 0; iFile < context.Request.Files.Count; iFile++)
            {
                ///'检查文件扩展名字  
                HttpPostedFile postedFile = context.Request.Files[iFile];


                string fileSavePath = Path.Combine(path, Guid.NewGuid() + Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(fileSavePath);

                var fileSrc = fileSavePath.Replace(appPath, ""); //转换成相对路径  
                fileSrc = fileSrc.Replace(@"\", @"/");

                var nameAndPath = new KeyValuePair<string, string>(postedFile.FileName, "/" + fileSrc);

                filePathList.Add(nameAndPath);
            }

            return filePathList;
        }

        public static List<dynamic> SaveFilesAndReturnFileInfo(HttpContext context, string basePath)
        {
            List<dynamic> filePathList = new List<dynamic>();

            var appPath = context.Server.MapPath("~/");

            string path = Path.Combine(
                appPath, @"upload",
                basePath,
                DateTime.Now.ToString("yyyyMM"));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            for (int iFile = 0; iFile < context.Request.Files.Count; iFile++)
            {
                ///'检查文件扩展名字  
                HttpPostedFile postedFile = context.Request.Files[iFile];


                string fileSavePath = Path.Combine(path, Guid.NewGuid() + Path.GetExtension(postedFile.FileName));
                postedFile.SaveAs(fileSavePath);

                var fileSrc = fileSavePath.Replace(appPath, ""); //转换成相对路径  
                fileSrc = fileSrc.Replace(@"\", @"/");

                //var nameAndPath = new KeyValuePair<string, string>(postedFile.FileName, "/" + fileSrc);

                //filePathList.Add(nameAndPath);

                dynamic file = new {
                    FileTitle = postedFile.FileName,
                    FilePath = "/" + fileSrc,
                    FileType = postedFile.ContentType,
                    FileSize = postedFile.ContentLength
                };

                filePathList.Add(file);
            }

            return filePathList;
        }

        #region 公共方法：请求webApi接口，获取并返回结果
        public static dynamic GetWebApi(string url, int timeOut = 180000)
        {
            try
            {
                HttpClient client = new HttpClient();

                string resultStr = "";

                HttpResponseMessage response = null;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = client.GetAsync(url).Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    resultStr = response.Content.ReadAsStringAsync().Result;
                }

                try
                {
                    var serializer = new JavaScriptSerializer();
                    dynamic obj = serializer.Deserialize(resultStr, typeof(object));

                    dynamic result = JsonConvert.DeserializeObject(resultStr);
                    return result;
                }
                catch (Exception ex)
                {
                    return new { status = false, message = resultStr };
                }
            }
            catch (Exception ex)
            {
                return new { status = false, message = ex.Message };
            }
        }
        /// <summary>
        /// POST请求WEBAPI
        /// </summary>
        /// <param name="url">API地址</param>
        /// <param name="data">POST参数</param>
        /// <param name="timeOut">超时时间，毫秒单位</param>
        /// <returns></returns>
        public static dynamic PostWebApi(string url, dynamic data, int timeOut)
        {
            try
            {
                HttpClient client = new HttpClient();

                string resultStr = "";

                HttpResponseMessage response = null;
                HttpContent content = new StringContent(data.ToString());

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = client.PostAsync(url, content).Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    resultStr = response.Content.ReadAsStringAsync().Result;
                }

                try
                {
                    var serializer = new JavaScriptSerializer();
                    dynamic obj = serializer.Deserialize(resultStr, typeof(object));

                    dynamic result = JsonConvert.DeserializeObject(resultStr);
                    return result;
                }
                catch (Exception ex)
                {
                    return new { status = false, message = resultStr };
                }
            }
            catch (Exception ex)
            {
                return new { status = false, message = ex.Message };
            }
        }

        #endregion
    }

}