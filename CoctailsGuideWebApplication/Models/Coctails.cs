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
        public string Name { get; set; }
        //[Range(0, , ErrorMessage = "kndv")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        //[Display(Name = "Year of creation")]
        public string YearofCreation { get; set; }
        //[Display(Name = "Country")]
        public int CountryofCreationId { get; set; }
        //[Display(Name = "Creation history")]
        public string CreationHistory { get; set; }
        //[Display(Name = "Strength")]
        public int StrengthId { get; set; }
        //[Display(Name = "Technique")]
        public int TechniqueId { get; set; }
        //[Display(Name = "Glass")]
        public int GlassId { get; set; }
        public string Recipe { get; set; }

        //[Display(Name = "Country")]
        public virtual Country CountryofCreation { get; set; }
        //[Display(Name = "Glass")]
        public virtual Glass Glass { get; set; }
        public virtual Strengths Strength { get; set; }
        //[Display(Name = "Technique")]
        public virtual Techniques Technique { get; set; }
        //[Display(Name = "Compound")]
        public virtual ICollection<Compounds> Compounds { get; set; }
    }
}
