using System;
using System.Collections.Generic;

namespace CoctailsGuideWebApplication
{
    public partial class Country
    {
        public Country()
        {
            Coctails = new HashSet<Coctails>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Coctails> Coctails { get; set; }
    }
}
