using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class PlayerQuest(Quest details)
    {
        public Quest Details { get; set; } = details;
        public bool IsCompleted { get; set; } 

    }
}
