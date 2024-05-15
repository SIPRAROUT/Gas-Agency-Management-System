using GasAgencyManagementProject.Models.BLL;
using GasAgencyManagementProject.Models.DLL;
using GasAgencyManagementProject.Models.Master;
using GasAgencyManagementProject.Models.Transaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GasAgencyManagementProject.Controllers
{
    public class MasterController : Controller
    {
        
        public string User1 = string.Empty;

        
        #region Dashboard
        public ActionResult DashBoard()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string UserId = Convert.ToString(Session["User_Id"]);

            List<Tbl_Mstr_Page> list1 = new List<Tbl_Mstr_Page>();

            list1 = new Bll().BLL_CheckAccess(int.Parse(UserId));

            Session["CHKACCSEE"] = list1;
            return View();
        }
        #endregion

        #region Change Password
        [PasswordFilterController]
        [HttpPost]
        public ActionResult SavePass(string Cpass, string Npass, string CFpass)
        {
            // MstUserModel objMstUserModel = new MstUserModel();
            string Msg = "";
            if (Session["User_Pwd"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["User_Pwd"].ToString() != Cpass.Trim())
            {
                Msg = "Your Password is invalid Please enter valid password !";
            }
            else
            {
                Msg = new Bll().BLL_ChangePassword 
                  (
                  new ChangePasswordModel { UserId = Session["User_Id"].ToString(), Pwd = Cpass, NewPwd = Npass, ConfPwd = CFpass }
                  );
                Session["User_Pwd"] = CFpass;
            }

            var rslt = Msg;
            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Administrator

        #region Organisation 
        public ActionResult Organisation()
        {
            //if (Session["U_Id"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "1", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "1", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            
            Tbl_Mstr_Organization obj = new Tbl_Mstr_Organization();
            obj = new Bll().BLLOrganizationDtl();
          

            return View(obj);
        }
        [HttpPost]
        public ActionResult SaveOrganisationMaster(string Org_Id, string Org_Name, string Org_Srt_Name, string Org_Addrs, string Org_PhNo, string Org_FaxNo,
         string Org_Email, string Org_Website, string Org_TinNo, string Org_CSTNo, string Org_PanNo, string Org_SrvcTaxNo, string Org_RegNo,
          string Org_TanNo, string base64Data, string ext, string hdnImgPath)
        {
            //if (Session["U_Id"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}

            string result = string.Empty;
            string ImagePath = "";
            string Action = "INSERT";
            if (Org_Id != "0" && Org_Id != "")
            {
                Action = "UPDATE";
            }
            if (base64Data == "0")
            {
                ImagePath = hdnImgPath;
            }
            else
            {
                ImagePath = new Bll().BLLSaveImage(base64Data, ext, "images/Org/");
            }

            Tbl_Mstr_Organization obj = new Tbl_Mstr_Organization()
            {
                Org_Id = Convert.ToInt32(Org_Id),
                Org_Name = Org_Name,
                Org_Srt_Name = Org_Srt_Name,
                Org_Addrs = Org_Addrs,
                Org_PhNo = Org_PhNo,
                Org_FaxNo = Org_FaxNo,
                Org_Email = Org_Email,
                Org_Website = Org_Website,
                Org_TinNo = Org_TinNo,
                Org_CSTNo = Org_CSTNo,
                Org_PanNo = Org_PanNo,
                Org_SrvcTaxNo = Org_SrvcTaxNo,
                Org_RegNo = Org_RegNo,
                Org_TanNo = Org_TanNo,
                Org_Logo = ImagePath,
                Created_By = 1,
                Modified_By = 1,
                Action = Action
            };
            result = new Bll().BLLSaveOrganisation(obj);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Users

        public ActionResult User()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "2", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "2", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
           
            return View();
        }

        [HttpPost]
        public ActionResult MstrUserIUD(Tbl_Mstr_User obj)
        {

            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }


            obj.Created_By = Convert.ToInt32(Session["User_Id"]);
            
            
            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "2", "D");
            obj.Created_By = Convert.ToInt32(User1);
            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {
                    result = new Bll().BLLMstrUserIUD(obj);

                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {
                result = new Bll().BLLMstrUserIUD(obj);

            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUser()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];

            UserDetail equipmentDetail = new UserDetail();
            equipmentDetail = new Bll().BLLUserDetail(displayLength, displayStart, sortCol, sortDir, search);
            return Json(new { data = equipmentDetail.aaData, draw = Request["draw"], recordsTotal = equipmentDetail.iTotalRecords, recordsFiltered = equipmentDetail.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditUser(string Id)
        {
            Tbl_Mstr_User Userob = new Tbl_Mstr_User();
            Userob = new Bll().BLLUserById(Id);
            return Json(Userob, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UserAccess
        public ActionResult UserAccess()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            //------------------------UserNames------------------------------
            List<Tbl_Mstr_User> lsttblMUserAccess = new List<Tbl_Mstr_User>();
            lsttblMUserAccess = new Bll().BLL_GetUserName(Session["User_Id"].ToString());
            ViewData["UserName"] = lsttblMUserAccess;


            //ViewData["UserName"] = "Admin";

            //-----------------------Module Names--------------------------------
            List<Tbl_Mstr_Module> lsttblModuleAccess = new List<Tbl_Mstr_Module>();
            lsttblModuleAccess = new Bll().BLL_GetModuleName();
            ViewData["ModuleName"] = lsttblModuleAccess;
            //---------------------------------------------------------------
              string UserId = Convert.ToString(Session["User_Id"]);
            List<Tbl_Mstr_Page> list1 = new List<Tbl_Mstr_Page>();
              list1 = new Bll().BLL_CheckAccess(int.Parse(UserId));

            //int uid = 1;
            //list1 = new Bll().BLL_CheckAccess(uid);

            Session["CHKACCSEE"] = list1;
            List<Tbl_Mstr_User> lstUser = new List<Tbl_Mstr_User>();
            lstUser = new Bll().BllAllUser();
            ViewData["User"] = lstUser;
            //------------------------------------------------------------------- 
            return View();
        }
        [HttpPost]
        public ActionResult SaveUserAccess(string ListOfData, string userid)
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string res = "";

            int updatedby = int.Parse(Convert.ToString(Session["User_Id"]));
            //res = "hello";
            res = new Bll().BLLSaveUserAccess(ListOfData, int.Parse(userid), updatedby);

            //res = new Bll().BLLSaveUserAccess(ListOfData, int.Parse(userid), 1);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult showaccess(string Id, string MId)
        {
            int parsedId, parsedMId;
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            List<Tbl_Mstr_Page> page = new List<Tbl_Mstr_Page>();
            if (int.TryParse(Id, out parsedId) && int.TryParse(MId, out parsedMId))
            {
                // Parsing succeeded
                page = new Bll().BLLGet_Page_Details_acc(parsedId, parsedMId);
            }
            else
            {

            }
            //page = new Bll().BLLGet_Page_Details_acc(int.Parse(Id), int.Parse(MId));
            return Json(page, JsonRequestBehavior.AllowGet);
        }

        public Tbl_Mstr_Page Useraccessset(string User, string pid, string type)
        {

            Tbl_Mstr_Page LIST = new Tbl_Mstr_Page();
            LIST = new Bll().BLL_CHK_RWD_Access(int.Parse(User), int.Parse(pid), type);
            return LIST;
        }
        [HttpPost]
        public ActionResult GetPageDtls(string Module_Id)
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            List<Tbl_Mstr_Page> page = new List<Tbl_Mstr_Page>();
            page = new Bll().BLLGet_Page_Details(Convert.ToInt32(Module_Id));
            return Json(page, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Master

        #region Supplier
        public ActionResult Supplier()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "4", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "4", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            return View();
        }
        [HttpPost]
        public ActionResult MstrSupplierIUD(Tbl_Mstr_Supplier obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }
            obj.Created_By = Convert.ToInt32(Session["User_Id"]);

            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "4", "D");
            obj.Created_By = Convert.ToInt32(User1);
            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {
                    result = new Bll().BLLMstrSupplierIUD(obj);
                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {
                result = new Bll().BLLMstrSupplierIUD(obj);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSupplier()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];

            SupplierDetail supplierDetail = new SupplierDetail();
            supplierDetail = new Bll().BLLSupplierDetail(displayLength, displayStart, sortCol, sortDir, search);
            return Json(new { data = supplierDetail.aaData, draw = Request["draw"], recordsTotal = supplierDetail.iTotalRecords, recordsFiltered = supplierDetail.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSupplier(string Id)
        {
            Tbl_Mstr_Supplier Supplierob = new Tbl_Mstr_Supplier();
            Supplierob = new Bll().BllSupplierById(Id);
            return Json(Supplierob, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Category
        public ActionResult Category()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "5", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "5", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            return View();
        }
        [HttpPost]
        public ActionResult MstrCategoryIUD(Tbl_Mstr_Category obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }
            obj.Created_By = Convert.ToInt32(Session["User_Id"]);

            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "5", "D");
            obj.Created_By = Convert.ToInt32(User1);
            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {

                    result = new Bll().BLLMstrCategoryIUD(obj);

                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {

                result = new Bll().BLLMstrCategoryIUD(obj);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategory()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];

            CategoryDetail categoryDetail = new CategoryDetail();
            categoryDetail = new Bll().BllCategoryDetail(displayLength, displayStart, sortCol, sortDir, search);
            /* categoryDetail = new Bll().BllCategoryDetail(2,0, 0,"asc", "");*/
            return Json(new { data = categoryDetail.aaData, draw = Request["draw"], recordsTotal = categoryDetail.iTotalRecords, recordsFiltered = categoryDetail.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCategory(string Id)
        {
            Tbl_Mstr_Category categoryob = new Tbl_Mstr_Category();
            categoryob = new Bll().BllCategoryById(Id);

            return Json(categoryob, JsonRequestBehavior.AllowGet);
        }



        public ActionResult CasEditCategory(string Id)
        {
            Tbl_Mstr_Category categoryob = new Tbl_Mstr_Category();
            categoryob = new Bll().casBllCategoryById(Id);

            return Json(categoryob, JsonRequestBehavior.AllowGet);
        }


        #endregion Category

        #region Product
        public ActionResult Product(string Id)
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "6", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "6", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            List<Tbl_Mstr_Category> lstCatgry = new List<Tbl_Mstr_Category>();
            lstCatgry = new Bll().BllAllCategory();
            ViewData["Category"] = lstCatgry;
            Tbl_Mstr_Product product = new Tbl_Mstr_Product();
            product = new Bll().BllProductById(Id);
            string ConsumerNo = new Dll().ExecuteProcedure
               (
                 "USP_AutogenerateProduct_Code",
                 new string[] { "@formtype" },
                 new object[] { "Product" }, "@Code"
               );
            ViewBag.Code = ConsumerNo;
            return View(product);
        }
        public ActionResult AutoProductCode()
        {
            /*Tbl_Mstr_Consumer Consumer = new Tbl_Mstr_Consumer();*/
            string Consumer = new Dll().ExecuteProcedure
                     (
                       "USP_AutogenerateProduct_Code",
                       new string[] { "@formtype" },
                       new object[] { "Product" }, "@Code"
                     );
            return Json(Consumer, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult MstrProductIUD(Tbl_Mstr_Product obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }
            obj.Created_By = Convert.ToInt32(Session["User_Id"]);

            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "6", "D");
            obj.Created_By = Convert.ToInt32(User1);
            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {

                    result = new Bll().BLLMstrProductIUD(obj);

                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {

                result = new Bll().BLLMstrProductIUD(obj);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProduct()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];

            ProductDetail productDetail = new ProductDetail();
            productDetail = new Bll().BllProductDetail(displayLength, displayStart, sortCol, sortDir, search);
            /* categoryDetail = new Bll().BllCategoryDetail(2,0, 0,"asc", "");*/
            return Json(new { data = productDetail.aaData, draw = Request["draw"], recordsTotal = productDetail.iTotalRecords, recordsFiltered = productDetail.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditProduct(string Id)
        {
            Tbl_Mstr_Product productob = new Tbl_Mstr_Product();
            productob = new Bll().BllProductById(Id);
            return Json(productob, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSales(string Id)
        {
            Tbl_Mstr_Product productob = new Tbl_Mstr_Product();
           productob = new Bll().BllProductById(Id);
            return Json(productob, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ProductDetail
        public ActionResult ProductDetails()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "7", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "7", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            return View();
        }
        #endregion

        #region Area master

        public ActionResult Area()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "8", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "8", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            return View();
        }
        //-----------------insert--------------------------------//

        [HttpPost]
        public ActionResult MstrAreaIUD(Tbl_Mstr_Area obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }
            obj.Created_By = Convert.ToInt32(Session["User_Id"]);

            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "7", "D");
            obj.Created_By = Convert.ToInt32(User1);
            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {

                    result = new Bll().BLLMstrAreaIUD(obj);

                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {

                result = new Bll().BLLMstrAreaIUD(obj);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

       

        public ActionResult GetArea()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];
            string search = Request["search[value]"];



            AreaDetail AreaDetail = new AreaDetail();
            AreaDetail = new Bll().BLLAreaDetail(displayLength, displayStart, sortCol, sortDir, search);
            /*  AreaDetail = new Bll().BLLAreaDetail(2, 0, 0, "asc", "");*/
            return Json(new { data = AreaDetail.aaData, draw = Request["draw"], recordsTotal = AreaDetail.iTotalRecords, recordsFiltered = AreaDetail.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);


        }

        //--------------edit-----------------------//
        public ActionResult EditArea(string Id)
        {
            Tbl_Mstr_Area areaob = new Tbl_Mstr_Area();
            areaob = new Bll().BllAreaById(Id);
            return Json(areaob, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ConsumerType
        //public ActionResult Consumertype()
        //{
        //    if (Session["User_Id"] == null)
        //    {
        //        return RedirectToAction("Login", "Login");
        //    }
        //    User1 = Convert.ToString(Session["User_Id"]);
        //    Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "9", "W");
        //    Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "9", "R");
        //    ViewData["PERMISSION"] = MT;
        //    ViewData["PERMISSIONREAD"] = MTS;
        //    return View();
        //}

        ////insert//

        //[HttpPost]
        //public ActionResult MstrCnsmrTypeIUD(Tbl_Mstr_ConsumerType obj)
        //{
        //    if (Session["User_Id"] == null || Session["User_Id"] == "")
        //    {
        //        return RedirectToAction("Login", "Login");
        //    }
        //    obj.Created_By = Convert.ToInt32(Session["User_Id"]);

        //    string result = "";
        //    User1 = Convert.ToString(Session["User_Id"]);
        //    Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "9", "D");
        //    obj.Created_By = Convert.ToInt32(User1);
        //    if (obj.Action == "DELETE")
        //    {
        //        if (MTD.PERMISSION == true)
        //        {

        //            result = new Bll().BLLMstrConsumerTypeIUD(obj);

        //        }
        //        else
        //        {
        //            result = "YOU HAVE NO PERMISSION FOR DELETE!";
        //        }
        //    }
        //    else
        //    {

        //        result = new Bll().BLLMstrConsumerTypeIUD(obj);

        //    }



        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult GetConsumerType()
        //{
        //    int displayStart = Convert.ToInt32(Request["start"]);
        //    int displayLength = Convert.ToInt32(Request["length"]);
        //    int sortCol = Convert.ToInt32(Request["order[0][column]"]);
        //    string sortDir = Request["order[0][dir]"];
        //    string search = Request["search[value]"];



        //    ConsumerTypeDetail ConsumerTypeDetail = new ConsumerTypeDetail();
        //    ConsumerTypeDetail = new Bll().BLLConsumerTypeDetail(displayLength, displayStart, sortCol, sortDir, search);
        //    /*  AreaDetail = new Bll().BLLAreaDetail(2, 0, 0, "asc", "");*/
        //    return Json(new { data = ConsumerTypeDetail.aaData, draw = Request["draw"], recordsTotal = ConsumerTypeDetail.iTotalRecords, recordsFiltered = ConsumerTypeDetail.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        //}



        ////--------------edit-----------------------//
        //public ActionResult EditConsumerType(string Id)
        //{
        //    Tbl_Mstr_ConsumerType cnsmrob = new Tbl_Mstr_ConsumerType();
        //    cnsmrob = new Bll().BllConsumerTypeById(Id);
        //    return Json(cnsmrob, JsonRequestBehavior.AllowGet);
        //}


        #endregion

        #region Deliverypartner
        [HttpGet]
        public ActionResult Deliverypartner()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "9", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "9", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            List<Tbl_Mstr_Area> lstArea = new List<Tbl_Mstr_Area>();
            lstArea = new Bll().BllAllArea();
            ViewData["Area"] = lstArea;
            return View();
        }
        [HttpPost]
        public ActionResult MstrDeliveryPartnerIUD(Tbl_Mstr_DeliveryPartner obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }
            obj.Created_By = Convert.ToInt32(Session["User_Id"]);

            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "8", "D");
            obj.Created_By = Convert.ToInt32(User1);
            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {

                    result = new Bll().BLLMstrDeliveryPartnerIUD(obj);

                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {

                result = new Bll().BLLMstrDeliveryPartnerIUD(obj);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDeliveryPartner()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];

            Tbl_Mstr_DeliveryPartnerDetail deliverypartnerdt = new Tbl_Mstr_DeliveryPartnerDetail();
            deliverypartnerdt = new Bll().BLLDeliveryPartnerDetail(displayLength, displayStart, sortCol, sortDir, search);
            return Json(new { data = deliverypartnerdt.aaData, draw = Request["draw"], recordsTotal = deliverypartnerdt.iTotalRecords, recordsFiltered = deliverypartnerdt.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeliveryPartnerEdit(string Id)
        {
            Tbl_Mstr_DeliveryPartner deliveryPartner = new Tbl_Mstr_DeliveryPartner();

            if (Id != null && Id != "")
            {
                deliveryPartner = new Bll().BLLDeliveryPartnerById(Id);

            }

            return Json(deliveryPartner, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Consumer
        public ActionResult Consumer()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "10", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "10", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;

            //List<Tbl_Mstr_ConsumerType> lstConsumerType = new List<Tbl_Mstr_ConsumerType>();
            //lstConsumerType = new Bll().BllAllConsumerType();
            //ViewData["ConsumerType"] = lstConsumerType;
            List<Tbl_Mstr_Category> lstCatgry = new List<Tbl_Mstr_Category>();
            lstCatgry = new Bll().BllAllCategory();
            ViewData["Category"] = lstCatgry;
            List<Tbl_Mstr_Area> lstArea = new List<Tbl_Mstr_Area>();
            lstArea = new Bll().BllAllArea();
            ViewData["Area"] = lstArea;
            //List<Tbl_Mstr_Product> lstProduct = new List<Tbl_Mstr_Product>();
            //lstProduct = new Bll().BllAllProduct();
            //ViewData["Product"] = lstProduct;

            Tbl_Mstr_Consumer Consumer = new Tbl_Mstr_Consumer();
            //string ConsumerNo = new Dll().ExecuteProcedure
            //    (
            //      "USP_Autogenerate_Code",
            //      new string[] { "@formtype", " @CategoryId" },
            //      new object[] { "Consumer",Consumer.Catgry_Id }, "@Code"
            //    );
            //ViewBag.Code = ConsumerNo;
            return View(Consumer);
        }
        public ActionResult AutoCunsumerCode(int Catgry_Id)
        {
            try
            {
                Tbl_Mstr_Category Consumer1=new Tbl_Mstr_Category();
                //Tbl_Mstr_Consumer Consumer1 = new Tbl_Mstr_Consumer();
                string Consumer = new Dll().ExecuteProcedure(
                    "USP_Autogenerate_Code",
                    new string[] { "@formtype", "@CategoryId" },
                    new object[] { "Consumer", Catgry_Id },
                    "@Code"
                );

                return Json(Consumer, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in AutoCunsumerCode: {ex.Message}");

                // Return a JSON response with an error message
                return Json(new { error = "Error fetching Consumer No" });
            }
        }


        public ActionResult ConsumerEdit(string Id)
        {
            Tbl_Mstr_Consumer Consumer = new Tbl_Mstr_Consumer();
           

            if (Id != null && Id != "")
            {
                Consumer = new Bll().BLLConsumerById(Id);

            }

            return Json(Consumer, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult MstrConsumerIUD(Tbl_Mstr_Consumer obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }

            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "9", "D");
            obj.Created_By = Convert.ToInt32(User1);
            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {

                    result = new Bll().BLLMstrConsumerIUD(obj);


                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {

                string ImagePath1 = "";
                string ImagePath2 = "";

                if (obj.base64Data1 == "0")
                {
                    ImagePath1 = obj.hdnImgPath1;
                }
                else
                {
                    ImagePath1 = new Bll().BLLSaveImage1(obj.base64Data1, obj.ext1, "images/Image/");
                }
                obj.Photo = ImagePath1;
                if (obj.base64Data2 == "0")
                {
                    ImagePath2 = obj.hdnImgPath2;
                }
                else
                {
                    ImagePath2 = new Bll().BLLSaveFile(obj.base64Data2, obj.ext2, "images/Document/");
                }
                obj.Id_Proof_Doc = ImagePath2;
                //-----------------------------------------
                result = new Bll().BLLMstrConsumerIUD(obj);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetConsumer()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];

            Tbl_Mstr_ConsumerDetail ConsumerDetail = new Tbl_Mstr_ConsumerDetail();
            ConsumerDetail = new Bll().BLLConsumerDetail(displayLength, displayStart, sortCol, sortDir, search);
            return Json(new { data = ConsumerDetail.aaData, draw = Request["draw"], recordsTotal = ConsumerDetail.iTotalRecords, recordsFiltered = ConsumerDetail.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }




        #endregion

        #region Stock
        public ActionResult Stock()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "11", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "11", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
            List<Tbl_Mstr_Supplier> lstSupplier = new List<Tbl_Mstr_Supplier>();
            lstSupplier = new Bll().BllAllSupplier();
            ViewData["Supplier"] = lstSupplier;
            List<Tbl_Mstr_Category> lstCatgry = new List<Tbl_Mstr_Category>();
            lstCatgry = new Bll().BllAllCategory();
            ViewData["Category"] = lstCatgry;
            List<Tbl_Mstr_Product> lstProduct = new List<Tbl_Mstr_Product>();
            lstProduct = new Bll().BllAllProduct();
            ViewData["Product"] = lstProduct;
            Tbl_Mstr_Stock stock = new Tbl_Mstr_Stock();
            string ConsumerNo = new Dll().ExecuteProcedure
                (
                  "USP_AutogenerateStock_Code",
                  new string[] { "@formtype" },
                  new object[] { "Stock" }, "@Code"
                );
            ViewBag.Code = ConsumerNo;
            return View(stock);
        }
        public ActionResult AutoStockCode()
        {
            /*Tbl_Mstr_Consumer Consumer = new Tbl_Mstr_Consumer();*/
            string Consumer = new Dll().ExecuteProcedure
                     (
                       "USP_AutogenerateStock_Code",
                       new string[] { "@formtype" },
                       new object[] { "Stock" }, "@Code"
                     );
            return Json(Consumer, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MstrStockIUD(Tbl_Mstr_Stock obj)
        {
            if (Session["User_Id"] == null || Session["User_Id"] == "")
            {
                return RedirectToAction("Login", "Login");
            }
            obj.Created_By = Convert.ToInt32(Session["User_Id"]);
            string result = "";
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MTD = new MasterController().Useraccessset(User1, "10", "D");
            obj.Created_By = Convert.ToInt32(User1);
            DataTable dt = new DataTable();
            dt.Columns.Add("Product_Id");
            dt.Columns.Add("Catgry_Id");
            dt.Columns.Add("Unit_Type");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Stock_Price");
            dt.Columns.Add("Selling_Price");
            if (obj.DT_Stock != null)
            {
                for (var i = 0; i < obj.DT_Stock.Count; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Product_Id"] = obj.DT_Stock[i].Product_Id;
                    dt.Rows[i]["Catgry_Id"] = obj.DT_Stock[i].Catgry_Id;
                    dt.Rows[i]["Unit_Type"] = obj.DT_Stock[i].Unit_Type;
                    dt.Rows[i]["Quantity"] = obj.DT_Stock[i].Quantity;
                    dt.Rows[i]["Stock_Price"] = obj.DT_Stock[i].Stock_Price;
                    dt.Rows[i]["Selling_Price"] = obj.DT_Stock[i].Selling_Price;
                }
            }

            if (obj.Action == "DELETE")
            {
                if (MTD.PERMISSION == true)
                {

                    result = new Bll().BLLMstrStockIUD(obj,dt);

                }
                else
                {
                    result = "YOU HAVE NO PERMISSION FOR DELETE!";
                }
            }
            else
            {
                result = new Bll().BLLMstrStockIUD(obj,dt);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStockPrice()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];

            StockpriceDetail stock = new StockpriceDetail();
            stock = new Bll().BLLStockDetail(displayLength, displayStart, sortCol, sortDir, search);
            return Json(new { data = stock.aaData, draw = Request["draw"], recordsTotal = stock.iTotalRecords, recordsFiltered = stock.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StockEdit(string Id)
        {
            Tbl_Mstr_Stock_DTL stock = new Tbl_Mstr_Stock_DTL();

            if (Id != null && Id != "")
            {
                stock = new Bll().BLLStockPriceById(Id);

            }
            return Json(stock, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StockDTEdit(string Id)
        {
            List<DT_DT_Stock> stock = new List<DT_DT_Stock>();

            if (Id != null && Id != "")
            {
                stock = new Bll().BLLStockPriceDTById(Id);

            }
            return Json(stock, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetStockDetails(int stockId)
        //{
        //    List<DT_DT_Stock> stockDetails = new Bll().BLLStockDetails(stockId);
        //    return Json(stockDetails, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region StockPrice
        public ActionResult StockPrice()
        {
            if (Session["User_Id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            User1 = Convert.ToString(Session["User_Id"]);
            Tbl_Mstr_Page MT = new MasterController().Useraccessset(User1, "12", "W");
            Tbl_Mstr_Page MTS = new MasterController().Useraccessset(User1, "12", "R");
            ViewData["PERMISSION"] = MT;
            ViewData["PERMISSIONREAD"] = MTS;
           
            return View();
        }
        public ActionResult GetStock()
        {
            int displayStart = Convert.ToInt32(Request["start"]);
            int displayLength = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            int sortCol = Convert.ToInt32(Request["order[0][column]"]);
            string sortDir = Request["order[0][dir]"];

            Tbl_Mstr_StockDetail stock = new Tbl_Mstr_StockDetail();
            stock = new Bll().BLLStock(displayLength, displayStart, sortCol, sortDir, search);
            return Json(new { data = stock.aaData, draw = Request["draw"], recordsTotal = stock.iTotalRecords, recordsFiltered = stock.iTotalDisplayRecords }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStockDetails(int stockId)
        {
            List<DT_DT_Stock> stockDetails = new Bll().BLLStockDetails(stockId);
            return Json(stockDetails, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

    }
}
