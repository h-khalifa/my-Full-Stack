using ExO.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ExO.Startup))]

namespace ExO
{
    public partial class Startup
    {
        ApplicationDbContext ctx = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            InitRoles();
        }

        private void InitRoles()
        {
            RoleManager<IdentityRole> roleManager =
                new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));


            if (!roleManager.RoleExists("Student"))
            {
                IdentityRole role = new IdentityRole("Student");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Teacher"))
            {
                IdentityRole role = new IdentityRole("Teacher");
                roleManager.Create(role);
            }
           

        }

    }
}
