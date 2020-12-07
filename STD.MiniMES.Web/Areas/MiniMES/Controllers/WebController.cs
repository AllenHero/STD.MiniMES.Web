using log4net;
using Newtonsoft.Json;
using STD.Framework.Core;
using STD.Framework.Utils;
using STD.MiniMES.Models;
using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    public class WebApiController : ApiController
    {
        public string GetTest()
        {
            return "3";
        }

        #region   DataTable转Json

        private string ConvertDataTableToJson(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0)
            {
                //如果查询到的数据为空则返回标记ok:false
                sb.Append("{\"ok\":false}");
            }
            else
            {
                sb.Append("{\"ok\":true,");
                for (int j = 0; j < 1; j++)
                {
                    sb.Append(string.Format("\"{0}\":[", dt.TableName));

                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("{");
                        for (int i = 0; i < dr.Table.Columns.Count; i++)
                        {
                            sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), dr[i].ToString().Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                        sb.Remove(sb.ToString().LastIndexOf(','), 1);
                        sb.Append("},");
                    }

                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("],");
                }
                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("}");
            }
            return sb.ToString();
        }

        #endregion


        #region  获取条码

        public dynamic GetBarcode(string PlanDate, string WorkShopCode, string LineCode, string WorkSheetNo, string TenantID)
        {
            ParamQuery paramQuery = ParamQuery.Instance()
                .AndWhere("TenantID", TenantID);
            if (!string.IsNullOrEmpty(PlanDate))
                paramQuery = paramQuery.AndWhere("PlanDate", PlanDate);
            if (!string.IsNullOrEmpty(WorkShopCode))
                paramQuery = paramQuery.AndWhere("WorkShopCode", WorkShopCode);
            if (!string.IsNullOrEmpty(LineCode))
                paramQuery = paramQuery.AndWhere("LineCode", LineCode);
            if (!string.IsNullOrEmpty(WorkSheetNo))
                paramQuery = paramQuery.AndWhere("WorkSheetNo", WorkSheetNo);
            List<minimes_barcode> group = new minimes_barcodeService().GetModelList(paramQuery);

            DataTable dt = new DataTable("DataTable");
            dt.Columns.Add("ID");
            dt.Columns.Add("Barcode");
            dt.Columns.Add("PlanDate");
            dt.Columns.Add("WorkShopCode");
            dt.Columns.Add("LineCode");
            dt.Columns.Add("Qty");
            dt.Columns.Add("WorkSheetNo");
            dt.Columns.Add("SerialNumber");
            dt.Columns.Add("CreateDate");
            dt.Columns.Add("TenantID");

            foreach (var item in group)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = item.ID;
                dr["Barcode"] = item.Barcode;
                dr["PlanDate"] = item.PlanDate;
                dr["WorkShopCode"] = item.WorkShopCode;
                dr["LineCode"] = item.LineCode;
                dr["Qty"] = item.Qty;
                dr["WorkSheetNo"] = item.WorkSheetNo;
                dr["SerialNumber"] = item.SerialNumber;
                dr["CreateDate"] = item.CreateDate;
                dr["TenantID"] = item.TenantID;
                dt.Rows.Add(dr);
            }
            string result = JsonConvert.SerializeObject(this.ConvertDataTableToJson(dt));
            return JsonConvert.DeserializeObject(this.ConvertDataTableToJson(dt));
        }

        #endregion


        #region 根据线体获取工单

        //根据线体获取工单
        public dynamic GetOrderNo(string LineId, string Date, string TenantId)
        {
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(Date);
                //List<dynamic> result = ApiDataSource.GetWorkSheetList(TenantId, dateTime, null, null, null).ToObject<List<dynamic>>();
                List<dynamic> result = ApiDataSource.GetWorkSheetList(TenantId, null, LineId, null, null, null, null, null, null, null).ToObject<List<dynamic>>();
                //List<dynamic> worksheetlist = result.Where(x => x.LineId == LineId).ToList();
                return new { status = "success", message = "获取数据成功", rows = result };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
        }

        #endregion


        #region   获取工单详情

        //获取工单详情
        public dynamic GetOrderNoDetail(string LineId, string OrderNo, string Date, string TenantId)
        {
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(Date);
                ParamQuery paramQuery = ParamQuery.Instance()
                    .AndWhere("TenantID", TenantId);
                if (!string.IsNullOrEmpty(LineId))
                    paramQuery = paramQuery.AndWhere("LineId", LineId);
                if (!string.IsNullOrEmpty(OrderNo))
                    paramQuery = paramQuery.AndWhere("OrderNo", OrderNo);
                //if (!string.IsNullOrEmpty(Date))
                //    paramQuery = paramQuery.AndWhere("PlanDate", Date);
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                minimes_order_record result = new minimes_order_record();
                if (list.Count > 0)//已经有数据
                {
                    result = list[0];
                }
                else
                {
                    //List<dynamic> listWorkSheet = ApiDataSource.GetWorkSheetList(TenantId, dateTime, null, OrderNo, null).ToObject<List<dynamic>>();
                    List<dynamic> listWorkSheet = ApiDataSource.GetWorkSheetList(TenantId, OrderNo, LineId, null, null, null, null, null, null, null).ToObject<List<dynamic>>();
                    //List<dynamic> worksheetlist = listWorkSheet.Where(x => x.LineId == LineId).ToList();
                    if (listWorkSheet.Count < 1)
                        return new { status = "error", message = "无可用工单" };
                    result.LineId = listWorkSheet[0].LineId + "";
                    result.LineName = listWorkSheet[0].LineName + "";
                    List<dynamic> ilinelist = ApiDataSource.GetLineList(TenantId, null, null, LineId, null).ToObject<List<dynamic>>();
                    if (ilinelist != null && ilinelist.Count > 0)
                    {
                        result.LineName = ilinelist[0].LineName;
                    }

                    result.OrderNo = OrderNo;
                    result.ProductCode = listWorkSheet[0].ProductCode + "";
                    result.ProductName = listWorkSheet[0].ProductName + "";
                    result.Qty = 0;
                    result.QtyNG = 0;
                    result.PlanQty = listWorkSheet[0].PlanCount;
                    result.State = 1;
                    result.PlanDate = dateTime;
                    result.OrderPlanDate = dateTime;
                }

                minimes_productService hoursReportService = new minimes_productService();
                List<minimes_product> reportList = hoursReportService.GetModelList(ParamQuery.Instance()
                    .AndWhere("TenantID", TenantId)
                    .AndWhere("InventoryCode", result.ProductCode)
                    );
                if (reportList.Count < 1)
                {
                    result.Ratio = 1;
                    //return new { status = "error", message = "找不到该产品" };
                }

                foreach (var row in reportList)
                {
                    if (result.ProductCode == row.InventoryCode)
                        result.Ratio = row.TallyRatio.Value;
                }
                List<dynamic> linelist = ApiDataSource.GetLineList(TenantId, null, null, LineId, null).ToObject<List<dynamic>>();
                if (linelist.Count < 1)
                    return new { status = "error", message = "找不到该产线" };
                dynamic date = new
                {
                    OrderNo = result.OrderNo,
                    ProductCode = result.ProductCode,
                    ProductName = result.ProductName,
                    Qty = result.Qty,
                    QtyNG = result.QtyNG,
                    PlanQty = result.PlanQty,
                    Ratio = result.Ratio,
                    StandUsers = linelist[0].StandUsers
                };
                return new { status = "success", message = "获取数据成功", rows = date };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
        }

        #endregion


        #region   修改单次计数

        //修改单次计数
        [System.Web.Http.HttpPost]
        public dynamic UpdateTallyRatio(dynamic data)
        {
            try
            {
                string InventoryCode = data.InventoryCode;
                string InventoryName = data.InventoryName;
                string TallyRatio = data.TallyRatio;
                string UserCode = data.UserCode;
                string TenantId = data.TenantId;
                string UpdateUser = data.UpdateUser;
                string Person = data.Person;

                //List<dynamic> listUser = ApiDataSource.GetUserList(TenantId, null, UserCode, null).ToObject<List<dynamic>>();
                //if (listUser.Count < 1)
                //    return new { status = "error", message = "找不到此工号"};
                //int i = 0;
                using (var db = Db.Context("MiniMES"))
                {
                    db.Insert("minimes_product_update")
                   .Column("ID", Guid.NewGuid().ToString())
                   .Column("InventoryCode", InventoryCode)
                   .Column("InventoryName", InventoryName)
                   .Column("TallyRatio", TallyRatio)
                   .Column("UserCode", UserCode)
                   .Column("TenantID", TenantId)
                   .Column("UpdateUser", UpdateUser)
                   .Column("UpdateTime", DateTime.Now)
                   .Execute();
                }
                //修改产品单次计数
                //using (var db = Db.Context("MiniMES"))
                //{
                //    i = db.Update("minimes_product")
                //            .Column("TallyRatio", TallyRatio)
                //            .Where("InventoryCode", InventoryCode)
                //            .Execute();
                //}
                return new { status = "success", message = "修改成功" };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
        }


        #endregion


        #region  扫码计数

        //扫码计数
        [System.Web.Http.HttpPost]
        public dynamic UpdateScan(dynamic data)
        {
            try
            {
                string OrderNo = data.OrderNo;
                string LineId = data.LineId;
                string PlanDate = data.Date;
                string Barcode = data.Barcode;
                int Qty = data.Qty;
                string TallyRatio = data.TallyRatio;
                string Person = data.StandUsers;
                string SignID = data.SignID;
                string CreateUser = data.CreateUser;
                DateTime CreateDate = data.CreateDate;
                if (string.IsNullOrEmpty(Person))
                {
                    Person = "1";
                }
                string TenantId = data.TenantId;
                int refQty = 0;
                int refQtyNG = 0;
                string Message = "";
                if (!CheckScan(TenantId, Barcode, ref Message))
                {
                    return new { status = "error", message = Message };
                }

                //获取上次工单数据
                DateTime dateTime = Convert.ToDateTime(PlanDate);
                ParamQuery paramQuery = ParamQuery.Instance()
                    .AndWhere("TenantID", TenantId);
                if (!string.IsNullOrEmpty(LineId))
                    paramQuery = paramQuery.AndWhere("LineId", LineId);
                if (!string.IsNullOrEmpty(OrderNo))
                    paramQuery = paramQuery.AndWhere("OrderNo", OrderNo);
                //if (!string.IsNullOrEmpty(PlanDate))
                //    paramQuery = paramQuery.AndWhere("PlanDate", PlanDate);
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                minimes_order_record row = new minimes_order_record();


                #region 更新工单
                if (list.Count > 0)//已经有数据,修改
                {
                    row = list[0];
                    //修改其他工单状态
                    var param0 = ParamUpdate.Instance()
                        .Column("State", 0)
                         .AndWhere("LineId", LineId);
                    new minimes_order_recordService().Update(param0);

                    //更新产量,修改状态
                    var param = ParamUpdate.Instance()
                      .Column("Qty", row.Qty + Qty)
                       .Column("State", 1)
                       .Column("Ratio", TallyRatio)
                      .AndWhere("ID", row.ID);
                    new minimes_order_recordService().Update(param);
                    //if (result < 1)
                    //    return new { status = "error", message = "保存数据失败" };
                    refQtyNG = row.QtyNG;
                    refQty = row.Qty + Qty;

                }
                else//没有数据，新增
                {
                    //List<dynamic> listWorkSheet = ApiDataSource.GetWorkSheetList(TenantId, dateTime, null, OrderNo, null).ToObject<List<dynamic>>();
                    List<dynamic> listWorkSheet = ApiDataSource.GetWorkSheetList(TenantId, OrderNo, LineId, null, null, null, null, null, null, null).ToObject<List<dynamic>>();
                    //List<dynamic> worksheetlist = listWorkSheet.Where(x => x.LineId == LineId).ToList();
                    if (listWorkSheet.Count < 1)
                        return new { status = "error", message = "获取不到工单数据" };
                    row.OrderNo = OrderNo;
                    row.LineId = LineId;
                    row.PlanDate = Convert.ToDateTime(PlanDate);
                    row.LineName = listWorkSheet[0].LineName + "";
                    List<dynamic> ilinelist = ApiDataSource.GetLineList(TenantId, null, null, LineId, null).ToObject<List<dynamic>>();
                    if (ilinelist != null && ilinelist.Count > 0)
                    {
                        row.LineName = ilinelist[0].LineName;
                    }
                    row.ProductCode = listWorkSheet[0].ProductCode + "";
                    row.ProductName = listWorkSheet[0].ProductName + "";
                    row.Qty = 0;
                    row.QtyNG = 0;
                    row.UPH = 400;
                    row.PlanQty = listWorkSheet[0].PlanCount;
                    row.State = 1;

                    //修改状态
                    var param0 = ParamUpdate.Instance()
                    .Column("State", 0)
                    .AndWhere("LineId", LineId);
                    new minimes_order_recordService().Update(param0);

                    //新增数据
                    var param = ParamInsert.Instance()
                      .Column("OrderNo", row.OrderNo)
                      .Column("LineId", row.LineId)
                      .Column("LineName", row.LineName)
                      .Column("ProductName", row.ProductName)
                      .Column("ProductCode", row.ProductCode)
                      .Column("PlanDate", row.PlanDate)
                      .Column("OrderPlanDate", row.PlanDate)
                      .Column("UPH", row.UPH)
                      .Column("Ratio", TallyRatio)
                      .Column("Qty", Qty)
                      .Column("QtyNG", 0)
                      .Column("PlanQty", row.PlanQty)
                      .Column("State", 1)
                      .Column("MachineState", 1)
                      .Column("StopCount", 0)
                      .Column("StopTime", 0)
                      .Column("Person", Person)
                      .Column("PlanPerson", 1)
                      .Column("CreateDate", CreateDate)
                      .Column("TenantID", TenantId);
                    new minimes_order_recordService().Insert(param);
                    //if (result < 1)
                    //    return new { status = "error", message = "保存数据失败" };
                    refQty = Qty;
                }

                #region  更新工单状态信息

                ApiDataSource.EditWorkSheetStatus(TenantId, OrderNo);

                #endregion

                #region  更新工单数量信息

                ApiDataSource.EditWorkSheetCount(TenantId, OrderNo, refQty);

                #endregion
                #endregion

                #region 更新小时产量
                int Time = CreateDate.Hour;
                //minimes_hoursreport
                List<minimes_hoursreport> recode = new minimes_hoursreportService().GetModelList(ParamQuery.Instance()
                    .AndWhere("TenantID", TenantId)
                    .AndWhere("OrderNo", OrderNo)
                    .AndWhere("PlanDate", PlanDate)
                    .AndWhere("LineId", LineId)
                    .AndWhere("Time", Time)
                    );
                if (recode.Count > 0)//有数据,修改
                {
                    new minimes_hoursreportService().Update(ParamUpdate.Instance()
                      .Column("Qty", recode[0].Qty + Qty)
                      .Column("LastTime", CreateDate)
                      .AndWhere("ID", recode[0].ID)
                      );
                }
                else//没有数据,新增
                {
                    new minimes_hoursreportService().Insert(ParamInsert.Instance()
                   .Column("OrderNo", row.OrderNo)
                   .Column("LineId", row.LineId)
                   .Column("LineName", row.LineName)
                   .Column("ProductName", row.ProductName)
                   .Column("ProductCode", row.ProductCode)
                   .Column("PlanDate", PlanDate)
                   .Column("Time", Time)
                   .Column("Qty", Qty)
                   .Column("QtyNG", 0)
                   .Column("StartTime", CreateDate)
                   .Column("LastTime", CreateDate)
                   .Column("TenantID", TenantId)
                   .Column("DetailedNG", "")
                   .Column("Remark", "")
                   );
                }
                #endregion

                string IP = CommonHelper.GetHostAddress();

                #region 更新扫码记录
                new minimes_plc_recordService().Insert(ParamInsert.Instance()
                .Column("OrderNo", row.OrderNo)
                .Column("LineId", row.LineId)
                .Column("PlanDate", PlanDate)
                .Column("LineName", row.LineName)
                .Column("ProductName", row.ProductName)
                .Column("ProductCode", row.ProductCode)
                .Column("PlcQty", 0)
                .Column("PlcUpperQty", 0)
                .Column("Qty", Qty)
                .Column("CreateUser", CreateUser)
                .Column("CreateDate", CreateDate)
                .Column("PreTime", CreateDate)
                .Column("IntervalTime", 0)
                .Column("TenantID", TenantId)
                .Column("Barcode", Barcode)
                 .Column("SignID", SignID)
                .Column("IP", IP)
                );
                #endregion

                return new { status = "success", message = "保存数据成功", Qty = refQty, QtyNG = refQtyNG };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
        }

        #endregion


        #region  条码验证

        //条码验证
        private bool CheckScan(string TenantId, string Barcode, ref string Message)
        {
            bool result = true;
            List<minimes_parameter> listparameter = new minimes_parameterService().GetModelList(ParamQuery.Instance()
          .AndWhere("TenantID", TenantId)
          .AndWhere("(ParameterCode='BarcodeDigit' or ParameterCode='BarCodeValidate' or ParameterCode='BarcodeInterval') and 1", 1));
            foreach (var row in listparameter)
            {
                switch (row.ParameterCode)
                {
                    case "BarcodeDigit":
                        try
                        {
                            if (Convert.ToInt32(row.ParameterValue) != Barcode.Length)
                            {
                                Message = "条码长度不符合长度";
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            //Message = "条码长度报错。"+ex.Message;
                            //return false;
                        }
                        break;
                    case "BarCodeValidate":
                        try
                        {
                            if (!Barcode.Contains(row.ParameterValue))
                            {
                                Message = "条码不包含验证码";
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            //Message = "条码长度报错。"+ex.Message;
                            //return false;
                        }
                        break;
                    case "BarcodeInterval":
                        try
                        {
                            double time = Convert.ToDouble("-" + row.ParameterValue);
                            List<minimes_plc_record> list = new minimes_plc_recordService().GetModelList(ParamQuery.Instance()
                            .AndWhere("TenantID", TenantId)
                            .AndWhere("Barcode", Barcode)
                            .AndWhere("CreateDate", DateTime.Now.AddMinutes(time), Cp.Greater)
                            //.AndWhere("PlanDate", DateTime.Now.Date)
                            .OrderBy("CreateDate desc")
                            );
                            if (list.Count > 0)
                            {
                                Message = "间隔时间过短。";
                                return false;
                            }
                            //if (list.Count > 0)
                            //{
                            //    DateTime CreateDate = Convert.ToDateTime(list[0].CreateDate);
                            //    if (time > (DateTime.Now - CreateDate).TotalMinutes)
                            //    {
                            //        Message = "间隔时间过短。";
                            //        return false;
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Message = "间隔时间错误。" + ex.Message;
                            return false;
                        }
                        break;
                }
            }
            return result;
        }

        #endregion


        #region   不良计数

        //不良计数
        [System.Web.Http.HttpPost]
        public dynamic UpdateScanNG(dynamic data)
        {
            try
            {
                string OrderNo = data.OrderNo;
                string LineId = data.LineId;
                string PlanDate = data.Date;
                string Barcode = data.Barcode;
                int QtyNG = data.QtyNG;
                string TallyRatio = data.TallyRatio;
                string Person = data.StandUsers;
                string SignID = data.SignID;
                string CreateUser = data.CreateUser;
                DateTime CreateDate = data.CreateDate;
                if (string.IsNullOrEmpty(Person))
                {
                    Person = "1";
                }
                string TenantId = data.TenantId;
                int refQty = 0;
                int refQtyNG = 0;
                //获取上次工单数据
                DateTime dateTime = Convert.ToDateTime(PlanDate);
                ParamQuery paramQuery = ParamQuery.Instance()
                    .AndWhere("TenantID", TenantId);
                if (!string.IsNullOrEmpty(LineId))
                    paramQuery = paramQuery.AndWhere("LineId", LineId);
                if (!string.IsNullOrEmpty(OrderNo))
                    paramQuery = paramQuery.AndWhere("OrderNo", OrderNo);
                //if (!string.IsNullOrEmpty(PlanDate))
                //    paramQuery = paramQuery.AndWhere("PlanDate", PlanDate);
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                minimes_order_record row = new minimes_order_record();

                #region 更新工单
                if (list.Count > 0)//已经有数据,修改
                {
                    row = list[0];
                    //修改状态
                    //var param0 = ParamUpdate.Instance()
                    //    .Column("State", 0)
                    //     .AndWhere("LineId", LineId);
                    //new minimes_order_recordService().Update(param0);

                    //更新不良数量
                    var param = ParamUpdate.Instance()
                      .Column("QtyNG", row.QtyNG + QtyNG)
                       //.Column("State", 1)
                       .Column("Ratio", TallyRatio)
                      .AndWhere("ID", row.ID);
                    new minimes_order_recordService().Update(param);
                    //if (result < 1)
                    //    return new { status = "error", message = "保存数据失败" };
                    refQtyNG = row.QtyNG + QtyNG;
                    refQty = row.Qty;

                }
                else//没有数据，新增
                {
                    //List<dynamic> listWorkSheet = ApiDataSource.GetWorkSheetList(TenantId, dateTime, null, OrderNo, null).ToObject<List<dynamic>>();
                    List<dynamic> listWorkSheet = ApiDataSource.GetWorkSheetList(TenantId, OrderNo, LineId, null, null, null, null, null, null, null).ToObject<List<dynamic>>();
                    //List<dynamic> worksheetlist = listWorkSheet.Where(x => x.LineId == LineId).ToList();
                    if (listWorkSheet.Count < 1)
                        return new { status = "error", message = "获取不到工单数据" };
                    row.OrderNo = OrderNo;
                    row.LineId = LineId;
                    row.PlanDate = Convert.ToDateTime(PlanDate);
                    row.LineName = listWorkSheet[0].LineName + "";
                    List<dynamic> ilinelist = ApiDataSource.GetLineList(TenantId, null, null, LineId, null).ToObject<List<dynamic>>();
                    if (ilinelist != null && ilinelist.Count > 0)
                    {
                        row.LineName = ilinelist[0].LineName;
                    }
                    row.ProductCode = listWorkSheet[0].ProductCode + "";
                    row.ProductName = listWorkSheet[0].ProductName + "";
                    row.Qty = 0;
                    row.QtyNG = 0;
                    row.PlanQty = listWorkSheet[0].PlanCount;
                    row.State = 1;

                    //修改状态
                    //var param0 = ParamUpdate.Instance()
                    //.Column("State", 0)
                    //.AndWhere("LineId", LineId);
                    //new minimes_order_recordService().Update(param0);

                    //新增数据
                    var param = ParamInsert.Instance()
                      .Column("OrderNo", row.OrderNo)
                      .Column("LineId", row.LineId)
                      .Column("LineName", row.LineName)
                      .Column("ProductName", row.ProductName)
                      .Column("ProductCode", row.ProductCode)
                      .Column("PlanDate", row.PlanDate)
                      .Column("OrderPlanDate", row.PlanDate)
                      .Column("UPH", row.UPH)
                      .Column("Ratio", TallyRatio)
                      .Column("Qty", 0)
                      .Column("QtyNG", QtyNG)
                      .Column("PlanQty", row.PlanQty)
                      .Column("State", 0)
                      .Column("MachineState", 1)
                      .Column("StopCount", 0)
                      .Column("StopTime", 0)
                      .Column("Person", Person)
                      .Column("PlanPerson", 1)
                      .Column("CreateDate", CreateDate)
                      .Column("TenantID", TenantId);
                    new minimes_order_recordService().Insert(param);
                    //if (result < 1)
                    //    return new { status = "error", message = "保存数据失败" };
                    refQtyNG = QtyNG;
                }
                #endregion

                #region 更新小时产量
                int Time = CreateDate.Hour;
                //minimes_hoursreport
                List<minimes_hoursreport> recode = new minimes_hoursreportService().GetModelList(ParamQuery.Instance()
                    .AndWhere("TenantID", TenantId)
                    .AndWhere("OrderNo", OrderNo)
                    .AndWhere("PlanDate", PlanDate)
                    .AndWhere("LineId", LineId)
                    .AndWhere("Time", Time)
                    );
                if (recode.Count > 0)//有数据,修改
                {
                    new minimes_hoursreportService().Update(ParamUpdate.Instance()
                      .Column("QtyNG", recode[0].QtyNG + QtyNG)
                      .Column("LastTime", CreateDate)
                      .AndWhere("ID", recode[0].ID)
                      );
                }
                else//没有数据,新增
                {
                    new minimes_hoursreportService().Insert(ParamInsert.Instance()
                   .Column("OrderNo", row.OrderNo)
                   .Column("LineId", row.LineId)
                   .Column("LineName", row.LineName)
                   .Column("ProductName", row.ProductName)
                   .Column("ProductCode", row.ProductCode)
                   .Column("PlanDate", row.PlanDate)
                   .Column("Time", Time)
                   .Column("Qty", 0)
                   .Column("QtyNG", QtyNG)
                   .Column("StartTime", CreateDate)
                   .Column("LastTime", CreateDate)
                   .Column("TenantID", TenantId)
                   .Column("DetailedNG", "")
                   .Column("Remark", "")
                   );
                }
                #endregion

                #region 更新不良记录
                new minimes_plc_record_ngService().Insert(ParamInsert.Instance()
                .Column("OrderNo", row.OrderNo)
                .Column("LineId", row.LineId)
                .Column("PlanDate", row.PlanDate)
                .Column("LineName", row.LineName)
                .Column("ProductName", row.ProductName)
                .Column("ProductCode", row.ProductCode)
                .Column("PlcQty", 0)
                .Column("PlcUpperQty", 0)
                .Column("QtyNG", QtyNG)
                .Column("CreateUser", CreateUser)
                .Column("CreateDate", CreateDate)
                //.Column("PreTime", CreateDate)
                //.Column("IntervalTime", 0)
                .Column("TenantID", TenantId)
                .Column("Barcode", Barcode)
                .Column("SignID", SignID)
                );
                #endregion

                return new { status = "success", message = "保存数据成功", Qty = refQty, QtyNG = refQtyNG };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
        }

        #endregion


        #region   撤回记录

        //撤回记录
        [System.Web.Http.HttpPost]
        public dynamic DeleteScan(dynamic data)
        {
            List<BarcodeModel> result = new List<BarcodeModel>();
            try
            {
                string OrderNo = data.OrderNo;
                string LineId = data.LineId;
                string PlanDate = data.Date;
                string TenantId = data.TenantId;
                dynamic Rows = data.Rows;
                int refQty = 0;
                int refQtyNG = 0;
                //获取上次工单数据
                DateTime dateTime = Convert.ToDateTime(PlanDate);
                ParamQuery paramQuery = ParamQuery.Instance()
                    .AndWhere("TenantID", TenantId);
                if (!string.IsNullOrEmpty(LineId))
                    paramQuery = paramQuery.AndWhere("LineId", LineId);
                if (!string.IsNullOrEmpty(OrderNo))
                    paramQuery = paramQuery.AndWhere("OrderNo", OrderNo);
                //if (!string.IsNullOrEmpty(PlanDate))
                //    paramQuery = paramQuery.AndWhere("PlanDate", PlanDate);
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                minimes_order_record row = new minimes_order_record();

                #region 更新工单
                if (list.Count > 0)//已经有数据,修改
                {
                    row = list[0];
                    int totalQty = 0;
                    foreach (var item in Rows)
                    {
                        string Barcode = item.Barcode;
                        int Qty = item.Qty;
                        string SignID = item.SignID;
                        DateTime CreateDate = item.CreateDate;
                        totalQty += Qty;
                        int Time = CreateDate.Hour;
                        //更新不良数量
                        var param = ParamUpdate.Instance()
                          .Column("Qty", row.Qty - totalQty)
                          .AndWhere("ID", row.ID);
                        new minimes_order_recordService().Update(param);
                        //更新小时产量
                        List<minimes_hoursreport> recode = new minimes_hoursreportService().GetModelList(ParamQuery.Instance()
                        .AndWhere("TenantID", TenantId)
                        .AndWhere("OrderNo", OrderNo)
                        .AndWhere("PlanDate", PlanDate)
                        .AndWhere("LineId", LineId)
                        .AndWhere("Time", Time)
                          );
                        if (recode.Count > 0)
                        {
                            new minimes_hoursreportService().Update(ParamUpdate.Instance()
                      .Column("Qty", recode[0].Qty - Qty)
                      .Column("LastTime", CreateDate)
                      .AndWhere("ID", recode[0].ID)
                      );
                        }
                        //删除记录
                        new minimes_plc_recordService().Delete(ParamDelete.Instance()
                            .AndWhere("SignID", SignID)
                            );
                        //加入回滚记录
                        new minimes_plc_record_recallService().Insert(ParamInsert.Instance()
                       .Column("Qty", Qty)
                      .Column("OrderNo", row.OrderNo)
                      .Column("PlanDate", row.PlanDate)
                      .Column("LineId", row.LineId)
                      .Column("LineName", row.LineName)
                      .Column("CreateDate", CreateDate)
                      .Column("TenantID", TenantId)
                      .Column("SignID", SignID)
                            );
                        result.Add(new BarcodeModel
                        {
                            Barcode = item.Barcode,
                            Qty = item.Qty,
                            SignID = item.SignID,
                            CreateDate = item.CreateDate
                        });

                    }

                    refQty = row.Qty - totalQty;
                    refQtyNG = row.QtyNG;
                }
                else//没有数据，新增
                {
                    return new { status = "error", message = "工单数据错误", Qty = 0, Qty0NG = 0, rows = result };
                }
                #endregion

                return new { status = "success", message = "保存数据成功", Qty = refQty, QtyNG = refQtyNG, rows = result };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message, Qty = 0, Qty0NG = 0, rows = result };
            }
        }

        #endregion


        #region  获取历史数据

        //获取历史数据
        public dynamic GetScanHistroy(string LineId, string OrderNo, string Date, string TenantId)
        {
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(Date);
                ParamQuery paramQuery = ParamQuery.Instance()
                    .AndWhere("TenantID", TenantId);
                if (!string.IsNullOrEmpty(LineId))
                    paramQuery = paramQuery.AndWhere("LineId", LineId);
                if (!string.IsNullOrEmpty(OrderNo))
                    paramQuery = paramQuery.AndWhere("OrderNo", OrderNo);
                if (!string.IsNullOrEmpty(Date))
                    paramQuery = paramQuery.AndWhere("PlanDate", Date);
                paramQuery.OrderBy("CreateDate desc");
                List<minimes_plc_record> result = new minimes_plc_recordService().GetModelList(paramQuery);
                return new { status = "success", message = "获取数据成功", rows = result };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
        }

        #endregion


        #region   获取历史不良

        //获取历史不良
        public dynamic GetScanHistroyNG(string LineId, string OrderNo, string Date, string TenantId)
        {
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(Date);
                ParamQuery paramQuery = ParamQuery.Instance()
                    .AndWhere("TenantID", TenantId);
                if (!string.IsNullOrEmpty(LineId))
                    paramQuery = paramQuery.AndWhere("LineId", LineId);
                if (!string.IsNullOrEmpty(OrderNo))
                    paramQuery = paramQuery.AndWhere("OrderNo", OrderNo);
                if (!string.IsNullOrEmpty(Date))
                    paramQuery = paramQuery.AndWhere("PlanDate", Date);
                paramQuery.OrderBy("CreateDate desc");
                List<minimes_plc_record_ng> result = new minimes_plc_record_ngService().GetModelList(paramQuery);
                return new { status = "success", message = "获取数据成功", rows = result };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
        }

        #endregion


        #region    获取正在生产产线信息(当天)

        //获取正在生产产线信息(当天)
        [System.Web.Http.HttpPost]
        public dynamic GetLineProducing(dynamic data)
        {
            List<dynamic> result = new List<dynamic>();
            try
            {
                string TenantId = data.TenantId;
                dynamic Rows = data.Rows;
                ParamQuery paramQuery = ParamQuery.Instance()
                     .AndWhere("TenantID", TenantId);
                string Query = "(1=2";
                foreach (var row in Rows)
                {
                    Query += " or LineId='" + row.LineId + "'";
                }
                Query += ")";
                paramQuery = paramQuery.AndWhere("" + Query + " and 1", 1);
                paramQuery = paramQuery.AndWhere("State", 1);
                //paramQuery = paramQuery.AndWhere("PlanDate", DateTime.Now.Date);
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                foreach (var row in list)
                {
                    dynamic item = new
                    {
                        OrderNo = row.OrderNo,
                        LineId = row.LineId,
                        LineName = row.LineName,
                        PlanQty = row.PlanQty,
                        Qty = row.Qty,
                        QtyNG = row.QtyNG,
                        CreateDate = row.CreateDate,
                        State = row.State
                    };
                    result.Add(item);
                }
                return new { status = "success", message = "获取数据成功", rows = result };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message, rows = result };
            }
        }

        #endregion


        #region   获取产线日信息


        //获取产线日信息
        [System.Web.Http.HttpPost]
        public dynamic GetDateProducing(dynamic data)
        {
            List<dynamic> result = new List<dynamic>();
            try
            {
                string TenantId = data.TenantId;
                string Date = data.Date;
                ParamQuery paramQuery = ParamQuery.Instance()
                     .AndWhere("TenantID", TenantId);
                //paramQuery = paramQuery.AndWhere("PlanDate", Date);
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                foreach (var row in list)
                {
                    dynamic item = new
                    {
                        OrderNo = row.OrderNo,
                        LineId = row.LineId,
                        LineName = row.LineName,
                        PlanQty = row.PlanQty,
                        Qty = row.Qty,
                        QtyNG = row.QtyNG,
                        CreateDate = row.CreateDate,
                        State = row.State
                    };
                    result.Add(item);
                }
                return new { status = "success", message = "获取数据成功", rows = result };
            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message, rows = result };
            }
        }

        #endregion


        #region   测试接口

        //测试接口
        public dynamic GetTest(string a, int b, bool c, DateTime d)
        {
            string aa = a;
            int bb = b;
            bool cc = c;
            DateTime dd = d;
            return true;
        }
        #endregion


        #region  PLC采集数据

        //PLC采集数据
        [System.Web.Http.HttpPost]
        public dynamic GetPLCData(dynamic data)
        {
            string TenantId = data.TenantId;
            string OrderNo = data.OrderNo;
            int PlcQty = data.PlcQty;
            int PlcUpperQty = data.PlcUpperQty;
            int Qty = data.Qty;
            string LineId = data.LineId;
            int State = data.State;
            bool PLCSwitch = data.PLCSwitch;
            bool StateSwtich = data.StateSwtich;
            bool MinuteSwitch = data.MinuteSwitch;
            DateTime PreTime = data.PreTime;
            DateTime NowTime = data.NowTime;
            string IP = data.IP + "";
            try
            {
                ParamQuery paramQuery = ParamQuery.Instance()
                          .AndWhere("OrderNo", OrderNo)
                           //.AndWhere("State", 1)
                           //.AndWhere("PlanDate", NowTime.Date)
                           .AndWhere("LineId", LineId)
                          .AndWhere("TenantID", TenantId)
                          .OrderBy("CreateDate desc");
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                if (list.Count < 1)
                    return new { status = "error", message = "找不到该工单" };
                Qty = list[0].Ratio * Qty;

                minimes_ordertimeService ordertimeService = new minimes_ordertimeService();
                List<minimes_ordertime> ordertimeList = ordertimeService.GetModelList(ParamQuery.Instance().AndWhere("LineId", LineId).AndWhere("OrderNo", OrderNo).AndWhere("TenantID", TenantId).OrderBy(" CreateDate Desc"));
                if (ordertimeList.Count == 0 || (ordertimeList.Count > 0 && ordertimeList[0].EndTime != null))
                {
                    if (TenantId == "865438a5-6599-47af-ac6e-095bfba8c5f6")//威特晟部分产线不用更新工单
                    {
                        if (list[0].LineName.Contains("插件") || list[0].LineName.Contains("补焊") || list[0].LineName.Contains("装配"))
                        { }
                        else
                        {
                            #region  更新工单状态信息

                            //ApiDataSource.EditWorkSheetStatus(TenantId, OrderNo);

                            Action<string, string> setworksheetstatus = ApiDataSource.EditWorkSheetStatus;
                            setworksheetstatus.BeginInvoke(TenantId, OrderNo, null, null);

                            #endregion


                            #region  更新工单数量信息

                            //ApiDataSource.EditWorkSheetCount(TenantId, OrderNo, list[0].Qty + Qty);

                            Action<string, string, int> setworksheetcount = ApiDataSource.EditWorkSheetCount;
                            setworksheetcount.BeginInvoke(TenantId, OrderNo, list[0].Qty + Qty, null, null);

                            #endregion
                        }
                    }
                    else
                    {
                        #region  更新工单状态信息

                        //ApiDataSource.EditWorkSheetStatus(TenantId, OrderNo);

                        Action<string, string> setworksheetstatus = ApiDataSource.EditWorkSheetStatus;
                        setworksheetstatus.BeginInvoke(TenantId, OrderNo, null, null);

                        #endregion


                        #region  更新工单数量信息

                        //ApiDataSource.EditWorkSheetCount(TenantId, OrderNo, list[0].Qty + Qty);

                        Action<string, string, int> setworksheetcount = ApiDataSource.EditWorkSheetCount;
                        setworksheetcount.BeginInvoke(TenantId, OrderNo, list[0].Qty + Qty, null, null);

                        #endregion
                    }

                    #region PLC数量改变

                    if (PLCSwitch)
                    {
                        #region 更新工单

                        //更新产量,修改状态
                        var param = ParamUpdate.Instance()
                          .Column("Qty", list[0].Qty + Qty)
                          .Column("MachineState", State)
                          .AndWhere("ID", list[0].ID);
                        int i = new minimes_order_recordService().Update(param);

                        #endregion

                        #region 更新小时产量

                        int Time = NowTime.Hour;
                        //minimes_hoursreport
                        List<minimes_hoursreport> recode = new minimes_hoursreportService().GetModelList(ParamQuery.Instance()
                            .AndWhere("TenantID", TenantId)
                            .AndWhere("OrderNo", OrderNo)
                            .AndWhere("PlanDate", NowTime.Date)
                            .AndWhere("LineId", LineId)
                            .AndWhere("Time", Time)
                            );
                        if (recode.Count > 0)//有数据,修改
                        {
                            new minimes_hoursreportService().Update(ParamUpdate.Instance()
                              .Column("Qty", recode[0].Qty + Qty)
                              .Column("LastTime", NowTime)
                              .AndWhere("ID", recode[0].ID)
                              );
                        }
                        else//没有数据,新增
                        {
                            new minimes_hoursreportService().Insert(ParamInsert.Instance()
                           .Column("OrderNo", list[0].OrderNo)
                           .Column("WorkShopId", list[0].WorkShopId)//车间ID
                           .Column("WorkShopName", list[0].WorkShopName)//车间名称
                           .Column("LineId", list[0].LineId)
                           .Column("LineName", list[0].LineName)
                           .Column("ProductName", list[0].ProductName)
                           .Column("ProductCode", list[0].ProductCode)
                           .Column("PlanDate", NowTime.Date)
                           .Column("Time", Time)
                           .Column("Qty", Qty)
                           .Column("QtyNG", 0)
                           .Column("StartTime", NowTime)
                           .Column("LastTime", NowTime)
                           .Column("TenantID", TenantId)
                           .Column("DetailedNG", "")
                           .Column("Remark", "")
                           );
                        }

                        #endregion

                        #region 更新数据记录表

                        //string IP = CommonHelper.GetHostAddress();
                        new minimes_plc_recordService().Insert(ParamInsert.Instance()
                        .Column("OrderNo", list[0].OrderNo)
                        .Column("WorkShopId", list[0].WorkShopId)//车间ID
                        .Column("WorkShopName", list[0].WorkShopName)//车间名称
                        .Column("LineId", list[0].LineId)
                        .Column("PlanDate", list[0].PlanDate)
                        .Column("LineName", list[0].LineName)
                        .Column("ProductName", list[0].ProductName)
                        .Column("ProductCode", list[0].ProductCode)
                        .Column("PlcQty", PlcQty)
                        .Column("PlcUpperQty", PlcUpperQty)
                        .Column("Qty", Qty)
                        .Column("CreateUser", "PLC")
                        .Column("CreateDate", NowTime)
                        .Column("PreTime", PreTime)
                        .Column("IntervalTime", (NowTime - PreTime).TotalSeconds)
                        .Column("TenantID", TenantId)
                        .Column("Barcode", "PLC")
                        .Column("SignID", Guid.NewGuid())
                        .Column("IP", IP)
                        );

                        #endregion

                        //if(i>0)
                        //    return new { status = "success", message = "保存数据成功"};
                        //else
                        //    return new { status = "error", message = "保存数据失败" };
                    }

                    #endregion

                    #region PLC状态改变
                    if (StateSwtich)
                    {
                        new minimes_status_recordService().Update(ParamUpdate.Instance()
                      .Column("EndTime", NowTime)
                      .AndWhere("TenantID", TenantId)
                      .AndWhere("LineId", LineId)
                      .AndWhere("(EndTime is null) and 1", 1)
                      );
                        new minimes_status_recordService().Insert(ParamInsert.Instance()
                        .Column("WorkShopId", list[0].WorkShopId)//车间ID
                        .Column("WorkShopName", list[0].WorkShopName)//车间名称
                        .Column("LineId", LineId)
                        .Column("LineName", list[0].LineName)
                        .Column("PlanDate", NowTime.Date)
                        .Column("State", State)
                        .Column("StartTime", NowTime)
                        .Column("TenantId", TenantId)
                        // .Column("IP", IP)//精恒机械专属
                        );

                        if (State == 0)
                        {
                            new minimes_order_recordService().Update(ParamUpdate.Instance()
                           .Column("MachineState", State)
                           .Column("StopCount", list[0].StopCount + 1)
                           .AndWhere("ID", list[0].ID)
                             );
                        }
                        else
                        {
                            new minimes_order_recordService().Update(ParamUpdate.Instance()
                            .Column("MachineState", State)
                            .AndWhere("ID", list[0].ID)
                             );
                        }

                    }
                    #endregion

                    #region 停机时间增加
                    if (State == 0 && MinuteSwitch)
                    {
                        if (list[0].StopCount == 0)
                        {
                            new minimes_order_recordService().Update(ParamUpdate.Instance()
                            .Column("MachineState", State)
                            .Column("StopTime", list[0].StopTime + 1)
                            .Column("StopCount", 1)
                            .AndWhere("ID", list[0].ID)
                             );
                        }
                        else
                        {
                            new minimes_order_recordService().Update(ParamUpdate.Instance()
                            .Column("MachineState", State)
                            .Column("StopTime", list[0].StopTime + 1)
                            .AndWhere("ID", list[0].ID)
                             );
                        }
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
            return new { status = "success", message = "保存成功" };
        }


        //PLC采集数据
        [System.Web.Http.HttpPost]
        public dynamic GetWeightData(dynamic data)
        {
            try
            {
                string TenantId = data.TenantId;
                string OrderNo = data.OrderNo;
                double Weight = data.Weight;
                string LineId = data.LineId;
                DateTime NowTime = data.NowTime;
                string IP = data.IP + "";

                ParamQuery paramQuery = ParamQuery.Instance()
                          .AndWhere("OrderNo", OrderNo)
                           //.AndWhere("State", 1)
                           //.AndWhere("PlanDate", NowTime.Date)
                           .AndWhere("LineId", LineId)
                          .AndWhere("TenantID", TenantId)
                          .OrderBy("CreateDate desc");
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                if (list.Count < 1)
                    return new { status = "error", message = "找不到该工单" };

                //更新产量,修改状态
                var param = ParamUpdate.Instance()
                  .Column("Weight", list[0].Weight + Weight)
                  .AndWhere("ID", list[0].ID);
                int i = new minimes_order_recordService().Update(param);

                new minimes_weight_recordService().Insert(ParamInsert.Instance()
               .Column("OrderNo", list[0].OrderNo)
               .Column("LineId", list[0].LineId)
                .Column("Weight", Weight)
               .Column("PlanDate", list[0].PlanDate)
               .Column("LineName", list[0].LineName)
               .Column("ProductName", list[0].ProductName)
               .Column("ProductCode", list[0].ProductCode)
               .Column("CreateDate", NowTime)
               .Column("TenantID", TenantId)
               .Column("IP", IP)
               );

            }
            catch (Exception ex)
            {
                return new { status = "error", message = ex.Message };
            }
            return new { status = "success", message = "保存成功" };
        }
        #endregion


        #region RPC APP接口

        //11111根据线体ID和车间获取工单号
        [System.Web.Http.HttpPost]
        public dynamic GetRPCOrderNoData(dynamic data)
        {
            //PrintLog(string.Format("1111-GetRPCOrderNoData接口获取数据{0}", data));
            List<dynamic> result = new List<dynamic>();
            try
            {
                string TenantId = data.TenantId;
                string LineId = data.LineId;
                //string LineName = data.LineName;

                ParamQuery paramQuery = ParamQuery.Instance()
                          .AndWhere("TenantID", TenantId)
                          .AndWhere("LineId", LineId)
                         //  .AndWhere("LineName", LineName)
                         .OrderBy("CreateDate desc");
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                if (list.Count < 1)
                    return new { status = "error", message = "未找到对应工单号", rows = result };

                foreach (var row in list)
                {
                    dynamic item = new
                    {
                        OrderNo = row.OrderNo,
                        CreateDate = row.CreateDate,
                        State = row.State
                    };
                    result.Add(item);
                }
                return new { status = "success", message = "获取数据成功", rows = result };
            }
            catch (Exception ex)
            {
                return new { statues = "error", message = ex.Message };
            }


        }

        //2222根据工单号码获取具体产品信息
        [System.Web.Http.HttpPost]
        public dynamic GetRPCOrderDetailData(dynamic data)
        {
            //PrintLog(string.Format("2222-GetRPCOrderDetailData接口获取数据{0}", data));
            List<dynamic> result = new List<dynamic>();
            try
            {
                string TenantId = data.TenantId;
                string LineId = data.LineId;
                string OrderNo = data.OrderNo;

                ParamQuery paramQuery = ParamQuery.Instance()
                          .AndWhere("TenantID", TenantId)
                          .AndWhere("LineId", LineId)
                       .AndWhere("OrderNo", OrderNo)
                         .OrderBy("CreateDate desc");
                List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                if (list.Count < 1)
                    return new { status = "error", message = "找不到该工单信息", rows = result };

                foreach (var row in list)
                {
                    dynamic item = new
                    {
                        State = row.MachineState,
                        ProductCode = row.ProductCode,
                        ProductName = row.ProductName,
                        LineName = row.LineName,
                        PlanQty = row.PlanQty,

                        PlanStartDate = row.PlanStartDate,//计划开工时机
                        RealStartDate = row.RealStartDate, //实际开工时机
                        PlanEndDate = row.PlanEndDate,
                        RealEndDate = row.RealEndDate,
                        RecoverDate = row.RecoverDate,

                        RealStopDate = row.RealStopDate,

                        StopTime = row.StopTime,//停工时间
                        OrderNo = row.OrderNo,
                        CreateDate = row.CreateDate,

                        Ratio = row.Ratio,//单次计数拼版数
                        UPH = row.UPH,
                        Qty = row.Qty,//产量
                        QtyNG = row.QtyNG   //不良数量

                    };
                    result.Add(item);
                }
                return new { status = "success", message = "获取数据成功", rows = result };

                #region  更新工单状态信息

                // ApiDataSource.EditWorkSheetStatus(TenantId, OrderNo);

                #endregion
            }
            catch (Exception ex)
            {

                return new { statues = "error", message = ex.Message };
            }


        }


        private DateTime GetDate(dynamic datas)
        {
            DateTime dt = DateTime.Now;
            try
            {
                if (!string.IsNullOrEmpty(datas.ToString()))
                {
                    dt = DateTime.Parse(datas.ToString());
                }
            }
            catch (Exception)
            {
                dt = DateTime.Now;

            }

            return dt;
        }

        //3更改工单号状态 0待工1生成2停机3完工
        [System.Web.Http.HttpPost]
        public dynamic EditRPCOrderStateData(dynamic data)
        {
            //PrintLog(string.Format("3333-EditRPCOrderStateData接口获取数据{0}", data));
            List<dynamic> result = new List<dynamic>();
            try
            {
                //更新字段，状态，时间
                string OrderNo = data.OrderNo;
                int LastState = data.LastState;
                int State = data.State;
                string TenantId = data.TenantId;
                string LineId = data.LineId;
                string UserId = data.UserId;

                DateTime PlanStartDate = GetDate(data.PlanStartDate);//计划开工时机
                DateTime RealStartDate = GetDate(data.RealStartDate); //实际开工时机
                DateTime PlanEndDate = GetDate(data.PlanEndDate);
                //实际结束时间
                DateTime RealEndDate = GetDate(data.RealEndDate);
                DateTime RecoverDate = GetDate(data.RecoverDate);
                //实际停机时间
                DateTime RealStopDate = GetDate(data.RealStopDate);

                DateTime CreateNGDate = DateTime.Now;
                // int StopTime = data.StopTime;//停工时间分钟
                int StopTime = 0;//停工时间分钟
                //目前只考虑计算停工-生成的时间间隔

                if (State == 1)
                {
                    //增加一个判断，已经存在此线体开工的状态，不能再次开工
                    ParamQuery paramOnly = ParamQuery.Instance()
                         .AndWhere("TenantID", TenantId)
                          .AndWhere("MachineState", 1)
                         .AndWhere("LineId", LineId);
                    List<minimes_order_record> listOn = new minimes_order_recordService().GetModelList(paramOnly);
                    if (listOn.Count > 0)
                    {
                        return new { status = "error", message = "此线体有工单处于开工状态,请等待..." };
                    }
                    //
                    if (LastState == 2)
                    {
                        ParamQuery paramQuery = ParamQuery.Instance()
                          .AndWhere("TenantID", TenantId)
                          .AndWhere("LineId", LineId)
                          .AndWhere("OrderNo", OrderNo)
                          .AndWhere("MachineState", 2);
                        List<minimes_order_record> list = new minimes_order_recordService().GetModelList(paramQuery);
                        if (list.Count > 0) //上次记录存在则需要更改状态，计算停机时间，其他的不用
                        {
                            minimes_order_record mod = list[0];
                            TimeSpan tsp = RealStartDate - mod.RealStopDate;
                            StopTime = (int)Math.Round(tsp.TotalMinutes, 0);
                            int aa = new minimes_order_recordService().Update(ParamUpdate.Instance()
                                      .Column("MachineState", State)
                                      .Column("RecoverDate", RecoverDate)
                                      .Column("StopTime", StopTime)
                                      .AndWhere("OrderNo", OrderNo)
                                      .AndWhere("TenantId", TenantId)
                                      .AndWhere("LineId", LineId)
                                      );
                            //PrintLog(string.Format("修改返回值aa1:{0},", aa));
                        }
                        else
                        {
                            int aa2 = new minimes_order_recordService().Update(ParamUpdate.Instance()
                                    .Column("MachineState", State)
                                    .Column("RealStartDate", RealStartDate)
                                    .AndWhere("OrderNo", OrderNo)
                                    .AndWhere("TenantId", TenantId)
                                    .AndWhere("LineId", LineId)
                                    );
                            //PrintLog(string.Format("修改返回值aa2:{0},", aa2));
                        }
                    }
                    else
                    {
                        int aa3 = new minimes_order_recordService().Update(ParamUpdate.Instance()
                                    .Column("MachineState", State)
                                    .Column("RealStartDate", RealStartDate)
                                    .AndWhere("OrderNo", OrderNo)
                                    .AndWhere("TenantId", TenantId)
                                    .AndWhere("LineId", LineId)
                                    );
                        //PrintLog(string.Format("修改返回值aa2:{0},", aa3));
                    }
                }
                else if (State == 2)
                {
                    int aa = new minimes_order_recordService().Update(ParamUpdate.Instance()
                                   .Column("MachineState", State)
                                   .Column("RealStopDate", RealStopDate)
                                   .AndWhere("OrderNo", OrderNo)
                                   .AndWhere("TenantId", TenantId)
                                   .AndWhere("LineId", LineId)
                                   );
                    //PrintLog(string.Format("state=2 修改返回值aa2:{0},", aa));
                }
                else if (State == 3)
                {
                    int aa = new minimes_order_recordService().Update(ParamUpdate.Instance()
                                  .Column("MachineState", State)
                                  .Column("RealEndDate", RealEndDate)
                                  .AndWhere("OrderNo", OrderNo)
                                  .AndWhere("TenantId", TenantId)
                                  .AndWhere("LineId", LineId)
                                  );
                    //PrintLog(string.Format("state=3 修改返回值aa2:{0},", aa));
                }
                int aa5 = new minimes_order_record_stateService().Insert(ParamInsert.Instance()
                                  .Column("OrderNo", OrderNo)
                                  .Column("LastState", LastState)
                                  .Column("NowState", State)
                                  .Column("CreateDate", CreateNGDate)
                                  .Column("LineId", LineId)

                                  .Column("TenantId", TenantId)
                                   .Column("UserId", UserId)
                                  );
                //PrintLog(string.Format("插入状态返回值aa2:{0},", aa5));
                ParamQuery NparamQuery = ParamQuery.Instance()
                        .AndWhere("TenantID", TenantId)
                        .AndWhere("LineId", LineId)
                     .AndWhere("OrderNo", OrderNo)
                       .OrderBy("CreateDate desc");
                List<minimes_order_record> Nlist = new minimes_order_recordService().GetModelList(NparamQuery);
                if (Nlist.Count < 1)
                    return new { status = "error", message = "找不到该工单信息", rows = result };

                foreach (var row in Nlist)
                {
                    dynamic item = new
                    {
                        State = row.MachineState,
                        ProductCode = row.ProductCode,
                        ProductName = row.ProductName,
                        LineName = row.LineName,
                        PlanQty = row.PlanQty,

                        PlanStartDate = row.PlanStartDate,//计划开工时机
                        RealStartDate = row.RealStartDate, //实际开工时机
                        PlanEndDate = row.PlanEndDate,

                        RealEndDate = row.RealEndDate,
                        RecoverDate = row.RecoverDate,
                        RealStopDate = row.RealStopDate,

                        StopTime = row.StopTime,//停工时间
                        OrderNo = row.OrderNo,
                        CreateDate = row.CreateDate,

                        Ratio = row.Ratio,//单次计数拼版数
                        UPH = row.UPH,
                        Qty = row.Qty,//产量
                        QtyNG = row.QtyNG   //不良数量



                    };
                    result.Add(item);
                }

            }
            catch (Exception ex)
            {
                return new { statues = "error", message = ex.Message };
            }
            return new { status = "success", message = "保存状态成功", rows = result };
        }


        //5将工单号详细信息更新到表中
        [System.Web.Http.HttpPost]
        public dynamic PostRPCOrderDetailData(dynamic data)
        {
            ParamUpdate ppd = ParamUpdate.Instance();
            //PrintLog(string.Format("5555-PostRPCOrderDetailData接口获取数据{0}", data));
            List<dynamic> result = new List<dynamic>();
            try
            {
                //更新字段，单次计数和uph
                string OrderNo = data.OrderNo;
                int UPH = data.UPH;
                int Ratio = data.Ratio;
                string TenantId = data.TenantId;
                string LineId = data.LineId;
                if (Ratio > 0)
                {
                    ppd.Column("Ratio", Ratio);
                }
                if (UPH > 0)
                {
                    ppd.Column("UPH", UPH);
                }
                ppd.AndWhere("OrderNo", OrderNo)
                   .AndWhere("LineId", LineId)
                   .AndWhere("TenantId", TenantId);
                int aa = new minimes_order_recordService().Update(ppd);
                //PrintLog(string.Format("修改返回值aa1:{0},ppd:{1}", aa,ppd));

                #region 刷新数据给客户端
                ParamQuery NparamQuery = ParamQuery.Instance()
                       .AndWhere("TenantID", TenantId)
                       .AndWhere("LineId", LineId)
                       .AndWhere("OrderNo", OrderNo)
                       .OrderBy("CreateDate desc");
                List<minimes_order_record> Nlist = new minimes_order_recordService().GetModelList(NparamQuery);
                if (Nlist.Count < 1)
                    return new { status = "error", message = "找不到该工单信息", rows = result };

                foreach (var row in Nlist)
                {
                    dynamic item = new
                    {
                        State = row.MachineState,
                        ProductCode = row.ProductCode,
                        ProductName = row.ProductName,
                        LineName = row.LineName,
                        PlanQty = row.PlanQty,

                        PlanStartDate = row.PlanStartDate,//计划开工时机
                        RealStartDate = row.RealStartDate, //实际开工时机
                        PlanEndDate = row.PlanEndDate,

                        RealEndDate = row.RealEndDate,
                        RecoverDate = row.RecoverDate,
                        RealStopDate = row.RealStopDate,

                        StopTime = row.StopTime,//停工时间
                        OrderNo = row.OrderNo,
                        CreateDate = row.CreateDate,

                        Ratio = row.Ratio,//单次计数拼版数
                        UPH = row.UPH,
                        Qty = row.Qty,//产量
                        QtyNG = row.QtyNG   //不良数量

                    };
                    result.Add(item);
                }
                #endregion

            }
            catch (Exception ex)
            {
                return new { statues = "error", message = ex.Message };
            }
            return new { status = "success", message = "保存成功", rows = result };
        }
        //4更改工单号不良产量和总产量
        [System.Web.Http.HttpPost]
        public dynamic EditRPCOrderQtyData(dynamic data)
        {
            List<dynamic> result = new List<dynamic>();
            try
            {
                //PrintLog(string.Format("4444-EditRPCOrderQtyData接口获取数据{0}", data));
                //更新字段，状态，时间
                string OrderNo = data.OrderNo;
                int LastNGQty = data.LastNGQty;
                int ChangeNGQty = data.ChangeNGQty;
                //上次总数量
                int LastTotalQty = data.LastTotalQty;
                int ChangeTotalQty = data.ChangeTotalQty;

                string TenantId = data.TenantId;
                string LineId = data.LineId;
                string UserId = data.UserId;
                DateTime CreateDate = data.CreateDate;
                //计算出来的总数量
                int NowTotalQty = LastTotalQty + ChangeTotalQty;
                int NowNGQty = LastNGQty + ChangeNGQty;

                ParamUpdate ppd = ParamUpdate.Instance();
                if (ChangeTotalQty > 0)
                {
                    ppd.Column("Qty", NowTotalQty);
                }
                if (ChangeNGQty > 0)
                {
                    ppd.Column("QtyNG", NowNGQty);
                }
                ppd.AndWhere("OrderNo", OrderNo)
                   .AndWhere("TenantId", TenantId)
                   .AndWhere("LineId", LineId);

                int aa = new minimes_order_recordService().Update(ppd);

                int aa2 = new minimes_order_record_qtyService().Insert(ParamInsert.Instance()
                              .Column("OrderNo", OrderNo)
                              .Column("LastNGQty", LastNGQty)
                              .Column("ChangeNGQty", ChangeNGQty)
                              .Column("NowNGQty", NowNGQty)
                              .Column("LastTotalQty", LastTotalQty)

                              .Column("ChangeTotalQty", ChangeTotalQty)
                              .Column("NowTotalQty", NowTotalQty)
                              .Column("CreateDate", CreateDate)
                              .Column("LineId", LineId)
                              .Column("TenantId", TenantId)

                              .Column("UserId", UserId)
                              );
                //PrintLog(string.Format("修改返回值aa1:{0},添加返回值aa2:{1}", aa, aa2));

                #region 刷新数据给客户端
                ParamQuery NparamQuery = ParamQuery.Instance()
                       .AndWhere("TenantID", TenantId)
                       .AndWhere("LineId", LineId)
                       .AndWhere("OrderNo", OrderNo)
                       .OrderBy("CreateDate desc");
                List<minimes_order_record> Nlist = new minimes_order_recordService().GetModelList(NparamQuery);
                if (Nlist.Count < 1)
                    return new { status = "error", message = "找不到该工单信息", rows = result };

                foreach (var row in Nlist)
                {
                    dynamic item = new
                    {
                        State = row.MachineState,
                        ProductCode = row.ProductCode,
                        ProductName = row.ProductName,
                        LineName = row.LineName,
                        PlanQty = row.PlanQty,

                        PlanStartDate = row.PlanStartDate,//计划开工时机
                        RealStartDate = row.RealStartDate, //实际开工时机
                        PlanEndDate = row.PlanEndDate,

                        RealEndDate = row.RealEndDate,
                        RecoverDate = row.RecoverDate,
                        RealStopDate = row.RealStopDate,

                        StopTime = row.StopTime,//停工时间
                        OrderNo = row.OrderNo,
                        CreateDate = row.CreateDate,

                        Ratio = row.Ratio,//单次计数拼版数
                        UPH = row.UPH,
                        Qty = row.Qty,//产量
                        QtyNG = row.QtyNG   //不良数量

                    };
                    result.Add(item);
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { statues = "error", message = ex.Message };
            }
            return new { status = "success", message = "保存状态成功", rows = result };
        }
        private string GetStr(dynamic data)
        {
            string stmp = "";
            try
            {
                if (data == null)
                {
                    return "";
                }
                stmp = data.ToString();
            }
            catch (Exception ex)
            {

                stmp = "";
            }
            return stmp;
        }
        //6获取生产看板数据
        [System.Web.Http.HttpPost]
        public dynamic GetRPCProductBoard(dynamic data)
        {
            List<dynamic> result = new List<dynamic>();
            try
            {
                //PrintLog(string.Format("6666-GetRPCProductBoard接口获取数据{0}", data));

                string OrderNo = data.OrderNo;
                string TenantId = data.TenantId;
                string LineId = data.LineId;
                string UserId = data.UserId;

                int Ratio = 0;
                int UPH = 0;

                ParamQuery NparamQuery = ParamQuery.Instance()
                     .AndWhere("TenantID", TenantId)
                     .AndWhere("LineId", LineId)
                     .AndWhere("OrderNo", OrderNo);
                if (!string.IsNullOrEmpty(GetStr(data.Ratio)))
                {
                    NparamQuery.AndWhere("Ratio", Ratio);
                    Ratio = data.Ratio;
                }
                if (!string.IsNullOrEmpty(GetStr(data.UPH)))
                {
                    NparamQuery.AndWhere("UPH", UPH);
                    UPH = data.UPH;
                }
                List<minimes_order_record> Nlist = new minimes_order_recordService().GetModelList(NparamQuery);
                if (Nlist.Count < 1)
                    return new { status = "error", message = "找不到该工单信息", rows = result };
                #region 看板资料


                var row = Nlist[0];

                Ratio = Ratio < 1 ? row.Ratio : Ratio;
                UPH = UPH < 1 ? row.UPH : UPH;
                #region 计算数据
                int NowState = row.MachineState;

                int StopTime = row.StopTime;
                #region MyRegion
                //if (NowState==2)
                //{
                //    TimeSpan tsp = row.RealStopDate - row.RealStartDate;
                //    mProduceTime = (int)Math.Round(tsp.TotalMinutes, 0);
                //}
                //else
                //{
                //    TimeSpan tsp = row.RealEndDate - row.RealStartDate;
                //    mProduceTime = (int)Math.Round(tsp.TotalMinutes - StopTime, 0);
                //}
                #endregion


                //计算生产达成率
                double Utilization = Math.Round(((DateTime.Now - row.CreateDate).TotalMinutes - row.StopTime) * 100 / (DateTime.Now - row.CreateDate).TotalMinutes, 2);
                if (Utilization < 0)
                    Utilization = 0;
                row.Utilization = Utilization + "%";
                row.ProductTime = (int)(DateTime.Now - row.CreateDate).TotalMinutes + 1;
                if (row.ProductTime == 0)
                {
                    row.CurUPH = 0;
                    row.CT = "0";
                }
                else
                {
                    row.CurUPH = row.Qty * 60 / row.ProductTime;
                    if (row.CurUPH > row.UPH)
                        row.CurUPH = row.UPH;
                    row.CT = Math.Round((double)3600 / row.CurUPH, 5).ToString();
                }
                row.PlanRatio = Math.Round((double)row.CurUPH * 100 / row.UPH, 2) + "%";
                row.PlanCT = Math.Round((double)3600 / row.UPH, 5).ToString();



                //差异产量
                int mDiffQty = row.PlanQty - row.Qty;
                string mPlanGoalRate = Math.Round((double)row.Qty * 100 / row.PlanQty, 2) + "%";
                row.RatioQty = Math.Round((double)row.Qty * 100 / row.PlanQty, 2) + "%";


                #endregion

                dynamic item = new
                {
                    OrderNo = row.OrderNo,
                    State = row.MachineState,
                    Utilization = row.Utilization,
                    RatioQty = row.RatioQty,
                    //1停机信息 4
                    StopCount = row.StopCount,
                    StopTime = row.StopTime,//停工时间分钟
                    RealStartDate = row.RealStartDate, //实际开工时机
                    ProduceTime = row.ProductTime,//生产时间分钟

                    //3产量 5个
                    Qty = row.Qty,//产量
                    PlanQty = row.PlanQty,//计划产量
                    PlanRatio = row.PlanRatio,//计划达成率
                    DiffQty = mDiffQty,
                    //4节拍 4个

                    Ratio = Ratio,//单次计数拼版数
                    CT = row.CT,
                    PlanCT = row.PlanCT,
                    CurUPH = row.CurUPH,
                    UPH = UPH

                };
                result.Add(item);
                #endregion
            }

            catch (Exception ex)
            {

                return new { statues = "error", message = ex.Message };
            }
            return new { status = "success", message = "保存状态成功", rows = result };
        }


        //7车间看板
        [System.Web.Http.HttpPost]
        public dynamic GetRPCShopBoard(dynamic data)
        {
            //PrintLog(string.Format("7777-GetRPCShopBoard接口获取数据{0}", data));
            List<minimes_order_record> result = new List<minimes_order_record>();
            DateTime PlanDate = DateTime.Now; //不用了
            int State = 0;// data.State;
            string TenantID = data.TenantId;
            string WorkShopId = data.WorkShopId;
            try
            {

                List<dynamic> linelist = ApiDataSource.GetLineList(TenantID, WorkShopId, null, null, null).ToObject<List<dynamic>>();

                minimes_order_recordService reportService = new minimes_order_recordService();
                List<minimes_order_record> reportList = reportService.GetModelList(
                ParamQuery.Instance()
                      //.AndWhere("PlanDate", PlanDate)
                      .AndWhere("State", 1)
                      .AndWhere("TenantID", TenantID)
                 );

                foreach (var item1 in linelist)
                {
                    minimes_order_record row = new minimes_order_record();
                    row.Color = "tag-grey";
                    row.LineId = item1.LineId + "";
                    row.LineName = item1.LineName + "";
                    row.OrderNo = "";
                    row.ProductCode = "";
                    row.Qty = 0;
                    row.PlanQty = 0;
                    row.State = 1;
                    row.StateName = "未切单";
                    row.StopCount = 0;
                    row.StopTime = 0;
                    row.CreateDateStr = "";
                    result.Add(row);
                    foreach (var item in reportList)
                    {
                        if (row.LineId == item.LineId)
                        {
                            row.OrderNo = item.OrderNo;
                            row.ProductCode = item.ProductCode;
                            row.Qty = item.Qty;
                            row.PlanQty = item.PlanQty;
                            row.StopCount = item.StopCount;
                            row.StopTime = item.StopTime;
                            row.CreateDateStr = item.CreateDate.ToString("yyyy-MM-dd HH:mm");
                            if (row.PlanQty <= row.Qty)//已完成
                            {
                                row.StateName = "已完成";
                                row.Color = "tag-yellow";
                            }
                            else if (item.MachineState == 1)
                            {
                                row.Color = "tag-green";
                                row.StateName = "生产中";
                            }
                            else
                            {
                                row.Color = "tag-red";
                                row.StateName = "停机中";
                            }

                        }
                    }
                }
                string js = JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return new { statues = "error", message = ex.Message };
            }
            return new { status = "success", message = "保存状态成功", rows = result };
        }

        //8 小时产量
        [System.Web.Http.HttpPost]
        public dynamic GetRPCPHoursQty(dynamic data)
        {
            //PrintLog(string.Format("8888-GetRPCPHoursQty接口获取数据{0}", data));
            List<dynamic> result = new List<dynamic>();
            try
            {
                string LineId = data.LineId;
                string TenantId = data.TenantId;
                string Date = data.Date;
                string sql = string.Format(@"select a.ID, a.OrderNo,a.PlanDate,b.TimeDisplay,a.ProductName,c.UPH,a.Qty,a.QtyNG,a.DetailedNG,a.Remark from minimes_hoursreport a 
left join minimes_time b on a.Time=b.Time
left join minimes_order_record c on a.OrderNo=c.OrderNo and a.PlanDate=c.PlanDate  
where a.LineId = '{0}' and a.PlanDate = '{1}' and a.TenantId = '{2}' and (a.Qty>0 or a.QtyNG>0) order by b.ID; ", LineId, Convert.ToDateTime(Date).Date, TenantId);
                List<dynamic> reportList = new List<dynamic>();
                using (var db = Db.Context("MiniMES"))
                {
                    reportList = db.Sql(sql).QueryMany<dynamic>();
                }
                foreach (var row in reportList)
                {
                    dynamic item = new
                    {
                        ID = row.ID,
                        TimeDisplay = row.TimeDisplay + "",
                        ProductName = row.ProductName + "",
                        UPH = row.UPH + "",
                        Qty = row.Qty + "",
                        QtyNG = row.QtyNG + "",
                        DetailedNG = row.DetailedNG + "",
                        Remark = row.Remark + ""
                    };
                    result.Add(item);
                }
            }
            catch (Exception ex)
            {
                return new { statues = "error", message = ex.Message };
            }
            return new { status = "success", message = "保存状态成功", rows = result };
        }
        //9 小时产量只更新明细原因
        [System.Web.Http.HttpPost]
        public dynamic EditRPCPHoursRemark(dynamic data)
        {
            //PrintLog(string.Format("9999-EditRPCPHoursRemark接口获取数据{0}", data));
            string LineId = data.LineId;
            string TenantId = data.TenantId;
            string Date = data.Date;

            int ID = data.ID;
            string DetailedNG = data.DetailedNG; //原因或不良明细
            string Remark = data.Remark;    //备注信息


            List<dynamic> result = new List<dynamic>();
            try
            {
                int aa = new minimes_hoursreportService().Update(ParamUpdate.Instance()
                  .Column("DetailedNG", DetailedNG)
                  .Column("Remark", Remark)
                  .AndWhere("ID", ID)
                  );

                string sql = string.Format(@"select a.ID, a.OrderNo,a.PlanDate,b.TimeDisplay,a.ProductName,c.UPH,a.Qty,a.QtyNG,a.DetailedNG,a.Remark from minimes_hoursreport a 
left join minimes_time b on a.Time=b.Time
left join minimes_order_record c on a.OrderNo=c.OrderNo and a.PlanDate=c.PlanDate  
where a.LineId = '{0}' and a.PlanDate = '{1}' and a.TenantId = '{2}' and (a.Qty>0 or a.QtyNG>0) order by b.ID; ", LineId, Convert.ToDateTime(Date).Date, TenantId);
                //PrintLog(string.Format("修改返回值aa1:{0},添加返回值sql:{1}", aa, sql));
                List<dynamic> reportList = new List<dynamic>();
                using (var db = Db.Context("MiniMES"))
                {
                    reportList = db.Sql(sql).QueryMany<dynamic>();
                }
                foreach (var row in reportList)
                {
                    dynamic item = new
                    {
                        ID = row.ID,
                        TimeDisplay = row.TimeDisplay + "",
                        ProductName = row.ProductName + "",
                        UPH = row.UPH + "",
                        Qty = row.Qty + "",
                        QtyNG = row.QtyNG + "",
                        DetailedNG = row.DetailedNG + "",
                        Remark = row.Remark + ""
                    };
                    result.Add(item);
                }
            }
            catch (Exception ex)
            {
                return new { statues = "error", message = ex.Message };
            }
            return new { status = "success", message = "保存状态成功", rows = result };
        }
        #endregion


        #region  获取当前正在生产的工单

        /// <summary>
        /// 获取当前正在生产的工单
        /// </summary>
        /// <param name="TenantID"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public dynamic GetOrderNo(string TenantID, string LineId)
        {
            var ProductCode = "";
            var OrderNo = "";
            try
            {
                minimes_order_recordService reportService = new minimes_order_recordService();
                List<minimes_order_record> reportList = reportService.GetModelList(ParamQuery.Instance().AndWhere("LineId", LineId).AndWhere("State", 1).AndWhere("TenantID", TenantID));
                if (reportList.Count != 0)
                {
                    OrderNo = reportList[0].OrderNo;
                    ProductCode = reportList[0].ProductCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            var result = new
            {
                ProductCode = ProductCode,
                OrderNo = OrderNo
            };
            return result;
        }
        #endregion
    }
}
