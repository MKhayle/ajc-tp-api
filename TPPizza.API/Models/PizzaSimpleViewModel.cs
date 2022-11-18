namespace TPPizza.API.Models;

using TPPizza.DAL.Model;

public class PizzaSimpleViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public PateViewModel Pate { get; set; }

    public List<IngredientViewModel> Ingredients { get; set; }

    public string IngredientsDisplay => string.Join(", ", this.Ingredients.Select(i => i.Name));

    public static PizzaSimpleViewModel FromModel(Pizza pizza)
        => new()
        {
            Id = pizza.Id,
            Name = pizza.Name,
            Pate = PateViewModel.FromModel(pizza.Pate),
            Ingredients = pizza.Ingredients.Select(IngredientViewModel.FromModel).ToList()
        };
}
