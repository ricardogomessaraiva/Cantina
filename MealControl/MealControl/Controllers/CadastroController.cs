using MealControl.Models;
using Models;
using Services;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MealControl.Controllers
{
    public class CadastroController : BaseController
    {
        public UserService service = new UserService();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Load()
        {          
            return Json(new
            {         
                period = entity.Period.ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(Parent parent)
        {
            try
            {
                if (parent.Status == null)
                {
                    //New User Status id 3 = Waiting evaluation
                    parent.Status = entity.Status.FirstOrDefault(s => s.Id == 3);
                }

                ////user type 3 = user            
                //user.Type = entity.Type.FirstOrDefault(s => s.Id == 3);

                //Set user period by period description
                parent.Students.ForEach(s =>
                {
                    if (s.Period != null)
                        s.Period = entity.Period.First(p => p.Id == s.Period.Id);
                });

                var errors = service.Validate(parent);
                if (errors.Count() > 0)
                    return Json(errors);

                if (parent.Id != 0)
                    parent.Password = entity.Parent.First(s => s.Id == parent.Id).Password;
                else
                    parent.Password = BCrypt.Net.BCrypt.HashPassword(parent.Password);

                //if (parent.Status.Id == 1)
                //{
                //    entity.Parent.Attach(parent);
                //    entity.Entry(parent).State = System.Data.Entity.EntityState.Modified;
                //    entity.SaveChanges();
                //    return new HttpStatusCodeResult(HttpStatusCode.Accepted);//202            
                //}

                parent = service.Save(parent);                

                //Send email to all admins advising about the new user waiting for their evaluation
                var subject = "Novo usuário cadastrado - Aguardando liberação";
                var message = System.IO.File.ReadAllText(Server.MapPath("~/Mail.html")).Replace("##NAME##", parent.Name);
                var admins = entity.User.Where(s => s.Type.Id == 2).ToList();

                admins.ForEach(s =>
                {
                    var email = new Email { Mailto = s.Email, Subject = subject, Body = message };

                    try
                    {
                        new Mailer(email).Send();
                    }
                    catch (Exception ex)
                    {
                        //One fail occurred when send the email to the user admin in a row.                        
                    }

                });

                return new HttpStatusCodeResult(HttpStatusCode.Created);//201                
            }
            catch (Exception ex)
            {
                //Something wrong was caught when trying to save user
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                    "Uma falha ocorreu ao salvar os dados do cadastro."); //500                
            }
        }
    }
}