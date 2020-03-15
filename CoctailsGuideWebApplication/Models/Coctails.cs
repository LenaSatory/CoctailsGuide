using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoctailsGuideWebApplication
{
    public partial class Coctails
    {
        public Coctails()
        {
            Compounds = new HashSet<Compounds>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім)")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        //[Range(0, , ErrorMessage = "kndv")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Рік створення")]
        public string YearofCreation { get; set; }
        [Display(Name = "Країна")]
        public int CountryofCreationId { get; set; }
        [Display(Name = "Історія створення")]
        public string CreationHistory { get; set; }
        [Display(Name = "Міцність")]
        public int StrengthId { get; set; }
        [Display(Name = "Спосіб приготування")]
        public int TechniqueId { get; set; }
        [Display(Name = "Бокал подачі")]
        public int GlassId { get; set; }
        [Display(Name = "Рецепт")]
        public string Recipe { get; set; }

        [Display(Name = "Країна")]
        public virtual Country CountryofCreation { get; set; }
        [Display(Name = "Бокал подачі")]
        public virtual Glass Glass { get; set; }
        [Display(Name = "Міцність")]
        public virtual Strengths Strength { get; set; }
        [Display(Name = "Спосіб приготування")]
        public virtual Techniques Technique { get; set; }
        [Display(Name = "Склад")]
        public virtual ICollection<Compounds> Compounds { get; set; }
    }
}
