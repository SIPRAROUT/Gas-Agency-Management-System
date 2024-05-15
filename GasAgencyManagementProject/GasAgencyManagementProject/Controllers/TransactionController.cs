using GasAgencyManagementProject.Models.BLL;
using GasAgencyManagementProject.Models.DLL;
using GasAgencyManagementProject.Models.Master;
using GasAgencyManagementProject.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GasAgencyManagementProject.Controllers
{
    public class TransactionController : Controller
    {
        public string User1 = string.Empty;

        // GET: Transaction
        #region Transaction

        #region Sales
        public ActionResult Sales()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "13", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "13", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            List<Tbl_Mstr_Consumer> lstsales = new List<Tbl_Mstr_Consumer>();
            lstsales = new Bll().BllAllConsumer();
            ViewData["Consumer"] = lstsales;
            List<Tbl_Mstr_Product> lstProduct = new List<Tbl_Mstr_Product>();
            lstProduct = new Bll().BllAllProduct();
            ViewData["Product"] = lstProduct;
            List<Tbl_Mstr_Product> lstStock_Dtl = new List<Tbl_Mstr_Product>();
            lstStock_Dtl = new Bll().BllAllStock();
            ViewData["Stock"] = lstStock_Dtl;
            List<Tbl_Mstr_DeliveryPartner> lstDeliveryPartner = new List<Tbl_Mstr_DeliveryPartner>();
            lstDeliveryPartner = new Bll().BllAllDeliveryPartner();
            ViewData["DeliveryPartner"] = lstDeliveryPartner;
            Tbl_Trns_Sales sales = new Tbl_Trns_Sales();
            string SalesNo = new Dll().ExecuteProcedure
                (
                  "USP_AutogenerateSales_Code",
                  new string[] { "@formtype" },
                  new object[] { "Sales" }, "@Code"
                );
            ViewBag.Code = SalesNo;
            return View(sales);
        }
        public ActionResult AutoStockCode()
        {
            /*Tbl_Mstr_Consumer Consumer = new Tbl_Mstr_Consumer();*/
            string Consumer = new Dll().ExecuteProcedure
                     (
                       "USP_AutogenerateSales_Code",
                       new string[] { "@formtype" },
                       new object[] { "Sales" }, "@Code"
                     );
            return Json(Consumer, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalesIUD(Tbl_Trns_Sales obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }
            obj.Created_By = Convert.ToInt32(Session["User_Id"]);

            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "12", "D");
            obj.Created_By = Convert.ToInt32(User1);
            DataTable dt = new DataTable();
            dt.Columns.Add("Product_Id");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Actual_Cost");
            dt.Columns.Add("MRP");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Discount");
            dt.Columns.Add("Amount");

            if (obj.DT_Product != null)
            {
                for (var i = 0; i < obj.DT_Product.Count; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Product_Id"] = obj.DT_Product[i].Product_Id;
                    dt.Rows[i]["Quantity"] = obj.DT_Product[i].Quantity;
                    dt.Rows[i]["Actual_Cost"] = obj.DT_Product[i].Actual_Cost;
                    dt.Rows[i]["MRP"] = obj.DT_Product[i].MRP;
                    dt.Rows[i]["Rate"] = obj.DT_Product[i].Rate;
                    dt.Rows[i]["Discount"] = obj.DT_Product[i].Discount;
                    dt.Rows[i]["Amount"] = obj.DT_Product[i].Amount;
                }
            }

            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {
                    result = new Bll().BLLTransSalesIUD(obj, dt);
                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {
                result = new Bll().BLLTransSalesIUD(obj, dt);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSales()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];
            SalesDetail salesDetail = new SalesDetail();
            salesDetail = new Bll().BllSalesDetail(displayLength, displayStart, sortCol, sortDir, search);
            /* categoryDetail = new Bll().BllCategoryDetail(2,0, 0,"asc", "");*/
            return Json(new { data = salesDetail.aaData, draw = Request["draw"], recordsTotal = salesDetail.iTotalRecords, recordsFiltered = salesDetail.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSales(string Id)
        {
            Tbl_Trns_Sales salesob = new Tbl_Trns_Sales();
            salesob = new Bll().BllSalesById(Id);
            return Json(salesob, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CasEditProduct(string Id)
        {
            Tbl_Mstr_Stock stockob = new Tbl_Mstr_Stock();
            stockob = new Bll().casBllStockById(Id);

            return Json(stockob, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CasEditConsumer(string Id)
        {
            Tbl_Mstr_Consumer consumerob = new Tbl_Mstr_Consumer();
            consumerob = new Bll().casBllConsumerById(Id);

            return Json(consumerob, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CasEditDelivery(string Id)
        {
            List<Tbl_Mstr_DeliveryPartner> deliveryrob = new List<Tbl_Mstr_DeliveryPartner>();
            deliveryrob = new Bll().casBllDeliveryById(Id);

            return Json(deliveryrob, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region  FinalSettlement
        public ActionResult FinalSettlement()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "14", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "14", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            List<Tbl_Trns_Sales> lstsales = new List<Tbl_Trns_Sales>();
            lstsales = new Bll().BllAllSales();
            ViewData["Sales"] = lstsales;

            return View();
        }
        [HttpPost]
        public ActionResult FinalSettlementIUD(Tbl_Trns_Final_Settlement obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }
            obj.Created_By = Convert.ToInt32(Session["User_Id"]);
            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "13", "D");
            obj.Created_By = Convert.ToInt32(User1);
            DataTable dt = new DataTable();
            dt.Columns.Add("Product_Id");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Actual_Cost");
            dt.Columns.Add("MRP");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Discount");
            dt.Columns.Add("Amount");

            if (obj.DT_FinalSettlement != null)
            {
                for (var i = 0; i < obj.DT_FinalSettlement.Count; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Product_Id"] = obj.DT_FinalSettlement[i].Product_Id;
                    dt.Rows[i]["Quantity"] = obj.DT_FinalSettlement[i].Quantity;
                    dt.Rows[i]["Actual_Cost"] = obj.DT_FinalSettlement[i].Actual_Cost;
                    dt.Rows[i]["MRP"] = obj.DT_FinalSettlement[i].MRP;
                    dt.Rows[i]["Rate"] = obj.DT_FinalSettlement[i].Rate;
                    dt.Rows[i]["Discount"] = obj.DT_FinalSettlement[i].Discount;
                    dt.Rows[i]["Amount"] = obj.DT_FinalSettlement[i].Amount;
                }
            }

            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {
                    result = new Bll().BLLTransFinalSettlementIUD(obj,dt);
                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {
                result = new Bll().BLLTransFinalSettlementIUD(obj,dt);


            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CasEditSales(string Id)
        {
            Tbl_Trns_Sales stockob = new Tbl_Trns_Sales();
            stockob = new Bll().casBllFinalSettlementById(Id);
           
            return Json(stockob, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  ReturnSales
        public ActionResult ReturnSales()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "15", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "15", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            List<Tbl_Trns_Final_Settlement> lstsales = new List<Tbl_Trns_Final_Settlement>();
            lstsales = new Bll().BllAllReturn();
            ViewData["return"] = lstsales;
            return View();
        }
        [HttpPost]
        public ActionResult ReturnSalesIUD(Tbl_Trns_Return_Sales obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }
            obj.Created_By = Convert.ToInt32(Session["User_Id"]);
            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "14", "D");
            obj.Created_By = Convert.ToInt32(User1);
            DataTable dt = new DataTable();
            dt.Columns.Add("Product_Id");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Actual_Cost");
            dt.Columns.Add("MRP");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Discount");
            dt.Columns.Add("Amount");

            if (obj.DT_ReturnSales != null)
            {
                for (var i = 0; i < obj.DT_ReturnSales.Count; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Product_Id"] = obj.DT_ReturnSales[i].Product_Id;
                    dt.Rows[i]["Quantity"] = obj.DT_ReturnSales[i].Quantity;
                    dt.Rows[i]["Actual_Cost"] = obj.DT_ReturnSales[i].Actual_Cost;
                    dt.Rows[i]["MRP"] = obj.DT_ReturnSales[i].MRP;
                    dt.Rows[i]["Rate"] = obj.DT_ReturnSales[i].Rate;
                    dt.Rows[i]["Discount"] = obj.DT_ReturnSales[i].Discount;
                    dt.Rows[i]["Amount"] = obj.DT_ReturnSales[i].Amount;
                }
            }

            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {
                    result = new Bll().BLLTransReturnSalesIUD(obj, dt);
                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {
                result = new Bll().BLLTransReturnSalesIUD(obj, dt);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CasEditreturn(string Id)
        {
            Tbl_Trns_Final_Settlement stockob = new Tbl_Trns_Final_Settlement();
            stockob = new Bll().casBllReturnsalesById(Id);

            return Json(stockob, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  SalesStatus
        public ActionResult SalesStatus()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "16", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "16", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            return View();
        }
        public ActionResult GetFinalSettlement()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];

            Tbl_Trns_Final_SettlementDetail finalSettlement = new Tbl_Trns_Final_SettlementDetail();
            finalSettlement = new Bll().BLLFinalSettlement(displayLength, displayStart, sortCol, sortDir, search);
            return Json(new { data = finalSettlement.aaData, draw = Request["draw"], recordsTotal = finalSettlement.iTotalRecords, recordsFiltered = finalSettlement.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }

       

        #endregion

        #endregion
    }
}