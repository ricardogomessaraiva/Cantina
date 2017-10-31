using Models;
using System.Data.Entity;

namespace Contexts
{

    public class MealEntities : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Parent> Parent { get; set; }
        public DbSet<Phone> Phone { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<UserType> Type { get; set; }
        public DbSet<Period> Period { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet <Product> Product { get; set; }

        public MealEntities()
            : base("MealEntities")
        {

        }

        static MealEntities()
        {
            DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

    }
}
