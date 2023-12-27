using GameWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace GameWeb.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUsers>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Games> Games { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<GamesCategories> GamesCategories { get; set; }
    public DbSet<GamesAndCategories> GamesAndCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Comments>()
                .HasOne(a => a.User)
                .WithMany(u => u.CommentsList)
                .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<Comments>()
            .HasOne(a => a.Game)
            .WithMany(t => t.CommentsList)
            .HasForeignKey(a => a.GameId);

        modelBuilder.Entity<Games>()
            .HasMany(e => e.CategoryList)
            .WithMany(e => e.GamesList)
            .UsingEntity<GamesAndCategories>();
    }   
}
