using GasAgencyManagementProject.Models.BLL;
using GasAgencyManagementProject.Models.DLL;
using GasAgencyManagementProject.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GasAgencyManagementProject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Loginchk(string useremail, string pwd)
         {
            var rslt = "#sucess";
            Tbl_Mstr_User objLoginModels = new Tbl_Mstr_User();
            objLoginModels = new Bll().BLLCheckLogin(new Tbl_Mstr_User { User_Email = useremail, User_Pwd = pwd });
            try
            {
                switch (objLoginModels.msg)
                {
                    case "invalid User Email !":
                        rslt = "#invalid User Email !";
                        break;
                    case "invalid password !":
                        rslt = "#invalid password !";
                        break;
                    case "invalid User Email and password !":
                        rslt = "#invalid User Email and password !";
                        break;
                    case "valid":
                        {
                            //string sql = "Update [dbo].[Tbl_Mstr_User] set LastLogin=GETDATE() where Id='" + objLoginModels.User_Id.ToString() + "'";
                            //new Dll().ExecuteScalar(sql);
                            DataTable dt = new DataTable();
                            dt = new Dll().FillDataTable("SELECT  [Org_Name],[Org_Addrs],[Org_PhNo],[Org_Email] FROM [dbo].[Tbl_Mstr_Organization]");
                            rslt = "#sucess";
                            Session["User_Id"] = objLoginModels.User_Id.ToString();
                            Session["User_Email"] = useremail;
                            Session["User_Pwd"] = pwd;
                            //Session["LastLogin"] = objLoginModels.LastLogin.ToString();
                            Session["User_Name"] = objLoginModels.User_Name.ToString();
                            Session["User_Type"] = objLoginModels.User_Type;
                            //Session["O_Id"] = dt.Rows[0]["O_Id"].ToString();
                            Session["Org_Name"] = dt.Rows[0]["Org_Name"].ToString();
                            Session["Org_Addrs"] = dt.Rows[0]["Org_Addrs"].ToString();
                            Session["Org_PhNo"] = dt.Rows[0]["Org_PhNo"].ToString();
                            Session["Org_Email"] = dt.Rows[0]["Org_Email"].ToString();
                        }
                        break;
                    default:
                        rslt = "An Error Occurs Please try again";
                        break;
                }
            }
            catch (Exception ex)
            {
                rslt = ex.Message;
                //rslt = "An Error Occurs Please try again/02";
            }
            finally
            {
                ModelState.Clear();
            }
            rslt = rslt + "|" + useremail;
            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            FormsAuthentication.SignOut();


            this.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.Cache.SetNoStore();
            return RedirectToAction("Login");

        }
        public ActionResult eye()
        {
            return View();  
        }

    }
}