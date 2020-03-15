using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoctailsGuideWebApplication
{
    public partial class Ingredients
    {
        public Ingredients()
        {
            Compounds = new HashSet<Compounds>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Категорія")]
        public virtual Categories Category { get; set; }
        [Display(Name = "Склад")]
        public virtual ICollection<Compounds> Compounds { get; set; }
    }
}
