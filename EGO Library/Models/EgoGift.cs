using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGO_Library.Models
{
    public class EgoGift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
        public string StatusEffect { get; set; }
        public string Icon { get; set; } 
        public string Description { get; set; } 
        public List<string> Sources { get; set; }
        public List<string> FusionRecipes { get; set; }
    }
}
