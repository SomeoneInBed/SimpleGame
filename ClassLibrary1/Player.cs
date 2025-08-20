using System.Xml;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;

namespace Engine
{
    public class Player : LivingBeing
    {
        private int gold;
        private Monster _monster;
        private Location _location;
        public EventHandler<MessageEventArgs> OnMessage;
        public int Gold
        {
            get { return gold; }
            set
            {
                gold = value;
                OnPropertyChange("Gold");
            }
        }
        private int experiencePoints;
        public int ExperiencePoints
        {
            get { return experiencePoints; }
            private set
            {
                experiencePoints = value;

                while (experiencePoints >= 100)
                {
                    Level++;
                    ExperiencePoints -= 100;
                    MaxHitPoints += 10;
                }
                OnPropertyChange("ExperiencePoints");
                OnPropertyChange("Level");
            }
        }
        public int Level { get; set; } = 1;
        public Location Location { get { return _location; }
            set
            {
                _location = value;
                OnPropertyChange("Location");
            }
        }
        public Weapon CurrentWeapon { get; set; }
        public HealingSpell CurrentPotion { get; set; }
        public BindingList<InventoryItem> InventoryItems { get; set; } = [];
        public BindingList<PlayerQuest> PlayerQuests { get; set; } = [];
        public List<Weapon> Weapons => [.. InventoryItems.Where(i => i.Details is Weapon).Select(i => (Weapon)i.Details)];
        public List<HealingSpell> HealingPotions => [.. InventoryItems.Where(i => i.Details is HealingSpell).Select(ii => (HealingSpell)ii.Details)];


        public Player(int gold, int experiencePoints, int currentHitPoints, int maxHitPoints, int level = 1)
        : base(currentHitPoints, maxHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
        }

