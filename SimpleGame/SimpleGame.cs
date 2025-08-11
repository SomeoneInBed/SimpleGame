using System.Diagnostics.CodeAnalysis;
using Engine;
using System.IO;

namespace SimpleGame
{
    public partial class SimpleGame : Form
    {
        private const string V = " ";
        private Player _player; //new player + stats
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Monster can be reassigned.")]
        private Monster? _monster;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";
        public SimpleGame()
        {
            InitializeComponent();
            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                //load player data 
                _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                //create a new player
                _player = Player.CreateDefaultPlayer();
            }

            UpdatePlayerStats();

            //set the starting location
            moveTo(_player.Location);

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
            // If player goes home, health is restored
            if (newLocation == World.LocationByID(World.LOCATION_ID_HOME))
            {
                _player.CurrentHitPoints = _player.MaxHitPoints;
            }

            // Check if item is required to enter
            if (!_player.HasRequiredItemToEnterLocation(newLocation))
            {
                rtbMessages.Text = "You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter here." + Environment.NewLine;
                return; // Exit the method if player does not have the required item
            }

            _player.Location = newLocation;

            btnNorth.Visible = newLocation.LocationToNorth != null;
            btnSouth.Visible = newLocation.LocationToSouth != null;
            btnEast.Visible = newLocation.LocationToEast != null;
            btnWest.Visible = newLocation.LocationToWest != null;

            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            //check for quest 
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

                    _player.PlayerQuests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            //check for monster
            if (newLocation.MonsterLivingHere != null)
            {
                //Tell the player that the monster is here
                rtbMessages.Text += $"You see a {newLocation.MonsterLivingHere.Name} here." + Environment.NewLine;
                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID)!;

                _monster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.CurrentHitPoints, standardMonster.MaxHitPoints, standardMonster.MaxDamage, standardMonster.RewardEXP, standardMonster.RewardGold);

                foreach (LootItem li in standardMonster.LootTable)
                {
                    _monster.LootTable.Add(li);
                }

                //give the player their battle options (heal or attack)
                cboVisible();
            }

            else
            {
                _monster = null;

                cboInvisible();
            }

            UpdateInventory();
            UpdateQuestsList();
            UpdateWeaponsList();
            UpdatePotionsList();
            ScrollToBottomOfMessages();
            UpdatePlayerStats();
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

                if (_player.CurrentHitPoints <= 0)
                {
                    // Display message
                    rtbMessages.Text += "The " + _monster.Name + " killed you." + Environment.NewLine;
                    // Move player to "Home"
                    moveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }

            }
            ScrollToBottomOfMessages();
            UpdatePlayerStats();
        }

        private void btnUsePotion_Click(Object sender, EventArgs e)
        {
            // Get the currently selected potion from the combobox
            HealingSpell potion = (HealingSpell)cboPotions.SelectedItem;
            // Add healing amount to the player's current hit points
            _player.CurrentHitPoints += potion.AmountToHeal;
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
            UpdateInventory();
            UpdatePotionsList();
            ScrollToBottomOfMessages();
            UpdatePlayerStats();
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
            List<Weapon> weapons = new();

            foreach (InventoryItem i in _player.InventoryItems)
            {
                if (i.Details is Weapon && i.Quantity > 0)
                {
                    weapons.Add((Weapon)i.Details);
                }
            }

            if (weapons.Count == 0)
            {
                //no weapons in inventory
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.SelectedIndexChanged -= cboWeapons_SelectedIndexChanged;
                cboWeapons.DataSource = weapons;
                cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                if (_player.CurrentWeapon != null)
                {
                    cboWeapons.SelectedItem = _player.CurrentWeapon;
                }
                else
                {
                    cboWeapons.SelectedIndex = 0; //select first weapon if no current weapon
                }
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

        private void ScrollToBottomOfMessages()
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void UpdatePlayerStats()
        {
            // refresh player stats in the UI
            label1.Text = _player.CurrentHitPoints.ToString();
            label2.Text = _player.Gold.ToString();
            label3.Text = _player.ExperiencePoints.ToString();
            label4.Text = _player.Level.ToString();
        }

        private void cboVisible()
        {
            cboWeapons.Visible = true;
            cboPotions.Visible = true;
            btnUseWeapon.Visible = true;
            btnUsePotion.Visible = true;
        }
        private void cboInvisible()
        {
            cboWeapons.Visible = false;
            cboPotions.Visible = false;
            btnUseWeapon.Visible = false;
            btnUsePotion.Visible = false;
        }

        private void SimpleGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentWeapon = (Weapon)cboWeapons.SelectedItem;
        }
    }
}


