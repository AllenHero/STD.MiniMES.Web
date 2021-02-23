using STD.Framework.Core;
using STD.Framework.Utils;
using STD.Framework.Utils.EPPlus;
using STD.Framework.Utils.EPPlus.Style;
using STD.MiniMES.Models;
using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    public class minimes_stop_recordController : Controller
    {
        public ActionResult Index()
        {
            List<dynamic> WorkShopList = new List<dynamic>();
            dynamic WorkShops = ApiDataSource.GetWorkShopList(SysHelper.GetTenantId(), null);
            for (int i = 0; i < WorkShops.Count; i++)
            {
                WorkShopList.Add(new { value = WorkShops[i].WorkShopId.Value, text = WorkShops[i].WorkShopName.Value });
            }

            List<dynamic> LineList = new List<dynamic>();
            dynamic Lines = ApiDataSource.GetLineList(SysHelper.GetTenantId(), null,null,null,null);
            for (int i = 0; i < Lines.Count; i++)
            {
                LineList.Add(new { value = Lines[i].LineId.Value, text = Lines[i].LineName.Value });
            }
            var model = new
            {
                dataSource = new
                {
                    dsWrokShopName = WorkShopList,
                    dsLineName = LineList
                },
                urls = new
                {
                    query = "/api/MiniMES/minimes_stop_record",
                    newkey = "/api/MiniMES/minimes_stop_record/getnewkey",
                    edit = "/api/MiniMES/minimes_stop_record/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    WorkShopId = "",
                    LineId = "",
                    Shift="",
                    Date = "",
                    Operator="",
                    Type="",
                    OrderNo="",

                },
                defaultRow = new
                {
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "WorkShopId", "LineId", "Type", "OrderNo", "BeginTime", "EndTime", "Shift", "Operator", "Remark","Date", "CreateDate", "TenantID" }
                }
            };
            return View(model);
        }
    }

    public class minimes_stop_recordApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            try
            {
                query.LoadSettingXmlString(@"
            <settings defaultOrderBy='Date DESC'>
                <select>ID,WorkShopId,WorkShopName,LineId,LineName,OrderNo,Type,BeginTime,EndTime,Shift,Operator,Remark,Date,round(TIMESTAMPDIFF(SECOND,BeginTime,EndTime)/60,2) as StopTime</select>
                <from>minimes_stop_record</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
                     <field name='WorkShopId'		cp='equal'></field>
                     <field name='LineId'		cp='equal'></field>   
                     <field name='Shift'		cp='equal'></field>   
                     <field name='Date'		cp='DateRange'></field>   
                     <field name='Operator'		cp='equal'></field>  
                     <field name='Type'		cp='equal'></field>   
                     <field name='OrderNo'		cp='equal'></field>  
                </where>
            </settings>");
                var service = new minimes_stop_recordService();
                var pQuery = query.ToParamQuery();
                var result = service.GetDynamicListWithPaging(pQuery);
                foreach (var item in result.rows)
                {
                    if (item.Type=="1")
                    {
                        item.Type = "换模";
                    }
                    else if(item.Type == "2")
                    {
                        item.Type = "处理异常";
                    }
                    else if (item.Type == "3")
                    {
                        item.Type = "机台维修";
                    }
                    else if (item.Type == "4")
                    {
                        item.Type = "修模";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public string GetNewKey()
        {
            return Guid.NewGuid().ToString();
        }

        [System.Web.Http.HttpPost]
        public dynamic Edit(dynamic data)
        {
            int result = 0;
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
            <settings>
                <table>
                    minimes_stop_record
                </table>
                <where>
                    <field name='ID' cp='equal'></field>
                </where>
            </settings>");
            try
            {
                using (var db = Db.Context("MiniMES"))
                {

                    if (data.list.inserted.Count > 0)
                    {
                        foreach (var item in data.list.inserted)
                        {

                        }
                    }

                    if (data.list.updated.Count > 0)
                    {
                        foreach (var item in data.list.updated)
                        {
                            string Remark = item.Remark;
                            string ID = item.ID;
                            result = db.Update("minimes_stop_record")
                            .Column("Remark", Remark)
                            .Where("ID", ID)
                            .Execute();
                        }
                    }
                    if (result > 0)
                        return new { status = true, message = "保存成功" };
                    else
                        return new { status = false, message = "保存失败" };
                }
            }
            catch (Exception ex)
            {
                return new { status = false, message = ex.Message };
            }
        }
    }
}
