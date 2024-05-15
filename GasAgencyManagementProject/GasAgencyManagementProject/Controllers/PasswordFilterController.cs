using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GasAgencyManagementProject.Controllers
{
    public class PasswordFilterController : ActionFilterAttribute
    {
        // GET: Password
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var passwordSession = filterContext.HttpContext.Session["User_Pwd"];

            if (passwordSession == null)
            {
                // Redirect to the login page
                filterContext.Result = new RedirectResult("~/Login/Login");
            }

            base.OnActionExecuting(filterContext);

        }
    }
}