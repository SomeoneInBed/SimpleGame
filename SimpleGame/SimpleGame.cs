using Engine;

namespace SimpleGame
{
    public partial class SimpleGame : Form
    {
        private const string V = " ";
        private Player _player; //new player + stats
        private Monster _monster;
        public SimpleGame()
        {
            InitializeComponent();

            _player = new Player(20, 1, 1, 30, 30);

            var ironSword = World.ItemByID(World.ITEM_ID_IRON_SWORD);
            _player.InventoryItems.Add(new InventoryItem(ironSword, 1));// Add an iron sword to the player's inventory

            //labels

            label1.Text = _player.CurrentHitPoints.ToString();
            label2.Text = _player.Gold.ToString();
            label3.Text = _player.ExperiencePoints.ToString();
            label4.Text = _player.Level.ToString();

            //set the starting location
            moveTo(World.LocationByID(World.LOCATION_ID_HOME));

        }
        private void btnNorth_Click(Object sender, EventArgs e)
        {
            moveTo(_player.Location.LocationToNorth);
        }

        private void btnEast_Click(Object sender, EventArgs e)
        {
            moveTo(_player.Location.LocationToEast);
        }

        private void btnWest_Click(Object sender, EventArgs e)
        {
            moveTo(_player.Location.LocationToWest);
        }

        private void btnSouth_Click(Object sender, EventArgs e)
        {
            moveTo(_player.Location.LocationToSouth);
        }

        private void moveTo(Location newLocation)
        {
            if(newLocation == World.LocationByID(World.LOCATION_ID_HOME))
            {
                _player.CurrentHitPoints = _player.MaxHitPoints;
                label1.Text = _player.CurrentHitPoints.ToString();
            }
            //check if item is required to enter
            if (!_player.HasRequiredItemToEnterLocation(newLocation))
            {
                rtbMessages.Text = "You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter here." + Environment.NewLine;
                return; //exit the method if player does not have required item
            }

            _player.Location = newLocation;

            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;


            if (newLocation.QuestAvailableHere != null)
            {
                //see if player has completed quest
                bool playerAlreadyHasQuest = _player.HasQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = _player.IsQuestComplete(newLocation.QuestAvailableHere);

                foreach (PlayerQuest quest in _player.PlayerQuests)
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
                        bool playerHasAllItems = _player.HasAllCompletionItems(newLocation.QuestAvailableHere);


                        if (playerHasAllItems == true)
                        {
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine;

                            //remove quest items
                            _player.RemoveQuestItems(newLocation.QuestAvailableHere);

                            //give reward
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardEXP.ToString() + " experience points" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardEXP;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;


                            _player.AddItem(newLocation.QuestAvailableHere.RewardItem);

                            _player.MarkQuestComplete(newLocation.QuestAvailableHere);
                            _player.RemoveQuestItems(newLocation.QuestAvailableHere);

                        }
                    }
                }

                else
                {
                    //player does not have the quest
                    //display messsages
                    rtbMessages.Text += $"New quest available: {newLocation.QuestAvailableHere.Name}" + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "To complete this quest, you must return with: " + Environment.NewLine;
                    foreach (QuestComplete qc in newLocation.QuestAvailableHere.Completion)
                    {
                        if (qc.Quantity == 1)
                        {
                            rtbMessages.Text += qc.Quantity.ToString() + " " + qc.Details.Name + Environment.NewLine;
                        }

                        else
                        {
                            rtbMessages.Text += qc.Quantity.ToString() + " " + qc.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;

                    _player.PlayerQuests.Add(new PlayerQuest(newLocation.QuestAvailableHere, false));

                }
            }

            //check for monster
            if (newLocation.MonsterLivingHere != null)
            {
                rtbMessages.Text += $"You see a {newLocation.MonsterLivingHere.Name} here." + Environment.NewLine;

                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID)!;

                _monster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.CurrentHitPoints, standardMonster.MaxHitPoints, standardMonster.MaxDamage, standardMonster.RewardEXP, standardMonster.RewardGold);

                foreach (LootItem li in standardMonster.LootTable)
                {
                    _monster.LootTable.Add(li);
                }

                cboWeapons.Visible = true;
                cboPotions.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }

            else
            {
                _monster = null;

                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }

