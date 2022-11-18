using Microsoft.EntityFrameworkCore;
using System.IO;
using TPPizza.DAL;
using TPPizza.DAL.Model;

namespace TPPizza.Business
{
    public class PizzaService
    {
        private readonly PizzaDbContext _pizzaDbContext;

        public PizzaService(PizzaDbContext pizzaDbContext)
        {
            _pizzaDbContext = pizzaDbContext;
        }

        public List<Ingredient> GetIngredients()
        {
            return _pizzaDbContext.Ingredients.ToList();
        }

        public async Task<Ingredient> GetIngredientAsync(int id)
        {
            return await _pizzaDbContext.Ingredients.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task CreateIngredientAsync(Ingredient ingredient)
        {
            _pizzaDbContext.Ingredients.Add(ingredient);
            await _pizzaDbContext.SaveChangesAsync();
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            _pizzaDbContext.Ingredients.Update(ingredient);
            await _pizzaDbContext.SaveChangesAsync();
        }

        public async Task DeleteIngredientAsync(int id)
        {
            var ingredient = await _pizzaDbContext.Ingredients.FindAsync(id);
            if (ingredient != null)
            {
                _pizzaDbContext.Ingredients.Remove(ingredient);
                await _pizzaDbContext.SaveChangesAsync();
            }
        }

        public List<Pate> GetPates()
        {
            return _pizzaDbContext.Pates.ToList();
        }

        public async Task<Pate> GetPateAsync(int id)
        {
            return await _pizzaDbContext.Pates.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task CreatePateAsync(Pate pate)
        {
            _pizzaDbContext.Pates.Add(pate);
            await _pizzaDbContext.SaveChangesAsync();
        }

        public async Task UpdatePateAsync(Pate pate)
        {
            _pizzaDbContext.Pates.Update(pate);
            await _pizzaDbContext.SaveChangesAsync();
        }

        public async Task DeletePateAsync(int id)
        {
            var pate = await _pizzaDbContext.Pates.FindAsync(id);
            if (pate != null)
            {
                _pizzaDbContext.Pates.Remove(pate);
                await _pizzaDbContext.SaveChangesAsync();
            }
        }

        public List<Pizza> GetPizzas()
        {
            return _pizzaDbContext.Pizzas.Include(pi => pi.Pate).Include(pi => pi.Ingredients).ToList();
        }

        public async Task<Pizza> GetPizzaAsync(int id)
        {
            return await _pizzaDbContext.Pizzas.Include(pi => pi.Pate).Include(pi => pi.Ingredients).FirstOrDefaultAsync(i => i.Id == id);
        }

        // Méthodes web
        public async Task CreatePizzaAsync(Pizza pizza, List<int> ingredientsIds)
        {
            pizza.Ingredients = ingredientsIds.Select(id => _pizzaDbContext.Ingredients.First(i => i.Id == id)).ToList();

            _pizzaDbContext.Pizzas.Add(pizza);
            await _pizzaDbContext.SaveChangesAsync();
        }

        public async Task UpdatePizzaAsync(Pizza pizza, List<int> ingredientsIds)
        {
            var pizzaToEdit = _pizzaDbContext.Pizzas.Include(pi => pi.Ingredients).First(x => x.Id == pizza.Id);
            pizzaToEdit.Ingredients.Clear();
            pizzaToEdit.Ingredients = ingredientsIds.Select(id => _pizzaDbContext.Ingredients.First(i => i.Id == id)).ToList();

            pizzaToEdit.Name = pizza.Name;
            pizzaToEdit.PateId = pizza.PateId;

            await _pizzaDbContext.SaveChangesAsync();
        }

        public async Task DeletePizzaAsync(int id)
        {
            var pizza = await _pizzaDbContext.Pizzas.FindAsync(id);
            if (pizza != null)
            {
                _pizzaDbContext.Pizzas.Remove(pizza);
                await _pizzaDbContext.SaveChangesAsync();
            }
        }

        public bool PizzaExists(string name)
        {
            return _pizzaDbContext.Pizzas.Any(p => p.Name.Equals(name));
        }

        // Surcharges API 
        public bool PizzaExists(int id)
        {
            return _pizzaDbContext.Pizzas.Any(p => p.Id.Equals(id));
        }

        public bool PateExists(int id)
        {
            return _pizzaDbContext.Pates.Any(p => p.Id.Equals(id));
        }

        public bool IngredientExists(int id)
        {
            return _pizzaDbContext.Ingredients.Any(p => p.Id.Equals(id));
        }
    }
}