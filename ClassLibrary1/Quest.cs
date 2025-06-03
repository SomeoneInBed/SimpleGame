using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Quest(int id, string name, string description, int rewardxp, int rewardGold, Item? rewardItem = null)
    {
        public int ID { get; set; } = id;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public int RewardEXP { get; set; } = rewardxp;
        public int RewardGold { get; set; } = rewardGold;
        public Item? RewardItem { get; set; } = rewardItem;
        public List<QuestComplete> Completion { get; set; } = [];

    }
}
