using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace MealControl.Models
{
    public class Email
    {        
        public string Mailto { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }        
    }
}