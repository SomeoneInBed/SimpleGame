using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class InventoryItem(Item details, int quantity)
    {
        public Item Details { get; set; } = details;
        public int Quantity { get; set; } = quantity; 


    }
}
