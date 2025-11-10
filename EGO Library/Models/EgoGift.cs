using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        //public List<string> Sources { get; set; }
        public List<string> FusionRecipes { get; set; }

        public EgoGift() { }

        
        public EgoGift(int id, string name, int tier, string status, string description)
        {
            Id = id;
            Name = name;
            Tier = tier;
            StatusEffect = status;
            Description = description;
            Sources = new List<string>();
            FusionRecipes = new List<string>();
        }

    }

}
