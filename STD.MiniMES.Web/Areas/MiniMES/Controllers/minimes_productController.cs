using Newtonsoft.Json;
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
    public class minimes_productController : Controller
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
                    query = "/api/MiniMES/minimes_product",
                    newkey = "/api/MiniMES/minimes_product/getnewkey",
                    edit = "/api/MiniMES/minimes_product/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                },
                form = new
                {
                    InventoryCode = "",
                    InventoryName = "",
                    TenantId = SysHelper.GetTenantId()
                },
                defaultRow = new
                {
                    TenantId = SysHelper.GetTenantId(),
                    CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                },
                setting = new
                {
                    idField = "InventoryId",
                    postListFields = new string[] { "InventoryId", "InventoryCode", "InventoryName", "TallyRatio", "TenantId" }
                }
            };
            return View(model);
        }
    }

    public class minimes_productApiController : ApiController
    {
        /// <summary>
        /// 返回list
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
            <settings defaultOrderBy='InventoryCode'>
                <select>*</select>
                <from>minimes_product </from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='InventoryCode'		cp='like'></field>   
                <field name='InventoryName'		cp='like'></field>   
                <field name='TenantId'		cp='equal'></field>   
                </where>
            </settings>");
            var service = new minimes_productService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }


        #region 产生主键
        /// <summary>
        /// 产生主键
        /// </summary>
        /// <returns></returns>
        public string GetNewKey()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion

        #region    同步产品
        public dynamic GetSyn(RequestWrapper query)
        {
            var TenantId = SysHelper.GetTenantId();
            List<dynamic> dsInventory = new List<dynamic>();
            List<dynamic> result = new List<dynamic>();
            try
            {

                var pageID = 1;
                var pageRows = 4000;
                var dsInventoryPagging = ApiDataSource.GetInventoryList(TenantId, null, null, null, "" + pageID, "" + pageRows);
                while (dsInventoryPagging != null && dsInventoryPagging.Count > 0)
                {

                    dsInventory.AddRange(dsInventoryPagging);
                    pageID++;
                    dsInventoryPagging = ApiDataSource.GetInventoryList(TenantId, null, null, null, "" + pageID, "" + pageRows);
                }

                ParamQuery paramQuery = ParamQuery.Instance()
        .AndWhere("TenantId", TenantId);
                List<minimes_product> list = new minimes_productService().GetModelList(paramQuery);
                foreach (var row in dsInventory)
                {
                    //state 2新增 1修改 0不变
                    dynamic dy = new
                    {
                        InventoryId = row.InventoryId,
                        InventoryCode = row.InventoryCode,
                        InventoryName = row.InventoryName,
                        InventoryClassId = row.InventoryClassId,
                        StandardUPH = row.StandardUPH,
                        StandardPersonNum = row.StandardPersonNum,
                        StandardProcessNum = row.StandardProcessNum,
                        StandardTime = row.StandardTime,
                        Spec = row.Spec,
                        Unit = row.Unit,
                        IsEnable = row.IsEnable,
                        State = 2
                    };
                    foreach (var item in list)
                    {
                        string InventoryCode = dy.InventoryCode + "";
                        string InventoryName = dy.InventoryName + "";
                        if (item.InventoryCode + "" == InventoryCode)
                        {
                            if (item.InventoryName + "" == InventoryName)
                                dy.State = 0;
                            else
                                dy.State = 1;
                            break;
                        }
                    }
                    result.Add(dy);
                }

                foreach (var row in result)
                {
                    string InventoryCode = row.InventoryCode + "";
                    string InventoryName = row.InventoryName + "";
                    string InventoryClassId = row.InventoryClassId + "";
                    if (row.State == 2)
                    {
                        new minimes_productService().Insert(ParamInsert.Instance()
                        .Column("InventoryId", Guid.NewGuid())
                        .Column("InventoryCode", InventoryCode)
                        .Column("InventoryName", InventoryName)
                        .Column("InventoryClassId", InventoryClassId)
                        //.Column("StandardUPH", row.StandardUPH)
                        //.Column("StandardPersonNum", row.StandardPersonNum)
                        // .Column("StandardProcessNum", row.StandardProcessNum)
                        // .Column("StandardTime", row.StandardTime)
                         .Column("Spec", row.Spec)
                         .Column("Unit", row.Unit)
                         .Column("IsEnable", row.IsEnable)
                         .Column("TenantId", TenantId)
                         .Column("TallyRatio", 1)
                        );
                    }
                    else if (row.State == 1)
                    {
                        var param = ParamUpdate.Instance()
                          .Column("InventoryName", InventoryName)
                          .AndWhere("InventoryCode", InventoryCode);
                        new minimes_productService().Update(param);
                    }

                }
            }
            catch(Exception ex)
            {
                return -1;
            }
            return 1;
        }


    #endregion

        #region 编辑数据
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
                    minimes_product
                </table>
                <where>
                    <field name='InventoryId' cp='equal'></field>
                </where>
            </settings>");
            var service = new minimes_productService();
            var result = service.Edit(null, listWrapper, data);
        }
        #endregion

    }

}
