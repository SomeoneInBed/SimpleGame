using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Weapon : Item
    {
        public string? Description { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int? Price { get; set; }

        public Weapon(int id, string name, string namePlural, string? description, int minDamage, int maxDamage, int? price)
        : base(id, name, namePlural)
        {
            Description = description;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Price = price; 
        }
    }
}
