namespace Contexts.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Contexts.MealEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }

        protected override void Seed(Contexts.MealEntities context)
        {
            const int USER_TYPE_DEVELOPER = 1;
            const int USER_TYPE_ADMINISTRADOR = 2;
            const int USER_TYPE_USER = 3;

            /**
             * 
             * * ADDING FIRSTS DATAS
             */

            //context.Status.Add(new Status { Description = "Ativo" });
            //context.Status.Add(new Status { Description = "Inativo" });
            //context.Status.Add(new Status { Description = "Aguardando analise" });
            //context.Status.Add(new Status { Description = "Bloqueado" });

            //context.Type.Add(new UserType { Name = "Desenvolvedor" });
            //context.Type.Add(new UserType { Name = "Administrador" });
            //context.Type.Add(new UserType { Name = "Usuário" });

            //context.Period.Add(new Period { Name = "Matinal" });
            //context.Period.Add(new Period { Name = "Vespertino" });
            //context.Period.Add(new Period { Name = "Noturno" });
            //context.Period.Add(new Period { Name = "Integral" });

            /**
            * 
            * * ADDING FIRST USER AS AN DEVELOPER AND ADMIN
            */

            string hashedPass = BCrypt.Net.BCrypt.HashPassword("ricardo");
            context.User.Add(new User
            {
                Name = "Ricardo Gomes Sariava",
                UserName = "ricardo.saraiva",
                Password = hashedPass,
                Email = "ricardogomessaraiva@hotmail.com",
                Type = context.Type.First(x => x.Id == USER_TYPE_DEVELOPER),
                Status = context.Status.First(x => x.Id == 1)
            });

            hashedPass = BCrypt.Net.BCrypt.HashPassword("admin");
            context.User.Add(new User
            {
                Name = "Administrador",
                UserName = "admin",
                Password = hashedPass,
                Email = "ricardogomessaraiva@hotmail.com",
                Type = context.Type.First(x => x.Id == USER_TYPE_ADMINISTRADOR),
                Status = context.Status.First(x => x.Id == 1)
            });
            
        }
    }
}
