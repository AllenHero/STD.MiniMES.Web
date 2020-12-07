using STD.Framework.Core;
using STD.MiniMES.Models;
using STD.MiniMES.Web;
using STD.MiniMES.Web.Areas.MiniMES.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    public class minimes_shifttimeController : Controller
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
                    query = "/api/MiniMES/minimes_shifttime",
                    newkey = "/api/MiniMES/minimes_shifttime/getnewkey",
                    edit = "/api/MiniMES/minimes_shifttime/edit"
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
                    ProductionDate = "",
                    CreatePerson="",
                },
                defaultRow = new
                {
                    //默认项
                    CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "WorkShopId", "WorkShopName", "ProductionDate", "ShiftTime1", "ShiftTime2", "CreateDate", "CreatePerson", "TenantID" }
                }
            };

            return View(model);
        }

        #region  导入

        /// <summary>
        /// 文件导入
        /// </summary>
        /// <param name="controllers">文件要保存的文件夹</param>
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [Web.MvcMenuFilter(false)]
        public ActionResult EditImport(string controllers)
        {
            string info = string.Empty;
            object objJson = "";
            try
            {
                //获取客户端上传的文件集合
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                //判断是否存在文件
                if (files.Count > 0)
                {
                    //获取文件集合中的第一个文件(每次只上传一个文件)
                    HttpPostedFile file = files[0];
                    string[] column = new string[] { "WorkShopName", "ProductionDate", "ShiftTime1", "ShiftTime2" };
                    DataTable dt = ExcelHelper.ImportDataTableFromExcel(file.InputStream, file.FileName, column, 0, 0);

                    int successCount = 0;
                    string ErrMsg = "";

                    using (var db = Db.Context("MiniMES"))
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string WorkShopName = dt.Rows[i]["WorkShopName"].ToString();
                            string ProductionDate = dt.Rows[i]["ProductionDate"].ToString();
                            string ShiftTime1 = dt.Rows[i]["ShiftTime1"].ToString();
                            string ShiftTime2 = dt.Rows[i]["ShiftTime2"].ToString();
                            double iShiftTime1 = 0;
                            double iShiftTime2 = 0;
                            double.TryParse(ShiftTime1, out iShiftTime1);
                            double.TryParse(ShiftTime2, out iShiftTime2);
                            DateTime dateTime = DateTime.Now;
                            if (DateTime.TryParse(ProductionDate,out dateTime))
                            {
                                ProductionDate = dateTime.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                ProductionDate = "";
                            }
                            string WorkShopId = "";
                            if (WorkShopName == "注塑车间")
                            {
                                WorkShopId = "08ff98d5-aaa4-4a58-8bdc-244b1ddaef20";
                            }
                            else if(WorkShopName == "冲压车间")
                            {
                                WorkShopId = "c3b621ea-29a0-4d7b-b8cc-87f72aee5612";
                            }
                            try
                            {
                                int iResult = 0;
                                string WorkShopIdWhere = "WorkShopId='"+ WorkShopId + "' and 1 ";
                                if (string.IsNullOrEmpty(WorkShopId))
                                {
                                    WorkShopIdWhere = "WorkShopId is null and 1 ";
                                }
                                string ProductionDateWhere = "ProductionDate like '" + ProductionDate + "%' and 1 ";
                                if (string.IsNullOrEmpty(ProductionDate))
                                {
                                    ProductionDateWhere = "ProductionDate is null and 1";
                                }
                                minimes_shifttime minimes_shifttime = new minimes_shifttimeService().GetModel(ParamQuery.Instance().Select("ProductionDate").AndWhere(WorkShopIdWhere,"1").AndWhere(ProductionDateWhere, "1"));

                                if (minimes_shifttime == null)
                                {
                                    iResult = db.Insert("minimes_shifttime")
                                                .Column("ID", Guid.NewGuid().ToString())
                                                .Column("WorkShopId", WorkShopId)
                                                .Column("WorkShopName", WorkShopName)
                                                .Column("ProductionDate", ProductionDate)
                                                .Column("ShiftTime1", ShiftTime1)
                                                  .Column("ShiftTime2", ShiftTime2)
                                                .Column("CreateDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                                                .Column("CreatePerson", SysHelper.GetUserName())
                                                .Column("TenantID", SysHelper.GetTenantId())
                                                .Execute();
                                }
                                else
                                {
                                    iResult = db.Update("minimes_shifttime")
                                                   .Column("WorkShopId", WorkShopId)
                                                .Column("WorkShopName", WorkShopName)
                                                .Column("ProductionDate", ProductionDate)
                                                .Column("ShiftTime1", ShiftTime1)
                                                  .Column("ShiftTime2", ShiftTime2)
                                                .Column("CreateDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                                                .Column("CreatePerson", SysHelper.GetUserName())
                                                .Column("TenantID", SysHelper.GetTenantId())
                                                .Where("ID", minimes_shifttime.ID)
                                                .Execute();
                                }
                                if (iResult > 0)
                                {
                                    successCount++;
                                }
                                else
                                {
                                    ErrMsg += "生产日期[" + ProductionDate + "]保存失败！\r\n";
                                }
                            }
                            catch (Exception ex)
                            {
                                return Json(new { status = false, message = "上传失败：" + ex.Message }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    return Json(new { status = true, message = ("导入完成，共导入" + successCount + "条记录。" + (ErrMsg == "" ? "" : "但存在以下错误：\r\n " + ErrMsg)) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "请选择文件！" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "上传失败：" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }

    public class minimes_shifttimeApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
            <settings defaultOrderBy='ID'>
                <select>*</select>
                <from>minimes_shifttime</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
                <field name='WorkShopId'		cp='equal'></field>
                <field name='ProductionDate'		cp='equal'></field>
                <field name='CreatePerson'		cp='equal'></field>
                </where>
            </settings>");
            var service = new minimes_shifttimeService();
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
                    minimes_shifttime
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
                        item.CreatePerson = SysHelper.GetUserName();
                        if (item.WorkShopId == "08ff98d5-aaa4-4a58-8bdc-244b1ddaef20")
                        {
                            item.WorkShopName = "注塑车间";
                        }
                        else if(item.WorkShopId == "c3b621ea-29a0-4d7b-b8cc-87f72aee5612")
                        {
                            item.WorkShopName = "冲压车间";
                        }
                        item.TenantID = SysHelper.GetTenantId();
                    }
                }

                if (data.list.updated.Count > 0)
                {
                    foreach (var item in data.list.updated)
                    {
                        item.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        item.CreatePerson = SysHelper.GetUserName();

                        if (item.WorkShopId == "08ff98d5-aaa4-4a58-8bdc-244b1ddaef20")
                        {
                            item.WorkShopName = "注塑车间";
                        }
                        else if (item.WorkShopId == "c3b621ea-29a0-4d7b-b8cc-87f72aee5612")
                        {
                            item.WorkShopName = "冲压车间";
                        }
                        item.TenantID = SysHelper.GetTenantId();
                    }
                }
                var service = new minimes_shifttimeService();
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
