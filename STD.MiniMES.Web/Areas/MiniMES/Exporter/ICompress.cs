/*************************************************************************
 * 文件名称 ：ICompress.cs                          
 * 描述说明 ：文件压缩接口
 * 
 * 创建信息 : 
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 深圳市数本科技开发有限公司 www.stdlean.com
**************************************************************************/

using System.IO;

namespace STD.MiniMES.Web
{
    public interface ICompress
    {
        string Suffix(string orgSuffix);
        Stream Compress(Stream fileStream,string fullName);
    }
}
