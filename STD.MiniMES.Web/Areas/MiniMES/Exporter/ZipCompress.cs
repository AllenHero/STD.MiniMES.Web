/*************************************************************************
 * 文件名称 ：ZipCompress.cs                          
 * 描述说明 ：压缩为ZIP
 * 
 * 创建信息 : 
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 深圳市数本科技开发有限公司 www.stdlean.com
**************************************************************************/

using System.IO;
using STD.Framework.Utils.Ionic.Zip;

namespace STD.MiniMES.Web
{
    public class ZipCompress: ICompress
    {
        public string Suffix(string orgSuffix)
        {
            return "zip";
        }

        public Stream Compress(Stream fileStream,string fullName)
        {
            using (var zip = new ZipFile())
            {
                zip.AddEntry(fullName, fileStream);
                Stream zipStream = new MemoryStream();
                zip.Save(zipStream);
                return zipStream;
            }
        }
    }
}
