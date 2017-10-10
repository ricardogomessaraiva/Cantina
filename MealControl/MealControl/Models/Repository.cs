using Contexts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealControl.Models
{
    public class Repository
    {
        public MealEntities Entity { get; set; }

        public Parent GetUser(Parent user)
        {
            return Entity.Parent.FirstOrDefault(u => u.UserName.Equals(user.UserName) && u.Password.Equals(user.Password));
        }

        public List<Period> Period()
        {
            return Entity.Period.ToList();
        }
    }
}