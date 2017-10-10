using Contexts;
using Models;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MealControl.Controllers
{

    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Validate(User user)
        {
            //var x = BCrypt.Net.BCrypt.HashPassword("");
            //var Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var entites = new MealEntities();
            var model = entites.User.FirstOrDefault(u => u.UserName == user.UserName);

            if (model == null)
                return new HttpStatusCodeResult((HttpStatusCode)404, "Usuário/Senha inválida.");            

            if (!BCrypt.Net.BCrypt.Verify(user.Password, model.Password))
                return new HttpStatusCodeResult((HttpStatusCode)404, "Usuário/Senha inválida.");

            if (model.Status.Id == 2)
                return new HttpStatusCodeResult((HttpStatusCode)412, "Usuário está inativo.");
            else if (model.Status.Id == 3)
                return new HttpStatusCodeResult((HttpStatusCode)412, "Usuário aguardando analise.");
            else if (model.Status.Id == 4)
                return new HttpStatusCodeResult((HttpStatusCode)403, "Usuário bloqueado.");

            var cookie = new HttpCookie("meal_user")
            {
                Value = new JavaScriptSerializer().Serialize(model),
                Expires = DateTime.Now.AddDays(1)// In the next day it will be created again 
            };
            Response.Cookies.Add(cookie);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}