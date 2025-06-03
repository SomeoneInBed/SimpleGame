using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LootItem
    { 
        public Item Details { get; set; }
        public int DropRate { get; set; }
        public bool IsDefault { get; set; }

        public LootItem(Item details, int dropRate, bool isDefault)
        {
            Details = details;
            DropRate = dropRate;
            IsDefault = isDefault;
        }
    }
}
