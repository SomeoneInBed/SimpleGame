using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Monster : LivingBeing
    {
        public int ID { get; set; }
        public int MaxDamage { get; set; }
        public int RewardEXP { get; set; }
        public int RewardGold { get; set; }
        public string Name { get; set; }
        public List<LootItem> LootTable { get; set; } = [];
        public Monster(int id, string name, int currentHitPoints, int maxHitPoints, int maxDamage, int rewardXP, int rewardGold)
            : base(currentHitPoints, maxHitPoints)
        {
            ID = id;
            MaxDamage = maxDamage;
            RewardEXP = rewardXP;
            RewardGold = rewardGold;
            Name = name;
        }

    }
}
