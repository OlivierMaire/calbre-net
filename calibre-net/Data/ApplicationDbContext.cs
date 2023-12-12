using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace calibre_net.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{

    public virtual DbSet<UserCredential> UserCredentials { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.UserId , l.LoginProvider } );
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId});
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new {t.UserId, t.LoginProvider });

        // modelBuilder.Entity<UserCredential>().OwnsOne(
        //     customer => customer.Descriptor, ownedNavigationBuilder =>
        //     {
        //         ownedNavigationBuilder.ToJson();
        //         // ownedNavigationBuilder.OwnsOne( a => a.Id).
        //         // ownedNavigationBuilder.OwnsOne(contactDetails => contactDetails.);
        //     });
    }
}


