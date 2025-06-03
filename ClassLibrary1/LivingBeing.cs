using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LivingBeing
    {
        public int CurrentHitPoints { get; set; }
        public int MaxHitPoints { get; set; }

        public LivingBeing(int currentHitPoints, int maxHitPoints)
        {
            CurrentHitPoints = currentHitPoints;
            MaxHitPoints = maxHitPoints;
        }

    }
}
