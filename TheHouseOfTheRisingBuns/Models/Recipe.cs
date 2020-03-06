using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheHouseOfTheRisingBuns.Models
{
    public class Recipe
    {

        public int ID { get; set; }

        public int CategoryID { get; set; }
        [Required]
        public string Title { get; set; }


        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
        [Required]
        public int Servings { get; set; }
        [Required]
        public string Ingredients { get; set; }
        [Required]
        public string Method { get; set; }


        [Display(Name = "Recipe Image")]
        public string RecipeThumb { get; set; }


        public Category CategoryName { get; set; }
        public ICollection<Category> Categories { get; set; }

        //public string Caption { get; set; }
        //public string Description { get; set; }
        //public IFormFile MyImage { get; set; }
    }
}