using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AskFm.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        #region custom props

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string biography { get; set; }

        public Gender Gender { get; set; }

        //nav props
        public ProfilePhoto Photo { get; set; }
        virtual public List<Notification> Notifications { get; set; }

        [InverseProperty("AskedBy")]
        virtual public List<Question> Questions_By { get; set; }
        [InverseProperty("AskedTo")]
        virtual public List<Question> Questions_To { get; set; }

        [InverseProperty("AnsweredBy")]
        virtual public List<Answer> Answers_By { get; set; }
        [InverseProperty("AskedBy")]
        virtual public List<Answer> Answers_To { get; set; }

        [InverseProperty("Followed")]
        virtual public List<Follow> Followers { get; set; }
        [InverseProperty("Follower")]
        virtual public List<Follow> Following { get; set; }

        virtual public List<Like> Likes { get; set; }

        #endregion


    }
    public enum Gender
    {
        Male,
        Female
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<ProfilePhoto> profilePhotos { get; set; }
        public DbSet<Answer_Photo> answerPhotos { get; set; }
        public DbSet<Notification> notifications { get; set; }
       
    }
}