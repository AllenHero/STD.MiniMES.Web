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
    public class minimes_restdateController : Controller
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
                    query = "/api/MiniMES/minimes_restdate",
                    newkey = "/api/MiniMES/minimes_restdate/getnewkey",
                    edit = "/api/MiniMES/minimes_restdate/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    RestDate = "",
                },
                defaultRow = new
                {
                    RestDate = "",
                    //默认项
                    CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "RestDate", "CreateDate", "CreatePerson", "TenantID" }
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
                    string[] column = new string[] { "RestDate"};
                    DataTable dt = ExcelHelper.ImportDataTableFromExcel(file.InputStream, file.FileName, column, 0, 0);

                    int successCount = 0;
                    string ErrMsg = "";

                    using (var db = Db.Context("MiniMES"))
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string RestDate = dt.Rows[i]["RestDate"].ToString();

                            if (!string.IsNullOrEmpty(RestDate))
                            {
                                try
                                {
                                    int iResult = 0;

                                    minimes_restdate minimes_restdate = new minimes_restdateService().GetModel(ParamQuery.Instance().Select("RestDate").AndWhere("RestDate", RestDate));
                                    if (minimes_restdate == null)
                                    {
                                        iResult = db.Insert("minimes_restdate")
                                                    .Column("ID", Guid.NewGuid().ToString())
                                                    .Column("RestDate", RestDate)
                                                    .Column("CreateDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                                                    .Column("CreatePerson", SysHelper.GetUserName())
                                                    .Column("TenantID", SysHelper.GetTenantId())
                                                    .Execute();
                                    }
                                    if (iResult > 0)
                                    {
                                        successCount++;
                                    }
                                    else
                                    {
                                        ErrMsg += "休息日期[" + RestDate + "]保存失败！\r\n";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    return Json(new { status = false, message = "上传失败：" + ex.Message }, JsonRequestBehavior.AllowGet);
                                }
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

    public class minimes_restdateApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
            <settings defaultOrderBy='RestDate desc'>
                <select>*</select>
                <from>minimes_restdate</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
                <field name='RestDate'		cp='daterange'></field>
                </where>
            </settings>");
            var service = new minimes_restdateService();
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
                    minimes_restdate
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
                        item.TenantID = SysHelper.GetTenantId();
                    }
                }

                if (data.list.updated.Count > 0)
                {
                    foreach (var item in data.list.updated)
                    {
                        item.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        item.CreatePerson = SysHelper.GetUserName();
                        item.TenantID = SysHelper.GetTenantId();
                    }
                }
                var service = new minimes_restdateService();
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