            UpdateInventory();
            UpdateQuestsList();
            UpdateWeaponsList();
            UpdatePotionsList();

        }
        private void btnUseWeapon_Click(Object sender, EventArgs e)
        {
            //grab the current weapon
            Weapon weapon = (Weapon)cboWeapons.SelectedItem;
            //determine amount of damage
            int damageToMonster = RandomNumberGenerator.RandomNumber(weapon.MinDamage, weapon.MaxDamage);
            //apply damage to monster
            _monster.CurrentHitPoints -= damageToMonster;
            rtbMessages.Text += "You hit the " + _monster.Name + " for " + damageToMonster.ToString() + " points." + Environment.NewLine;
            if (_monster.CurrentHitPoints <= 0)
            {
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "You defeated the " + _monster.Name + Environment.NewLine;
                _player.ExperiencePoints += _monster.RewardEXP;
                rtbMessages.Text += "You receive " + _monster.RewardEXP.ToString() + " experience points." + Environment.NewLine;

                _player.Gold += _monster.RewardGold;
                rtbMessages.Text += "You receive " + _monster.RewardGold.ToString() + " gold." + Environment.NewLine;

                List<InventoryItem> Loot = new();

                //add items based on drop percentage
                foreach (LootItem lootItem in _monster.LootTable)
                {
                    if (RandomNumberGenerator.RandomNumber(1, 100) >= lootItem.DropRate)
                    {
                        Loot.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }
                // If no items were randomly selected, then add the default loot item(s).
                if (Loot.Count == 0)
                {
                    foreach (LootItem lootItem in _monster.LootTable)
                    {
                        if (lootItem.IsDefault)
                        {
                            Loot.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }
                // Add the looted items to the player's inventory
                foreach (InventoryItem inventoryItem in Loot)
                {
                    _player.AddItem(inventoryItem.Details);
                    if (inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + Environment.NewLine;
                    }
                    else
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + Environment.NewLine;
                    }
                }

                UpdateInventory();
                UpdateWeaponsList();
                UpdatePotionsList();
                // Add a blank line to the messages box, just for appearance.
                rtbMessages.Text += Environment.NewLine;
                // Move player to current location (to heal player and create a new monster to fight)
                moveTo(_player.Location);
            }

            else
            {
                // Monster is still alive
                // Determine the amount of damage the monster does to the player
                int damageToPlayer = RandomNumberGenerator.RandomNumber(0, _monster.MaxDamage);
                // Display message
                rtbMessages.Text += "The " + _monster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;
                // Subtract damage from player
                _player.CurrentHitPoints -= damageToPlayer;
                // Refresh player data in UI
                label1.Text = _player.CurrentHitPoints.ToString();
                if (_player.CurrentHitPoints <= 0)
                {
                    // Display message
                    rtbMessages.Text += "The " + _monster.Name + " killed you." + Environment.NewLine;
                    // Move player to "Home"
                    moveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }
            }
        }

        private void btnUsePotion_Click(Object sender, EventArgs e)
        {
            // Get the currently selected potion from the combobox
            HealingSpell potion = (HealingSpell)cboPotions.SelectedItem;
            // Add healing amount to the player's current hit points
            _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);
            // CurrentHitPoints cannot exceed player's MaximumHitPoints
            if (_player.CurrentHitPoints > _player.MaxHitPoints)
            {
                _player.CurrentHitPoints = _player.MaxHitPoints;
            }
            // Remove the potion from the player's inventory
            foreach (InventoryItem ii in _player.InventoryItems)
            {
                if (ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }
            // Display message
            rtbMessages.Text += "You drink a " + potion.Name + Environment.NewLine;
            // Monster gets their turn to attack
            // Determine the amount of damage the monster does to the player
            int damageToPlayer = RandomNumberGenerator.RandomNumber(0, _monster.MaxDamage);
            // Display message
            rtbMessages.Text += "The " + _monster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;
            // Subtract damage from player
            _player.CurrentHitPoints -= damageToPlayer;
            if (_player.CurrentHitPoints <= 0)
            {
                // Display message
                rtbMessages.Text += "The " + _monster.Name + " killed you." + Environment.NewLine;
                // Move player to "Home"
                moveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }
            // Refresh player data in UI
            label1.Text = _player.CurrentHitPoints.ToString();
            UpdateInventory();
            UpdatePotionsList();
        }

        private void UpdateInventory()
        {
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";
            dgvInventory.Rows.Clear();

            foreach (InventoryItem i in _player.InventoryItems)
            {
                if (i.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { i.Details.Name, i.Quantity.ToString() });
                }
            }
        }

        private void UpdateQuestsList()
        {
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";
            dgvQuests.Rows.Clear();
            foreach (PlayerQuest pq in _player.PlayerQuests)
            {
                dgvQuests.Rows.Add(new[] { pq.Details.Name, pq.IsCompleted.ToString() });
            }
        }

        private void UpdateWeaponsList()
        {
            List<Weapon> weapon = new();
            foreach (InventoryItem i in _player.InventoryItems)
            {
                if (i.Details is Weapon && i.Quantity > 0)
                {
                    weapon.Add((Weapon)i.Details);
                }
            }

            if (weapon.Count == 0)
            {
                //player has no weapons so hide the combobox and buttons
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapon;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";
                cboWeapons.SelectedIndex = 0; // Select the first weapon by default
            }

        }

        private void UpdatePotionsList()
        {
            List<HealingSpell> healingSpells = new();
            foreach (InventoryItem i in _player.InventoryItems)
            {
                if (i.Details is HealingSpell && i.Quantity > 0)
                {
                    healingSpells.Add((HealingSpell)i.Details);
                }
            }
            if (healingSpells.Count == 0)
            {
                //player has no potions
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingSpells;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";
                cboPotions.SelectedIndex = 0;
            }
        }
        
    }
}


