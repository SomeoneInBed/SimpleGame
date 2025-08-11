using System.Xml;

namespace Engine
    {
        public class Player : LivingBeing
        {
        public int Gold { get; set; }
        private int experiencePoints;
        public int ExperiencePoints 
        {
            get { return experiencePoints; }
            set
            {
                experiencePoints = value;

                while (experiencePoints >= 100)
                {
                    Level++;
                    ExperiencePoints -= 100;
                }
            }
        }
        public int Level { get; set; } = 1;
        public Location Location { get; set; }
        public Weapon CurrentWeapon { get; set; }
        public List<InventoryItem> InventoryItems { get; set; } = [];
        public List<PlayerQuest> PlayerQuests { get; set; } = [];


        public Player(int gold, int experiencePoints, int currentHitPoints, int maxHitPoints)
        : base(currentHitPoints, maxHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(20, 1, 30, 30);
            player.InventoryItems.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_IRON_SWORD), 1));
            player.Location = World.LocationByID(World.LOCATION_ID_HOME); //set default location to home
            return player;
        }

        public static Player CreatePlayerFromXmlString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new XmlDocument();
                playerData.LoadXml(xmlPlayerData);

                int CurrentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int MaxHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaxHitPoints").InnerText);
                int Gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
                int ExperiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);

                Player player = new Player(Gold, ExperiencePoints, CurrentHitPoints, MaxHitPoints);

                int CurrentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
                player.Location = World.LocationByID(CurrentLocationID);
                
                if (playerData.SelectSingleNode("/Player/Stats/CurrentWeapon") != null) {
                    int weaponID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);
                    player.CurrentWeapon = (Weapon)World.ItemByID(weaponID); //set current weapon if it exists
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);

                    for (int i = 0; i < quantity; i++)
                    {
                        player.AddItem(World.ItemByID(id)); //add item to inventory
                    }
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isComplete = Convert.ToBoolean(node.Attributes["IsComplete"].Value);

                    PlayerQuest quest = new(World.QuestByID(id))
                    {
                        IsCompleted = isComplete
                    };
                    player.PlayerQuests.Add(quest); //add quest to player's quest list
                }

                
                return player;
            }
            catch
            {
                //if there was an error, give the default character 
                return Player.CreateDefaultPlayer();
            }
        }
        public bool HasRequiredItemToEnterLocation(Location location)
        {
            if (location.ItemRequiredToEnter  == null)
            {
                return true; //player does not need item to enter
            }

            return InventoryItems.Exists(i => i.Details.ID == location.ItemRequiredToEnter.ID); //check if player has item required
        }

        public bool HasQuest(Quest quest)
        {
            return PlayerQuests.Exists(pq => pq.Details.ID == quest.ID); //check if player has quest 
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
                if (!InventoryItems.Exists(i => i.Details.ID == qc.Details.ID && i.Quantity >= qc.Quantity))
                {
                    return false; //player does not have all items required for quest completion
                }
            }
            return true; 
        }

        public void RemoveQuestItems(Quest quest)
        {
            foreach (QuestComplete qc in quest.Completion)
            {
                InventoryItem item = InventoryItems.SingleOrDefault(i => i.Details.ID == qc.Details.ID);
                if (item != null)
                {
                    item.Quantity -= qc.Quantity; //reduce quantity of item in inventory
                }
            }
        }

        public void AddItem(Item itemToAdd)
        {
            InventoryItem? item = InventoryItems.SingleOrDefault(i => i.Details.ID == itemToAdd.ID);
            if (item == null)
            {
                InventoryItems.Add(new InventoryItem(itemToAdd, 1)); //if item doesn't exist, add it  
            }
            else
            {
                item.Quantity++;
            }
        }

        public void MarkQuestComplete(Quest quest)
        {
            PlayerQuest playerQuest = PlayerQuests.SingleOrDefault(pq => pq.Details.ID == quest.ID);  
            if (playerQuest != null)
            {
                playerQuest.IsCompleted = true; //mark quest as complete
            }
        }

        public string ToXmlString()
        {
            XmlDocument playerData = new XmlDocument();
            //create top level node
            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);
            //create the "stats" child nodes to hold the other player stat nodes
            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats);

            //create the child nodes for stats
            XmlNode currentHitPoints = playerData.CreateElement("CurrentHitPoints");
            currentHitPoints.AppendChild(playerData.CreateTextNode(this.CurrentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);

            //max hit points node
            XmlNode maxHitPoints = playerData.CreateElement("MaxHitPoints");
            maxHitPoints.AppendChild(playerData.CreateTextNode(this.MaxHitPoints.ToString()));
            stats.AppendChild(maxHitPoints);

            //gold node
            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(this.Gold.ToString()));
            stats.AppendChild(gold);

            //experience points node
            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(playerData.CreateTextNode(this.ExperiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            //current location node
            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(this.Location.ID.ToString()));
            stats.AppendChild(currentLocation);
            
            if (CurrentWeapon != null)
            {
                XmlNode currentWeapon = playerData.CreateElement("CurrentWeapon");
                currentWeapon.AppendChild(playerData.CreateTextNode(CurrentWeapon.ID.ToString()));
                stats.AppendChild(currentWeapon);
            }

            //inventoryItems node
            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);
            //create a node for each inventory item
            foreach (InventoryItem i in this.InventoryItems)
            {
                XmlNode inventoryItem = playerData.CreateElement("InventoryItem");
                //id attribute for items
                XmlAttribute itemID = playerData.CreateAttribute("ID");
                itemID.Value = i.Details.ID.ToString();
                inventoryItem.Attributes.Append(itemID);
                //quantity attribute for items
                XmlAttribute quantity = playerData.CreateAttribute("Quantity");
                quantity.Value = i.Quantity.ToString();
                inventoryItem.Attributes.Append(quantity);
                inventoryItems.AppendChild(inventoryItem);
            }

            //player quest node
            XmlNode playerQuests = playerData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);
            //the node for each quest
            foreach (PlayerQuest q in playerQuests)
            {
                XmlNode playerQuest = playerData.CreateElement("PlayerQuest");
                //id attribute
                XmlAttribute questID = playerData.CreateAttribute("ID");
                questID.Value = q.Details.ID.ToString(); 
                playerQuest.Attributes.Append(questID);
                //is complete attribute 
                XmlAttribute isComplete = playerData.CreateAttribute("IsComplete");
                isComplete.Value = q.IsCompleted.ToString();
                playerQuest.Attributes.Append(isComplete);
                playerQuests.AppendChild(playerQuest);
            }

            
            return playerData.InnerXml; //return the XML string representation of the player data
        }
    }
}
