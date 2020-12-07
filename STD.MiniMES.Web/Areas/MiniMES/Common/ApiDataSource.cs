using Newtonsoft.Json;
using STD.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace STD.MiniMES.Web
{
    public class ApiDataSource
    {
        static string APIGatewayUrl = ZConfig.GetConfigString("APIGatewayUrl");

        /// <summary>
        /// 获取班别列表
        /// </summary>
        /// <returns></returns>
        public static dynamic GetShiftList(string TenantID)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetShiftList",
                TenantId = TenantID
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 获取人员与组织架构的映射表
        /// </summary>
        /// <param name="TenantId"></param>
        /// <param name="OrganizeId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static dynamic GetUserOrganizeMapList(string TenantId, string OrganizeId, string UserName)
        {
            if (string.IsNullOrEmpty(TenantId))
            {
                TenantId = SysHelper.GetTenantId();
            }
            dynamic result = null;
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetUserOrganizeMap",
                TenantId = TenantId,
                OrganizeId = OrganizeId,
                UserName = UserName
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 获取所有的组织架构
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public static dynamic GetGetAllOrganize(string TenantId)
        {
            if (string.IsNullOrEmpty(TenantId))
            {
                TenantId = SysHelper.GetTenantId();
            }
            dynamic result = null;
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetAllOrganize",
                TenantId = TenantId
            };
            result = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            return result;
        }

        /// <summary>
        /// 获取车间列表
        /// </summary>
        /// <returns></returns>
        public static dynamic GetWorkShopList(string TenantID, string WorkShopId)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetWorkShopList",
                WorkShopId = WorkShopId,
                TenantId = TenantID
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        public static dynamic GetAreaList(string TenantID, string WorkShopId, string AreaId)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetAreaList",
                TenantId = TenantID,
                WorkShopId = WorkShopId,
                AreaId = AreaId
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 获取产线列表
        /// </summary>
        /// <returns></returns>
        public static dynamic GetLineList(string TenantID, string WorkShopId, string AreaId, string LineId, string LineCode)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetLineList",
                TenantId = TenantID,
                WorkShopId = WorkShopId,
                AreaId = AreaId,
                LineId = LineId,
                LineCode = LineCode
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }


        /// <summary>
        /// 获取产线列表LineCode转LineCode
        /// </summary>
        /// <returns></returns>
        public static dynamic GetLineCodeList(string TenantID, string WorkShopId, string AreaId, string LineId, string LineCode)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetLineList",
                TenantId = TenantID,
                WorkShopId = WorkShopId,
                AreaId = AreaId,
                LineId = LineId,
                LineCode = LineCode
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            string str = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject(str);
        }

        /// <summary>
        /// 获取产线区段列表
        /// </summary>
        /// <returns></returns>
        public static dynamic GetLineAreaList(string TenantID, string LineID, string LineAreaID, string LineAreaCode)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetLineAreaList",
                TenantId = TenantID,
                LineID = LineID,
                LineAreaID = LineAreaID,
                LineAreaCode = LineAreaCode
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 获取工位列表
        /// </summary>
        /// <param name="TenantID"></param>
        /// <param name="LineCode"></param>
        /// <param name="LineID"></param>
        /// <param name="LineStationId"></param>
        /// <param name="StationCode"></param>
        /// <returns></returns>
        public static dynamic GetStationList(string TenantID, string LineCode, string LineID, string LineStationId, string StationCode)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetLineStationList",
                TenantId = TenantID,
                LineCode = LineCode,
                LineID = LineID,
                LineStationId = LineStationId,
                StationCode = StationCode
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 获取工位列表Station
        /// </summary>
        /// <param name="TenantID"></param>
        /// <param name="LineCode"></param>
        /// <param name="LineID"></param>
        /// <param name="LineStationId"></param>
        /// <param name="StationCode"></param>
        /// <returns></returns>
        public static dynamic GetStationConvertList(string TenantID, string LineCode, string LineID, string LineStationId, string StationCode)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetLineStationList",
                TenantId = TenantID,
                LineCode = LineCode,
                LineID = LineID,
                LineStationId = LineStationId,
                StationCode = StationCode
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            string str = JsonConvert.SerializeObject(result);
            string strresult = str.Replace("StationCode", "Station");
            return JsonConvert.DeserializeObject(strresult);
        }


        /// <summary>
        /// 获取人员列表
        /// </summary>
        /// <returns></returns>
        public static dynamic GetUserList(string TenantID, string UserId, string UserCode, string UserName)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetUserList",
                TenantId = TenantID,
                UserId = UserId,
                UserCode = UserCode,
                UserName = UserName
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }


        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public static dynamic GetOrganizeList(string TenantId, string OrganizeId, string OrganizeCode, string OrganizeName)
        {
            if (string.IsNullOrEmpty(TenantId))
            {
                TenantId = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetOrganizeList",
                TenantId = TenantId,
                OrganizeId = OrganizeId,
                OrganizeCode = OrganizeCode,
                OrganizeName = OrganizeName
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            return list;
        }

        /// <summary>
        /// 获取人员组织架构信息
        /// </summary>
        /// <returns></returns>
        public static dynamic GetUserOrganize(string UserId)
        {
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetUserOrganize",
                UserId = UserId
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            return list;
        }

        /// <summary>
        /// 获取产线班别列表
        /// </summary>
        /// <returns></returns>
        public static dynamic GetLineShiftList(string TenantID, string LineId, string LineName, string ShiftName)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetLineShiftList",
                TenantId = TenantID,
                LineId = LineId,
                LineName = LineName,
                ShiftName = ShiftName
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <returns></returns>
        //public static dynamic GetWorkSheetList(string TenantID, System.DateTime? PlanDate, string TeamId, string WorkSheetNo, string WorkShopId)
        //{
        //    dynamic result = null;

        //    if (string.IsNullOrEmpty(TenantID))
        //    {
        //        TenantID = SysHelper.GetTenantId();
        //    }

        //    var postdata = new
        //    {
        //        AppCode = "PMS",
        //        ApiCode = "GetWorkSheetList",
        //        TenantId = TenantID,
        //        PlanDate = PlanDate,
        //        TeamId = TeamId,
        //        WorkSheetNo = WorkSheetNo,
        //        WorkShopId = WorkShopId
        //    };

        //    dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);

        //    if (list != null)
        //    {
        //        result = list.rows;
        //    }

        //    return result;
        //}



        /// <summary>
        /// 获取工单列表(New)
        /// </summary>
        /// <param name="TenantId"></param>
        /// <param name="WorkSheetNo"></param>
        /// <param name="LineId"></param>
        /// <param name="LineName"></param>
        /// <param name="ProductCode"></param>
        /// <param name="ProductName"></param>
        /// <param name="Status"></param>
        /// <param name="OrderNo"></param>
        /// <param name="PlanStartDate"></param>
        /// <param name="IsTodayProduce"></param>
        /// <returns></returns>
        //public static dynamic GetWorkSheetList(string TenantId, string WorkSheetNo, string LineId, string LineName, string ProductCode, string ProductName, int? Status, string OrderNo, System.DateTime? PlanStartDate, int? IsTodayProduce)
        //{
        //    dynamic result = null;
        //    if (string.IsNullOrEmpty(TenantId))
        //    {
        //        TenantId = SysHelper.GetTenantId();
        //    } 
        //    string LineDesc = "";
        //    string iLineName = ""; 
        //    if (!string.IsNullOrEmpty(LineId))
        //    { 
        //        List<dynamic> linelist = ApiDataSource.GetLineList(TenantId, null, null, LineId, null).ToObject<List<dynamic>>();
        //        if (linelist != null && linelist.Count > 0)
        //        {
        //            LineDesc = linelist[0].LineDesc;
        //            iLineName = linelist[0].LineName; 
        //        }
        //    }
        //    var postdata = new
        //    {
        //        AppCode = "SAPS",
        //        ApiCode = "GetWorkSheetList",
        //        TenantId = TenantId,
        //        WorkSheetNo = WorkSheetNo,
        //        LineId = "",
        //        LineName = LineName,
        //        ProductCode = ProductCode,
        //        ProductName = ProductName,
        //        Status = Status,
        //        OrderNo = OrderNo,
        //        PlanStartDate = PlanStartDate,
        //        IsTodayProduce = IsTodayProduce,
        //        ProcedureName = LineDesc
        //    };
        //    dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
        //    if (list != null)
        //    {
        //        result = list.rows;
        //    }
        //    if (result!=null&& result.Count>0&& !string.IsNullOrEmpty(iLineName))
        //    {
        //        foreach (var item in result)
        //        {
        //            item.LineId = LineId;
        //            item.LineName = iLineName;
        //        }

        //    }
        //    return result;
        //}

        // <summary>
        /// 获取工单列表(New)
        /// </summary>
        /// <param name="TenantId"></param>
        /// <param name="WorkSheetNo"></param>
        /// <param name="LineId"></param>
        /// <param name="LineName"></param>
        /// <param name="ProductCode"></param>
        /// <param name="ProductName"></param>
        /// <param name="Status"></param>
        /// <param name="OrderNo"></param>
        /// <param name="PlanStartDate"></param>
        /// <param name="IsTodayProduce"></param>
        /// <returns></returns>
        public static dynamic GetWorkSheetList(string TenantId, string WorkSheetNo, string LineId, string LineName, string ProductCode, string ProductName, int? Status, string OrderNo, string PlanStartDate, int? IsTodayProduce)
        {
            try
            {
                dynamic result = null;
                if (string.IsNullOrEmpty(TenantId))
                {
                    TenantId = SysHelper.GetTenantId();
                }
                var postdata = new
                {
                    AppCode = "SAPS",
                    ApiCode = "GetWorkSheetList",
                    TenantId = TenantId,
                    WorkSheetNo = WorkSheetNo,
                    LineId = LineId,
                    LineName = LineName,
                    ProductCode = ProductCode,
                    ProductName = ProductName,
                    Status = Status,
                    OrderNo = OrderNo,
                    PlanStartDate = PlanStartDate,
                    IsTodayProduce = IsTodayProduce,
                };
                dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
                if (list != null)
                {
                    result = list.rows;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        public static dynamic GetInventoryList(string TenantID, string InventoryCode, string InventoryName, string InventoryClassId, string page, string rows)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetInventoryList",
                TenantId = TenantID,
                InventoryCode = InventoryCode,
                InventoryName = InventoryName,
                InventoryClassId = InventoryClassId,
                page = page,
                rows = rows
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 更新工单状态信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static void EditWorkSheetStatus(string TenantID, string WorkSheetNo)
        {
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "SAPS",
                ApiCode = "SetWorkSheetStatus",
                TenantId = TenantID,
                WorkSheetNo = WorkSheetNo,
                Status = 5
            };
            dynamic result = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            //return result;
        }


        /// <summary>
        /// 更新工单数量信息
        /// </summary>
        /// <param name="TenantID"></param>
        /// <param name="WorkSheetNo"></param>
        /// <returns></returns>
        public static void EditWorkSheetCount(string TenantID, string WorkSheetNo, int CompletedCount)
        {
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "SAPS",
                ApiCode = "SetWorkSheetCount",
                TenantId = TenantID,
                WorkSheetNo = WorkSheetNo,
                CompletedCount = CompletedCount
            };
            dynamic result = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            //return result;
        }

        /// <summary>
        /// 更新工单数量信息
        /// </summary>
        /// <param name="TenantID"></param>
        /// <param name="WorkSheetNo"></param>
        /// <returns></returns>
        public static dynamic EditWorkSheetCount(string TenantID, string WorkSheetNo, string LineId, string LineName)
        {
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "SAPS",
                ApiCode = "SetWorkSheetLine",
                TenantId = TenantID,
                WorkSheetNo = WorkSheetNo,
                LineId = LineId,
                LineName = LineName
            };
            dynamic result = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            return result;
        }

        /// <summary>
        /// 获取待切工单
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public static dynamic GetChangeOrderNoList(string TenantId, string LineId)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantId))
            {
                TenantId = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "SAPS",
                ApiCode = "GetWorkSheetSynchronousList ",
                LineId = LineId,
                TenantId = TenantId,
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }



        /// <summary>
        /// 获当天异常
        /// </summary>
        /// <returns></returns>
        public static dynamic GetUndisposedException(string WorkShopCode, string TenantID)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "Andon",
                ApiCode = "GetUndisposedException",
                TenantId = TenantID,
                WorkShopCode = WorkShopCode
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 获本周异常
        /// </summary>
        /// <returns></returns>
        public static dynamic GetExceptionWeekAnalysis(string WorkShopCode, string TenantID)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "Andon",
                ApiCode = "GetExceptionWeekAnalysis",
                TenantId = TenantID,
                WorkShopCode = WorkShopCode
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.message;
            }
            return result;
        }


        /// <summary>
        /// 获质量问题
        /// </summary>
        /// <returns></returns>
        public static dynamic GetFirstCheckByWorkShopCode(string WorkShopCode, string TenantID)
        {
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "QualityControl",
                ApiCode = "GetFirstCheckByWorkShopCode",
                TenantId = TenantID,
                WorkShopCode = WorkShopCode
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            return list;
        }

        /// <summary>
        /// 获质量问题
        /// </summary>
        /// <returns></returns>
        public static dynamic GetRandomPltoInfo(string WorkShopCode, string TenantID)
        {
            if (string.IsNullOrEmpty(TenantID))
            {
                TenantID = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "QualityControl",
                ApiCode = "GetRandomPltoInfo",
                TenantId = TenantID,
                WorkShopCode = WorkShopCode
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            return list;
        }

        /// <summary>
        /// 获取工单详细信息
        /// </summary>
        /// <param name="TenantId"></param>
        /// <param name="WorkSheetNo"></param>
        /// <returns></returns>
        public static dynamic GetWorkSheetDetail(string TenantId,string WorkSheetNo)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(TenantId))
            {
                TenantId = SysHelper.GetTenantId();
            }
            var postdata = new
            {
                AppCode = "SAPS",
                ApiCode = "GetWorkSheetDetail",
                TenantId = TenantId,
                WorkSheetNo = WorkSheetNo
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }

        /// <summary>
        /// 工单查询
        /// </summary>
        /// <param name="TenantId"></param>
        /// <param name="WorkSheetNo"></param>
        /// <param name="LineId"></param>
        /// <param name="ProductCode"></param>
        /// <param name="PlanStartDate"></param>
        /// <param name="LineName"></param>
        /// <param name="ShiftId"></param>
        /// <param name="ShiftName"></param>
        /// <param name="ProductName"></param>
        /// <param name="OrderNo"></param>
        /// <param name="TeamId"></param>
        /// <param name="TeamName"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static dynamic GetWorkSheetNoList(string TenantId, string WorkSheetNo, string LineId, string ProductCode, string PlanStartDate)
        {
            try
            {
                dynamic result = null;
                if (string.IsNullOrEmpty(TenantId))
                {
                    TenantId = SysHelper.GetTenantId();
                }
                var postdata = new
                {
                    AppCode = "SAPS",
                    ApiCode = "GetWorkSheetNoList",
                    TenantId = TenantId,
                    WorkSheetNo = WorkSheetNo,
                    LineId = LineId,
                    ProductCode = ProductCode,
                    PlanStartDate = PlanStartDate,
                };
                dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
                if (list != null)
                {
                    result = list;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}