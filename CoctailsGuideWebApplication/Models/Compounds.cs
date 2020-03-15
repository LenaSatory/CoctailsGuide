using System;
using System.Collections.Generic;

namespace CoctailsGuideWebApplication
{
    public partial class Compounds
    {
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public string Amount { get; set; }
        public int CoctailId { get; set; }

        public virtual Coctails Coctail { get; set; }
        public virtual Ingredients Ingredient { get; set; }
    }
}
