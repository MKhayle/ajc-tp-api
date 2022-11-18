using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TPPizza.DAL.Model;

namespace TPPizza.Web.Models
{
    public class IngredientViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage ="Merci de saisir une valeur")]
        public string Name { get; set; }

        public static IngredientViewModel FromModel(Ingredient ingredient)
        {
            return new() { Id = ingredient.Id, Name = ingredient.Name };
        }

        public static Ingredient ToModel(IngredientViewModel ingredient)
        {
            return new() { Id = ingredient.Id, Name = ingredient.Name };
        }
    }
}
