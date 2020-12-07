using Newtonsoft.Json;
using STD.Framework.Core;
using STD.Framework.Utils;
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
    public class CommandController : Controller
    {
        //
        // GET: /MiniMES/minmes_command/

        public ActionResult Index()
        {
            return View();
        }

    }


    public class CommandApiController : ApiController
    //  public partial class WebApiController : ApiController

    {

        //11111根据线体ID获取数据
        [System.Web.Http.HttpPost]
        public dynamic GetCommandData(dynamic data)
        {
            List<dynamic> result = new List<dynamic>();
            string LineId = data.LineId;

            string TenantID = data.TenantID;

            string PlanDate = data.PlanDate;
            ParamQuery query = ParamQuery.Instance();
            query.AndWhere("TenantID", TenantID);
            query.AndWhere("LineId", LineId);
            if (!string.IsNullOrEmpty(PlanDate))
            {
                query.AndWhere("PlanDate", PlanDate);
            }
            //query.OrderBy("ID asc");

            try
            {
                List<minimes_command> listall = new minimes_commandService().GetModelList(ParamQuery.Instance()
                        .AndWhere("TenantID", TenantID)
                        .AndWhere("LineId", LineId)
                        .OrderBy("CreateDate asc"));

                return new { status = "success", message = "获取数据成功", rows = listall };
            }
            catch (Exception ex)
            {

                return new { statues = "error", message = ex.Message };
            }
        }
        //222增加一条命令
        [System.Web.Http.HttpPost]
        public dynamic EditPutCommandData(dynamic data)
        {
            List<dynamic> result = new List<dynamic>();
            string LineId = data.LineId;
            string LineName = data.LineName;
            string UserId = data.UserId;
            string UserCode = data.UserCode;
            string UserName = data.UserName;
            int CommandState = data.CommandState; //0暂停1启动
            string CommandModule = data.CommandModule;
            string CommandInfo = data.CommandInfo;
            string Remark = data.Remark;
            string TenantID = data.TenantID;
            string GId = data.GId;
            DateTime PlanDate = DateTime.Now.Date;
            DateTime CreateDate = DateTime.Now;

            try
            {
                int res1 = new minimes_commandService().Insert(ParamInsert.Instance()
                                .Column("LineId", LineId)
                                .Column("LineName", LineName)
                                .Column("UserId", UserId)
                                .Column("UserCode", UserCode)
                                .Column("UserName", UserName)

                                .Column("CommandState", CommandState)
                                .Column("CommandModule", CommandModule)
                                .Column("CommandInfo", CommandInfo)
                                .Column("Remark", Remark)
                                .Column("TenantID", TenantID)

                                .Column("CreateDate", CreateDate)
                                .Column("PlanDate", PlanDate)
                                .Column("GId", GId)

                                );
                bool bok1 = res1 > 0 ? true : false;
                dynamic item = new
                {
                    ResNum = res1,
                    Bok = bok1
                };
                result.Add(item);
                return new { status = "success", message = "新增数据成功", rows = result };
            }
            catch (Exception ex)
            {

                return new { statues = "error", message = ex.Message };
            }
        }
        //333删除后将此条数据增加到log里面
        [System.Web.Http.HttpPost]
        public dynamic EditDeleteCommandData(dynamic data)
        {
            List<dynamic> result = new List<dynamic>();
            int Id = data.Id;
            string LineId = data.LineId;
            string TenantId = data.TenantID;
            string Remark = data.Remark;
            try
            {
                List<minimes_command> listdone = new minimes_commandService().GetModelList(
                      ParamQuery.Instance()
                      .AndWhere("LineId", LineId)
                      .AndWhere("ID", Id)
                      .AndWhere("TenantID", TenantId)
                      );
                minimes_command md = listdone[0];
                new minimes_commandService().Delete(
                    ParamDelete.Instance()
                       .AndWhere("LineId", LineId)
                       .AndWhere("ID", Id)
                       .AndWhere("TenantID", TenantId)
                       );
                int res1 = new minimes_commandlogService().Insert(ParamInsert.Instance()
                            .Column("LineId", md.LineId)
                            .Column("LineName", md.LineName)
                            .Column("UserId", md.UserId)
                            .Column("UserCode", md.UserCode)
                            .Column("UserName", md.UserName)

                            .Column("CommandState", md.CommandState)
                            .Column("CommandModule", md.CommandModule)
                            .Column("CommandInfo", md.CommandInfo)
                            .Column("Remark", md.Remark + Remark)
                            .Column("TenantID", md.TenantID)

                            .Column("CreateDate", md.CreateDate)
                            .Column("PlanDate", md.PlanDate)
                            .Column("ID", md.ID)
                            .Column("LogDate", DateTime.Now)
                            .Column("GId", md.GId)
                            );
                bool bok1 = res1 > 0 ? true : false;
                dynamic item = new
                {
                    ResNum = res1,
                    Bok = bok1
                };
                result.Add(item);
                return new { status = "success", message = "删除数据成功", rows = result };
            }
            catch (Exception ex)
            {
                return new { statues = "error", message = ex.Message };
            }
        }
    }
}
