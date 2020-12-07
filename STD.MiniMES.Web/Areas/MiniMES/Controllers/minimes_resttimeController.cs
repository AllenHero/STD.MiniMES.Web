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
    public class minimes_resttimeController : Controller
    {
        public ActionResult Index()
        {
            List<dynamic> WorkShopList = new List<dynamic>();
            dynamic WorkShops = ApiDataSource.GetWorkShopList(SysHelper.GetTenantId(), null);
            for (int i = 0; i < WorkShops.Count; i++)
            {
                WorkShopList.Add(new { value = WorkShops[i].WorkShopId.Value, text = WorkShops[i].WorkShopName.Value });
            }

            var model = new
            {
                dataSource = new
                {
                    dsWrokShopName = WorkShopList
                },
                urls = new
                {
                    query = "/api/MiniMES/minimes_resttime",
                    newkey = "/api/MiniMES/minimes_resttime/getnewkey",
                    edit = "/api/MiniMES/minimes_resttime/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    WorkShopId = ""
                },
                defaultRow = new
                {
                    //默认项
                    CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "WorkShopId", "RestTime", "CreateDate", "TenantId" }
                }
            };

            return View(model);
        }
    }

    public class minimes_resttimeApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
            <settings defaultOrderBy='ID'>
                <select>*</select>
                <from>minimes_resttime</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
                <field name='WorkShopId'		cp='equal'></field>
                </where>
            </settings>");
            var service = new minimes_resttimeService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return Guid.NewGuid().ToString();
        }

        [System.Web.Http.HttpPost]
        public dynamic Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
            <settings>
                <table>
                    minimes_resttime
                </table>
                <where>
                    <field name='ID' cp='equal'></field>
                </where>
            </settings>");
            try
            {
                if (data.list.inserted.Count > 0)
                {
                    foreach (var item in data.list.inserted)
                    {
                        item.TenantId = SysHelper.GetTenantId();
                    }
                }

                if (data.list.updated.Count > 0)
                {
                    foreach (var item in data.list.updated)
                    {

                    }
                }
                var service = new minimes_resttimeService();
                var result = service.Edit(null, listWrapper, data);
                if (result > 0)
                    return new { status = true, message = "保存成功" };
                else
                    return new { status = false, message = "保存失败" };
            }
            catch (Exception ex)
            {
                return new { status = false, message = ex.Message };
            }
        }
    }
}
