using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GasAgencyManagementProject.Models.Master
{
    #region Tbl_Mstr_Organization
    public class Tbl_Mstr_Organization
    {
        [Key]
        public int Org_Id { get; set; }

        public string Org_Name { get; set; }

        public string Org_Srt_Name { get; set; }

        public string Org_Addrs { get; set; }

        public string Org_PhNo { get; set; }

        public string Org_FaxNo { get; set; }

        public string Org_Email { get; set; }

        public string Org_Website { get; set; }

        public string Org_TinNo { get; set; }

        public string Org_CSTNo { get; set; }

        public string Org_PanNo { get; set; }

        public string Org_SrvcTaxNo { get; set; }

        public string Org_RegNo { get; set; }

        public string Org_TanNo { get; set; }

        public string Org_Logo { get; set; }

        public int Created_By { get; set; }

        public int Modified_By { get; set; }

        public string Action { get; set; }

        public string msg { get; set; }
    }
    #endregion

    #region Tbl_Mstr_User
    public class Tbl_Mstr_User
    {
        [Key]
        public int User_Id { get; set; }

        public string User_Pwd { get; set; }

        public string User_Name { get; set;}

        public string User_Type { get; set; }

        public string User_Email { get; set; }

        public string User_PhNo { get; set; }

        public int Created_By { get; set; }

        public int Modified_By { get; set; }

        public string Action { get; set; }

        public string msg { get; set; }

        public int  TotalCount { get; set; }


    }
    public class UserDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Mstr_User> aaData { get; set; }
    }
    #endregion

    #region Tbl_Mstr_Page
    public class Tbl_Mstr_Page
    {
        public int? Page_Id { get; set; }
        public int? PID { get; set; }
        public int? MID { get; set; }
        public string Page_Name { get; set; }
        public string Controller { get; set; }
        public string READ_ { get; set; }
        public string WRITE_ { get; set; }
        public string DELETE_ { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public string Action { get; set; }
        public string msg { get; set; }
        public bool PERMISSION { get; set; }
    }
    #endregion

    #region Tbl_Mstr_Supplier
    public class Tbl_Mstr_Supplier
    {
        [Key]
        public int Suplr_Id { get; set; }
        public string Suplr_Name { get; set; }
        public string Suplr_Status { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public string Action { get; set; }
        public string msg { get; set; }
        public int TotalCount { get; set; }
    }
    public class SupplierDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Mstr_Supplier> aaData { get; set; }
    }
    #endregion

    #region Tbl_Mstr_Area
    public class Tbl_Mstr_Area
    {
        [Key]
        public int Area_Id { get; set; }
        public string Area_Info { get; set; }
        public string Area_Details { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public string Action { get; set; }
        public string msg { get; set; }
        public int TotalCount { get; set; }
    }
    public class AreaDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Mstr_Area> aaData { get; set; }
    }
    #endregion

    #region Tbl_Mstr_Category
    public class Tbl_Mstr_Category
    {
        [Key]
        public int Catgry_Id { get; set; }
        public string Catgry_Name { get; set; }
        public string Catgry_Status { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public string Action { get; set; }
        public int TotalCount { get; set; }
        public string msg { get; set; }
    }
    public class CategoryDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Mstr_Category> aaData { get; set; }
    }
    #endregion

    #region Tbl_Mstr_Product
    public class Tbl_Mstr_Product
    {
        [Key]
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_No { get; set; }
        public string Catgry_Name { get; set; }
        public int Catgry_Id { get; set; }
        public string Product_Type { get;set; }
        public string Product_Status { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public string Action { get; set; }
        public string msg { get; set; }
        public int TotalCount { get; set; }
    }
    public class ProductDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Mstr_Product> aaData { get; set; }
    }
    #endregion

    #region Tbl_Mstr_Module
    public class Tbl_Mstr_Module
    {
        [Key]
        public int MID { get; set; }
        public string Module_Name { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public string Action { get; set; }
        public string msg { get; set; }
    }
    #endregion

    #region Tbl_Mstr_DeliveryPartner
    public class Tbl_Mstr_DeliveryPartner
    {
        [Key]
        public int Dp_Id { get; set; }
        public string Dp_Name { get; set; }
        public string Dp_PhNo { get; set; }
        public string Dp_Email { get; set; }
        public string Gender { get; set; }
        public string Area_Info { get; set; }
        public int Area_Id { get; set; }
        public string Addrs { get; set; }
        public string Join_Date { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public string Action { get; set; }
        public int TotalCount { get; set; }
        public string msg { get; set; }
    }
    public class Tbl_Mstr_DeliveryPartnerDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Mstr_DeliveryPartner> aaData { get; set; }
    }
    #endregion

    #region Tbl_Mstr_Consumer
    public class Tbl_Mstr_Consumer
    {
        [Key]
        public int Cnsmr_Id { get; set; }
        public string Cnsmr_No { get; set; }
        public string Cnsmr_Name { get; set; }
        public int Catgry_Id { get; set; }
        public string Catgry_Name { get; set; }
        public string Gender { get; set; }
        public string Cnsmr_PhNo { get; set; }
        public string Cnsmr_Email { get; set; }
        public int Area_Id { get; set; }
        public string Area_Info { get; set; }   
        public string Addrs { get; set; }
        public string Photo { get; set; }
        public string base64Data1 { get; set; }
        public string ext1 { get; set; }
        public string hdnImgPath1 { get; set; }
        public string Id_Proof_Type { get; set; }
        public string Id_Proof_No { get; set; }
        public string Id_Proof_Doc { get; set; }
        public string base64Data2 { get; set; }
        public string ext2 { get; set; }
        public string hdnImgPath2 { get; set; }
        public int Product_Id { get; set; }
        public string Status_ { get; set; }
        public string Cnsmr_Date { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public string Action { get; set; }
        public int TotalCount { get; set; }
        public string msg { get; set; }

    }
    public class Tbl_Mstr_ConsumerDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Mstr_Consumer> aaData { get; set; }
    }
    public class Consumer_Auto_Gen
    {
        public string AUTO_GEN_NO { get; set; }
        public string Action { get; set; }

    }
    #endregion

    #region Tbl_Mstr_Stock
    public class Tbl_Mstr_Stock
    {
        [Key]
        public int Stock_Id { get; set; }
        public string Stock_No { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public int Suplr_Id { get; set; }
        public string Suplr_Name { get; set; }
        public int Catgry_Id { get; set; }
        public string Catgry_Name { get; set; }
        public string Unit_Type { get; set; }
        public int Quantity { get; set; }
        public decimal Stock_Price { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public decimal Selling_Price { get; set; }
        public string Entry_Date { get; set; }
        public string _Expiry_Date { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public int TotalCount { get; set; }
        public string Action { get; set; }
        public string msg { get; set; }
        public List<DT_DT_Stock> DT_Stock { get; set; }
    }
    public class DT_DT_Stock
    {
        public int Stock_Id { get; set; }
        public string Stock_No { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public int Catgry_Id { get; set; }
        public string Catgry_Name { get; set; }
        public int Suplr_Id { get; set; }
        public string Suplr_Name { get; set; }
        public string Unit_Type { get; set; }
        public int Quantity { get; set; }
        public decimal Stock_Price { get; set; }
        public decimal Selling_Price { get; set; }
        public int D_Flag { get; set; }

    }
    public class Tbl_Mstr_StockDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Mstr_Stock> aaData { get; set; }
    }
    #endregion

    #region Tbl_Mstr_Stock_DTL(Stockview)
    public class Tbl_Mstr_Stock_DTL
    {
        [Key]
        public int Stock_Id { get; set; }
        public string Stock_No { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public int Suplr_Id { get; set; }
        public string Suplr_Name { get; set; }
        public int Catgry_Id { get; set; }
        public string Catgry_Name { get; set; }
        public string Unit_Type { get; set; }
        public int Quantity { get; set; }
        public decimal Stock_Price { get; set; }
        public decimal Selling_Price { get; set; }
        public string Entry_Date { get; set; }
        public string _Expiry_Date { get; set; }
        public int Created_By { get; set; }
        public int Modified_By { get; set; }
        public int TotalCount { get; set; }
        public string Action { get; set; }
        public string msg { get; set; }
        public List<DT_DT_Stock> DT_Stock { get; set; }


    }
    public class StockpriceDetail
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<Tbl_Mstr_Stock_DTL> aaData { get; set; }
    }
    #endregion

    #region changepasswordmodel
    public class ChangePasswordModel
    {
        public string UFullName { get; set; }
        public string UserId { get; set; }
        public string Pwd { get; set; }
        public string NewPwd { get; set; }
        public string ConfPwd { get; set; }
    }
    #endregion

}

