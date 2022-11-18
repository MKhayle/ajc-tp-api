
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TPPizza.DAL.Model;

namespace TPPizza.DAL
{
    public class PizzaDbContext : DbContext
    {
        public PizzaDbContext()
        {

        } 

        public PizzaDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TPPizza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Pate> Pates { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
