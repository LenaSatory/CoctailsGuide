using System;
using System.Collections.Generic;

namespace CoctailsGuideWebApplication
{
    public partial class Glass
    {
        public Glass()
        {
            Coctails = new HashSet<Coctails>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Coctails> Coctails { get; set; }
    }
}
