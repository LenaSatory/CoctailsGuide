using System;
using System.Collections.Generic;

namespace CoctailsGuideWebApplication
{
    public partial class Categories
    {
        public Categories()
        {
            Ingredients = new HashSet<Ingredients>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ingredients> Ingredients { get; set; }
    }
}
