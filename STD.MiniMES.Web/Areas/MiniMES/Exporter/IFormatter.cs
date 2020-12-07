/*************************************************************************
 * 文件名称 ：IFormatter.cs                          
 * 描述说明 ：字段格式化接口
 * 
 * 创建信息 : 
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 深圳市数本科技开发有限公司 www.stdlean.com
**************************************************************************/

namespace STD.MiniMES.Web
{
    public interface IFormatter
    {
        object Format(object value);
    }
}
