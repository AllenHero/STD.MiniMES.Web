using STD.Framework.Core;
using STD.MiniMES.Models;
using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    public class minimes_order_recordController : Controller
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
                    query = "/api/MiniMES/minimes_order_record",
                    newkey = "/api/MiniMES/minimes_order_record/getnewkey",
                    edit = "/api/MiniMES/minimes_order_record/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    OrderNo = "",
                    PlanDate = "",
                    ProductCode = "",
                    ProductName = "",
                    LineId = "",
                    LineName = "",
                    TenantId = SysHelper.GetTenantId()
                },
                defaultRow = new
                {
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "OrderNo", "PlanDate", "LineId", "LineName", "ProductCode", "ProductName", "Qty", "QtyNG", "TenantId" }
                }
            };

            return View(model);
        }


    }

    public class minimes_order_recordApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
            <settings defaultOrderBy='ID'>
                <select>*</select>
                <from>minimes_order_record</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
                    <field name='PlanDate'		cp='daterange'></field>
                     <field name='OrderNo'		cp='equal'></field>   
                     <field name='TenantId'		cp='equal'></field>   
                </where>
            </settings>");
            var service = new minimes_order_recordService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return Guid.NewGuid().ToString();
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
            <settings>
                <table>
                    minimes_order_record
                </table>
                <where>
                    <field name='ID' cp='equal'></field>
                </where>
            </settings>");
            var service = new minimes_order_recordService();
            var result = service.Edit(null, listWrapper, data);
        }
    }


}
