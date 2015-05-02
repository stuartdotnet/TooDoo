using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using TooDoo.Model;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace TooDoo.Repository
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
 
        }

        public void Seed(ToDoContext context)
        {
            //
            // Add Required User
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            string name = "manager";
            string password = "password";


            var user = new ApplicationUser()
            {
                UserName = name
            };

            var result = UserManager.Create(user, password);

            context.SaveChanges();
        }
    }

    public class CreateInitializer : CreateDatabaseIfNotExists<ToDoContext>
    {
        protected override void Seed(ToDoContext context)
        {
            context.Seed(context);
            //base.Seed(context);
        }
    }

}
