using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TPPizza.DAL.Model;

namespace TPPizza.Web.Models
{
    public class PateViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage ="Merci de saisir une valeur")]
        public string Name { get; set; }

        public static PateViewModel FromModel(Pate Pate)
        {
            return new() { Id = Pate.Id, Name = Pate.Name };
        }

        public static Pate ToModel(PateViewModel Pate)
        {
            return new() { Id = Pate.Id, Name = Pate.Name };
        }
    }
}
