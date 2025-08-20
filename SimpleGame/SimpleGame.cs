using System.Diagnostics.CodeAnalysis;
using Engine;
using System.IO;
using System.ComponentModel;
using System.Xml.Xsl;

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
            UpdateQuestsList();
            UpdateInventory();
            FillInCbo();
            //set the starting location
            _player.moveTo(_player.Location);

            _player.PropertyChanged += PlayerOnPropertyChanged;
            _player.OnMessage = DisplayMessage;

        }

        private void _player_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            _player.MoveNorth();
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            _player.MoveEast();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            _player.MoveWest();
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            _player.MoveSouth();
        }

        private void DisplayMessage(object sender, MessageEventArgs msgs )
        {
            rtbMessages.Text += msgs.Message + Environment.NewLine;
            if (msgs.AddExtraNewLine)
            {
                rtbMessages.Text += Environment.NewLine;
            }
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

       
        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            //grab the current weapon
            Weapon weapon = (Weapon)cboWeapons.SelectedItem;
            _player.UseWeapon(weapon);
        }

        private void btnUsePotion_Click(Object sender, EventArgs e)
        {
            // Get the currently selected potion from the combobox
            HealingSpell potion = (HealingSpell)cboPotions.SelectedItem;
            _player.UsePotion(potion);
            
        }

        private void UpdateQuestsList()
        {
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;
            dgvQuests.DataSource = _player.PlayerQuests;
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197, 
                DataPropertyName = "Name"
            });
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Complete?",
                DataPropertyName = "IsCompleted"
            });
        }

        private void UpdatePlayerStats()
        {
            label1.DataBindings.Add("Text", _player, "CurrentHitPoints");
            label2.DataBindings.Add("Text", _player, "Gold");
            label3.DataBindings.Add("Text", _player, "ExperiencePoints");
            label4.DataBindings.Add("Text", _player, "Level");
        }

        private void UpdateInventory()
        {
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.DataSource = _player.InventoryItems;
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Description"
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });
        }

        private void cboVisible()
        {
            cboWeapons.Visible = _player.Weapons.Any();
            cboPotions.Visible = _player.HealingPotions.Any();
            btnUseWeapon.Visible = _player.Weapons.Any();
            btnUsePotion.Visible = _player.HealingPotions.Any();
        }
        private void cboInvisible()
        {
            cboWeapons.Visible = false;
            cboPotions.Visible = false;
            btnUseWeapon.Visible = false;
            btnUsePotion.Visible = false;
        }

        private void FillInCbo()
        {
            cboWeapons.DataSource = _player.Weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "Id";
            if (_player.CurrentWeapon != null)
            {
                cboWeapons.SelectedItem = _player.CurrentWeapon;
            }
            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;
            cboPotions.DataSource = _player.HealingPotions;
            cboPotions.DisplayMember = "Name";
            cboPotions.ValueMember = "Id";
            _player.PropertyChanged += PlayerOnPropertyChanged;
        }

        private void SimpleGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());

             if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                File.Delete(PLAYER_DATA_FILE_NAME);
            }  //reset player data if needed
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentWeapon = (Weapon)cboWeapons.SelectedItem;
        }

        private void PlayerOnPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Weapons")
            {
                cboWeapons.DataSource = _player.Weapons;
                if (!_player.Weapons.Any())
                {
                    cboWeapons.Visible = false;
                    btnUseWeapon.Visible = false;
                }
            }

            if (e.PropertyName == "HealingPotions")
            {
                cboPotions.DataSource = _player.HealingPotions;
                if (!_player.HealingPotions.Any())
                {
                    cboPotions.Visible = false;
                    btnUsePotion.Visible = false;
                }
            }

            if (e.PropertyName == "Location")
            {
                btnNorth.Visible = _player.Location.LocationToNorth != null;
                btnSouth.Visible = _player.Location.LocationToSouth != null;
                btnEast.Visible = _player.Location.LocationToEast != null;
                btnWest.Visible = _player.Location.LocationToWest != null;

                rtbLocation.Text = _player.Location.Name + Environment.NewLine;
                rtbLocation.Text += _player.Location.Description + Environment.NewLine;

                if(_player.Location.MonsterLivingHere == null)
                {
                    cboInvisible();
                }
                else
                {
                    cboVisible();
                }
            }
        }
    }
}


