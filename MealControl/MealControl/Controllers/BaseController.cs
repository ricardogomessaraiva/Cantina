using Contexts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MealControl.Controllers
{
    public class BaseController : Controller
    {
        public MealEntities entity { get; set; }

        public BaseController()
        {
            entity = new MealEntities();
        }

        [HttpGet]
        public ActionResult GetUser()
        {
            var user = new User();

            if (Request.Cookies["meal_user"] != null)
                user = new JavaScriptSerializer().Deserialize<User>(Request.Cookies["meal_user"].Value);

            user.Password = null;

            return Json(new
            {
                user
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public ActionResult Logout()
        {
            Session.Abandon();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}