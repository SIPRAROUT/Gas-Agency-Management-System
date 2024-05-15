using GasAgencyManagementProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GasAgencyManagementProject.Models.Transaction
{
    #region Tbl_Trns_Sales
    public class Tbl_Trns_Sales
    {
        public int Sales_Id { get; set; }
        public string Invoice_No { get; set; }
        public string Booking_Date { get; set; }
        public string Approximate_Delivery_Date { get; set; }
        public int Consumer_Id { get; set; }
        public string Consumer_No { get; set; }
        public string Consumer_Name { get; set; }
        public string Cnsmr_Name { get; set; }
        public string Phone_No { get; set; }
        public string Cnsmr_PhNo { get; set; }
        public int Catgry_Id { get; set; }
        public string Catgry_Name { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public int Stock_Id { get; set; }
        public string Unit_Type { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Total_Amount { get; set; }
        public string Discount { get; set; }
        public decimal Actual_Cost { get; set; }
        public int Delivery_Partner_Id { get; set; }
        public int Area_Id { get; set; }
        public string Area_Info { get; set; }
        public string Area_Name { get; set; }
        public string Dp_Name { get; set; }
        public string Payment_Status { get; set; }
        public decimal Intial_Payment { get; set; }
        public string Intial_Payment_Mode { get; set; }
        public decimal Final_Payment { get; set; }
        public string Final_Payment_Mode { get; set; }
        public string Action { get; set; }
        public int D_Flag { get; set; }
        public int Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
        public List<DT_DT_Product> DT_Product { get; set; }
    }
    public class DT_DT_Product
    {
        public int Sales_Id { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal Actual_Cost { get; set; }
        public string Discount { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int D_Flag { get; set; }

    }

    public class SalesDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Trns_Sales> aaData { get; set; }
    }
    #endregion

    #region  Tbl_Trns_Final_Settlement
    public class Tbl_Trns_Final_Settlement
    {
        public int Delivery_Id { get; set; }
        public string Delivery_Status { get; set; }
        public string Delivery_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Consumer_Name { get; set; }
        public string Phone_No { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal MRP { get; set; }
        public decimal Actual_Cost { get; set; }
        public string Discount { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Total_Amount { get; set; }
        public string Area { get; set; }
        public string Area_Info { get; set; }
        public string Delivery_Name { get; set; }
        public string Dp_Name { get; set; }

        public decimal Intial_Payment { get; set; }
        public string Intial_Payment_Mode { get; set; }
        public string Catgry_Name { get; set; }
        public int Quantity { get; set; }
        public string Payment_Status { get; set; }
        public decimal Final_Payment { get; set; }
        public string Final_Payment_Mode { get; set; }
        public string Action { get; set; }
        public int D_Flag { get; set; }
        public int Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
        public List<DT_DT_FinalSettlement> DT_FinalSettlement { get; set; }
        public int TotalCount { get; set; }

    }
    public class Tbl_Trns_Final_SettlementDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Trns_Final_Settlement> aaData { get; set; }
    }
    public class DT_DT_FinalSettlement
    {
        public int Delivery_Id { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal Actual_Cost { get; set; }
        public string Discount { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int D_Flag { get; set; }

    }

    #endregion

    #region  Tbl_Trns_Return_Sales
    public class Tbl_Trns_Return_Sales
    {
        public int Delivery_Id { get; set; }
        public string Delivery_Status { get; set; }
        public string Delivery_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Consumer_Name { get; set; }
        public string Phone_No { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal MRP { get; set; }
        public decimal Actual_Cost { get; set; }
        public string Discount { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Total_Amount { get; set; }
        public int Area_Id { get; set; }
        public string Area_Info { get; set; }
        public string Dp_Name { get; set; }
        public string Delivery_Name { get; set; }
        public decimal Intial_Payment { get; set; }
        public string Intial_Payment_Mode { get; set; }
        public string Catgry_Name { get; set; }
        public int Quantity { get; set; }
        public string Payment_Status { get; set; }
        public decimal Final_Payment { get; set; }
        public string Final_Payment_Mode { get; set; }
        public string Action { get; set; }
        public int D_Flag { get; set; }
        public int Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
        public List<DT_DT_ReturnSales> DT_ReturnSales { get; set; }


    }
    public class DT_DT_ReturnSales
    {
        public int Delivery_Id { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal Actual_Cost { get; set; }
        public string Discount { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int D_Flag { get; set; }

    }

    #endregion
}

