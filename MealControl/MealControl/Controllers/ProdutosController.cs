using Contexts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MealControl.Controllers
{
    public class ProdutosController : Controller
    {
        // GET: Produtos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save()
        {            
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}