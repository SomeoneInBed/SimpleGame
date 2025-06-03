using System.Net.Security;

namespace Engine
{
    public class Player : LivingBeing
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location Location { get; set; }
        public List<InventoryItem> InventoryItems { get; set; } = [];
        public List<PlayerQuest> PlayerQuests { get; set; } = [];


        public Player(int gold, int experiencePoints, int level, int currentHitPoints, int maxHitPoints)
        : base(currentHitPoints, maxHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
        }

        public bool HasRequiredItemToEnterLocation(Location location)
        {
            if (location.ItemRequiredToEnter  == null)
            {
                return true; //player does not need item to enter
            }

            foreach (InventoryItem i in InventoryItems)
            {
                if (i.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    return true; //player has required item
                }
            }

            return false; //player does not have required item
        }

        public bool HasQuest(Quest quest)
        {
            foreach (PlayerQuest pq in PlayerQuests)
            {
                if (pq.Details.ID == quest.ID)
                {
                    return true; //player has the quest
                }
            }
            return false; 
        }

        public bool IsQuestComplete(Quest quest)
        {
            foreach (PlayerQuest pq in PlayerQuests)
            {
                if (pq.Details.ID == quest.ID)
                {
                    return pq.IsCompleted;
                }
            }
            return false; //quest not complete
        }

        public bool HasAllCompletionItems(Quest quest)
        {
            foreach(QuestComplete qc in quest.Completion)
            {
                bool foundItem = false;

                foreach (InventoryItem i in InventoryItems)
                {
                    if (i.Details.ID == qc.Details.ID)
                    {
                        foundItem = true;
                        if (i.Quantity < qc.Quantity)
                        {
                            return false;
                        }
                    }
                }

                if (!foundItem)
                {
                    return false;
                }
            }
            return true; 
        }

        public void RemoveQuestItems(Quest quest)
        {
            foreach (QuestComplete qc in quest.Completion)
            {
                foreach(InventoryItem i in InventoryItems)
                {
                    if(i.Details.ID == qc.Details.ID)
                    {
                        i.Quantity -= qc.Quantity;
                        break;
                    }
                }
            }
        }

        public void AddItem(Item itemToAdd)
        {
            foreach(InventoryItem i in InventoryItems)
            {
                if (i.Details.ID == itemToAdd.ID)
                {
                    i.Quantity++;
                    return; 
                }
                   
            }

            InventoryItems.Add(new InventoryItem(itemToAdd, 1));
        }

        public void MarkQuestComplete(Quest quest)
        {
            foreach(PlayerQuest pq in PlayerQuests)
            {
                if (pq.Details.ID == quest.ID)
                {
                    pq.IsCompleted = true;
                    return; 
                }
            }    
        }
    }
}