        private void RaiseInventoryChangedEvent(Item item)
        {
            if (item is Weapon)
            {
                OnPropertyChange("Weapons");
            }
            else if (item is HealingSpell)
            {
                OnPropertyChange("HealingPotions");
            }
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(20, 1, 30, 30);
            player.InventoryItems.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_IRON_SWORD), 1));
            player.Location = World.LocationByID(World.LOCATION_ID_HOME); //set default location to home
            return player;
        }

        

        public void addExperiencePoints(int amount)
        {
            ExperiencePoints += amount;
        }
        public bool HasRequiredItemToEnterLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
            {
                return true; //player does not need item to enter
            }

            return InventoryItems.Any(i => i.Details.ID == location.ItemRequiredToEnter.ID); //check if player has item required
        }

        public bool HasQuest(Quest quest)
        {
            return PlayerQuests.Any(pq => pq.Details.ID == quest.ID); //check if player has quest 
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
            foreach (QuestComplete qc in quest.Completion)
            {
                if (!InventoryItems.Any(i => i.Details.ID == qc.Details.ID && i.Quantity >= qc.Quantity))
                {
                    return false; //player does not have all items required for quest completion
                }
            }
            return true;
        }

        public void RemoveQuestItems(Quest quest) { 
            foreach (QuestComplete qc in quest.Completion)
            {
                InventoryItem item = InventoryItems.SingleOrDefault(i => i.Details.ID == qc.Details.ID);
                if (item != null)
                {
                    RemoveItem(item.Details, qc.Quantity); //reduce quantity of item in inventory
                }
            }
        }

        public void AddItem(Item itemToAdd, int quantity = 1)
        {
            InventoryItem item = InventoryItems.SingleOrDefault(item => item.Details.ID == itemToAdd.ID);
            if (item == null)
            {
                InventoryItems.Add(new InventoryItem(itemToAdd, quantity));
            }
            else
            {
                item.Quantity += quantity;
            }
            RaiseInventoryChangedEvent(itemToAdd);
        }
            
        public void MarkQuestComplete(Quest quest)
        {
            PlayerQuest playerQuest = PlayerQuests.SingleOrDefault(pq => pq.Details.ID == quest.ID);
            if (playerQuest != null)
            {
                playerQuest.IsCompleted = true; //mark quest as complete
            }
        }

        public void RemoveItem(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = InventoryItems.SingleOrDefault(i => i.Details.ID == itemToRemove.ID);
            if(item == null)
            {
                return;
            }
            else
            {
                item.Quantity -= quantity; //reduce quantity of item in inventory
                if (item.Quantity == 0)
                {
                    InventoryItems.Remove(item); //remove item from inventory if quantity is 0 or less
                }
                RaiseInventoryChangedEvent(itemToRemove);

            }
        }

        public void MoveNorth()
        {
            if(Location.LocationToNorth != null)
            {
                moveTo(Location.LocationToNorth);
            }
        }
        public void MoveSouth()
        {
            if (Location.LocationToSouth != null)
            {
                moveTo(Location.LocationToSouth);
            }
        }
        public void MoveEast()
        {
            if(Location.LocationToEast != null)
            {
                moveTo(Location.LocationToEast);
            }
        }
        public void MoveWest()
        {
            if (Location.LocationToWest != null)
            {
                moveTo(Location.LocationToWest);
            }
        }
        public void MoveHome()
        {
            moveTo(World.LocationByID(World.LOCATION_ID_HOME));
        }

        public void moveTo(Location newLocation)
        {
            // If player goes home, health is restored
            if (newLocation == World.LocationByID(World.LOCATION_ID_HOME))
            {
               CurrentHitPoints = MaxHitPoints;
            }

            // Check if item is required to enter
            if (!HasRequiredItemToEnterLocation(newLocation))
            {
                RaiseMessage("You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter here." + Environment.NewLine);
                return; // Exit the method if player does not have the required item
            }

            Location = newLocation;

            //check for quest 
            if (newLocation.QuestAvailableHere != null)
            {
                //see if player has completed quest
                bool playerAlreadyHasQuest = HasQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = IsQuestComplete(newLocation.QuestAvailableHere);

                foreach (PlayerQuest quest in PlayerQuests)
                {
                    if (quest.Details.ID == newLocation.QuestAvailableHere.ID)
                    {
                        playerAlreadyHasQuest = true;
                        if (quest.IsCompleted)
                        {
                            playerAlreadyHasQuest = true;
                        }
                    }
                }

                //see if player has quest
                if (playerAlreadyHasQuest)
                {
                    //player has not completed quest
                    if (!playerAlreadyCompletedQuest)
                    {
                        bool playerHasAllItems = HasAllCompletionItems(newLocation.QuestAvailableHere);


                        if (playerHasAllItems == true)
                        {
                            RaiseMessage("");
                            RaiseMessage("You complete the " + newLocation.QuestAvailableHere.Name + " quest.");

                            //remove quest items
                            RemoveQuestItems(newLocation.QuestAvailableHere);

                            //give reward
                            RaiseMessage("You receive: " + Environment.NewLine);
                            RaiseMessage(newLocation.QuestAvailableHere.RewardEXP.ToString() + " experience points");
                            RaiseMessage(newLocation.QuestAvailableHere.RewardGold.ToString() + " gold");
                            RaiseMessage(newLocation.QuestAvailableHere.RewardItem.Name);
                            RaiseMessage("");
                            addExperiencePoints(newLocation.QuestAvailableHere.RewardEXP);
                            Gold += newLocation.QuestAvailableHere.RewardGold;


                            AddItem(newLocation.QuestAvailableHere.RewardItem);

                            MarkQuestComplete(newLocation.QuestAvailableHere);

                        }
                    }
                }

                else
                {
                    //player does not have the quest
                    //display messsages
                    RaiseMessage($"New quest available: {newLocation.QuestAvailableHere.Name}");
                    RaiseMessage(newLocation.QuestAvailableHere.Description);
                    RaiseMessage("To complete this quest, you must return with: ");
                    foreach (QuestComplete qc in newLocation.QuestAvailableHere.Completion)
                    {
                        if (qc.Quantity == 1)
                        {
                            RaiseMessage(qc.Quantity.ToString() + " " + qc.Details.Name);
                        }

                        else
                        {
                            RaiseMessage(qc.Quantity.ToString() + " " + qc.Details.NamePlural);
                        }
                    }
                    RaiseMessage("");

                    PlayerQuests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            //check for monster
            if (newLocation.MonsterLivingHere != null)
            {
                //Tell the player that the monster is here
                RaiseMessage($"You see a {newLocation.MonsterLivingHere.Name} here.");
                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID)!;

                _monster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.CurrentHitPoints, standardMonster.MaxHitPoints, standardMonster.MaxDamage, standardMonster.RewardEXP, standardMonster.RewardGold);

                foreach (LootItem li in standardMonster.LootTable)
                {
                    _monster.LootTable.Add(li);
                }
            }
            else
            {
                _monster = null;
            }
        }

        private void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            if (OnMessage != null)
            {
                OnMessage(this, new MessageEventArgs(message, addExtraNewLine));
            }
        }

        public void UseWeapon(Weapon currentWeapon)
        {
            int damageToMonster = RandomNumberGenerator.RandomNumber(currentWeapon.MinDamage, currentWeapon.MaxDamage);

            //apply damage
            _monster.CurrentHitPoints -= damageToMonster;

            RaiseMessage("You hit the " + _monster.Name + " for " + damageToMonster + " points.");

            // Check if the monster is dead
            if (_monster.CurrentHitPoints <= 0)
            {
                // Monster is dead
                RaiseMessage("");
                RaiseMessage("You defeated the " + _monster.Name);

                // Give player experience points for killing the monster
                addExperiencePoints(_monster.RewardEXP);
                RaiseMessage("You receive " + _monster.RewardEXP + " experience points");

                // Give player gold for killing the monster 
                Gold += _monster.RewardGold;
                RaiseMessage("You receive " + _monster.RewardGold + " gold");

                // Get random loot items from the monster
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                // Add items to the lootedItems list, comparing a random number to the drop percentage
                foreach (LootItem lootItem in _monster.LootTable)
                {
                    if (RandomNumberGenerator.RandomNumber(1, 100) <= lootItem.DropRate)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }

                // If no items were randomly selected, then add the default loot item(s).
                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _monster.LootTable)
                    {
                        if (lootItem.IsDefault)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }
                // Add the looted items to the player's inventory
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    AddItem(inventoryItem.Details);

                    if (inventoryItem.Quantity == 1)
                    {
                        RaiseMessage("You loot " + inventoryItem.Quantity + " " + inventoryItem.Details.Name);
                    }
                    else
                    {
                        RaiseMessage("You loot " + inventoryItem.Quantity + " " + inventoryItem.Details.NamePlural);
                    }
                }

                // Add a blank line to the messages box, just for appearance.
                RaiseMessage("");

                // Move player to current location (to heal player and create a new monster to fight)
                moveTo(Location);
            }
            else
            {
                // Monster is still alive

                // Determine the amount of damage the monster does to the player
                int damageToPlayer = RandomNumberGenerator.RandomNumber(0, _monster.MaxDamage);

                // Display message
                RaiseMessage("The " + _monster.Name + " did " + damageToPlayer + " points of damage.");

                // Subtract damage from player
                CurrentHitPoints -= damageToPlayer;

                if (CurrentHitPoints <= 0)
                {
                    // Display message
                    RaiseMessage("The " + _monster.Name + " killed you.");

                    // Move player to "Home"
                    MoveHome();
                }
            }
        }

        public void UsePotion(HealingSpell potion)
        {
            // Add healing amount to the player's current hit points
            CurrentHitPoints += potion.AmountToHeal;

            // CurrentHitPoints cannot exceed player's MaximumHitPoints
            if (CurrentHitPoints > MaxHitPoints)
            {
                CurrentHitPoints = MaxHitPoints;
            }

            // Remove the potion from the player's inventory
            RemoveItem(potion, 1);

            // Display message
            RaiseMessage("You drink a " + potion.Name);

            // Monster gets their turn to attack

            // Determine the amount of damage the monster does to the player
            int damageToPlayer = RandomNumberGenerator.RandomNumber(0, _monster.MaxDamage);

            // Display message
            RaiseMessage("The " + _monster.Name + " did " + damageToPlayer + " points of damage.");

            // Subtract damage from player
            CurrentHitPoints -= damageToPlayer;

            if (CurrentHitPoints <= 0)
            {
                // Display message
                RaiseMessage("The " + _monster.Name + " killed you.");

                // Move player to "Home"
                MoveHome();
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

            //level node
            XmlNode level = playerData.CreateElement("Level");
            level.AppendChild(playerData.CreateTextNode(this.Level.ToString()));
            stats.AppendChild(level);

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
            foreach (PlayerQuest q in this.PlayerQuests)
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
                int Level = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Level").InnerText);

                Player player = new Player(Gold, ExperiencePoints, CurrentHitPoints, MaxHitPoints, Level);

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
    } 
}
