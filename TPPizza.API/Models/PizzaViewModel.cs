namespace TPPizza.API.Models;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TPPizza.DAL.Model;

public class PizzaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Veuillez rentrer un nom")]
    public string Name { get; set; }

    [Display(Name = "Pâte")]
    public int PateId { get; set; }

    [Display(Name = "Ingredients")]
    [Required(ErrorMessage = "Veuillez selectionner entre 2 et 5 ingrédients")]
    public List<int> IngredientsIds { get; set; }

    public static Pizza ToModel(PizzaViewModel pizzaViewModel)
        => new()
        {
            Id = pizzaViewModel.Id,
            Name = pizzaViewModel.Name,
            PateId = pizzaViewModel.PateId
        };

    public static PizzaViewModel FromModel(Pizza pizza)
    => new()
    {
        Id = pizza.Id,
        Name = pizza.Name,
        PateId = pizza.PateId,
        IngredientsIds = pizza.Ingredients.Select(pi => pi.Id).ToList()
    };

}
