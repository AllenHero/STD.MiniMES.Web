using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using STD.Framework.Core;
using STD.MiniMES.Models;
using STD.MiniMES.Web;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    public class minimes_parameterController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/MiniMES/minimes_parameter",
                    newkey = "/api/MiniMES/minimes_parameter/getnewkey",
                    edit = "/api/MiniMES/minimes_parameter/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    //查询条件
                    ParameterCode = "",
                    ParameterName = "",
                    TenantID = SysHelper.GetTenantId()
                },
                defaultRow = new
                {
                    //默认项
                    TenantID = SysHelper.GetTenantId()
                },
                setting = new
                {
                    //列表显示
                    idField = "ID",
                    postListFields = new string[] { "ID", "ParameterCode", "ParameterName", "ParameterValue", "TenantID" }
                }
            };

            return View(model);
        }

    }

    public class minimes_parameterApiController : ApiController
    {
        /// <summary>
        /// 返回list
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
            <settings defaultOrderBy='ParameterCode'>
                <select>*</select>
                <from>minimes_parameter</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                  <field name='TenantID'	cp='equal'></field>
                 <field name='ParameterCode'	cp='like'></field>
                 <field name='ParameterName'	cp='like'></field>
                </where>
            </settings>");
            var service = new minimes_parameterService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        /// <summary>
        /// 产生主键
        /// </summary>
        /// <returns></returns>
        public string GetNewKey()
        {
            return Guid.NewGuid().ToString();
        }


        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="data">json</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
            <settings>
                <table>
                    minimes_parameter
                </table>
                <where>
                    <field name='ID' cp='equal'></field>
                </where>
            </settings>");
            var service = new minimes_parameterService();
            var result = service.Edit(null, listWrapper, data);
        }
    }

}
