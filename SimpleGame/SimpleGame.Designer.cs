using System;

namespace SimpleGame
{
    partial class SimpleGame
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            lblHitPoints = new Label();
            lblGold = new Label();
            lblLevel = new Label();
            lblExperience = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            btnNorth = new Button();
            btnWest = new Button();
            btnEast = new Button();
            btnSouth = new Button();
            btnUseWeapon = new Button();
            btnUsePotion = new Button();
            rtbLocation = new RichTextBox();
            rtbMessages = new RichTextBox();
            cboPotions = new ComboBox();
            cboWeapons = new ComboBox();
            Control = new Label();
            dgvInventory = new DataGridView();
            dgvQuests = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvInventory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvQuests).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(124, 18);
            label1.Name = "label1";
            label1.Size = new Size(100, 20);
            label1.TabIndex = 0;
            // 
            // lblHitPoints
            // 
            lblHitPoints.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHitPoints.Location = new Point(18, 18);
            lblHitPoints.Name = "lblHitPoints";
            lblHitPoints.Size = new Size(100, 20);
            lblHitPoints.TabIndex = 1;
            lblHitPoints.Text = "HitPoints: ";
            // 
            // lblGold
            // 
            lblGold.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGold.Location = new Point(18, 46);
            lblGold.Name = "lblGold";
            lblGold.Size = new Size(100, 20);
            lblGold.TabIndex = 2;
            lblGold.Text = "Gold:";
            // 
            // lblLevel
            // 
            lblLevel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLevel.Location = new Point(18, 100);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(100, 20);
            lblLevel.TabIndex = 3;
            lblLevel.Text = "Level; ";
            // 
            // lblExperience
            // 
            lblExperience.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblExperience.Location = new Point(18, 74);
            lblExperience.Name = "lblExperience";
            lblExperience.Size = new Size(115, 20);
            lblExperience.TabIndex = 4;
            lblExperience.Text = "Experience: ";
            // 
            // label2
            // 
            label2.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(124, 46);
            label2.Name = "label2";
            label2.Size = new Size(100, 20);
            label2.TabIndex = 5;
            label2.Text = " ";
            // 
            // label3
            // 
            label3.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(124, 74);
            label3.Name = "label3";
            label3.Size = new Size(100, 20);
            label3.TabIndex = 6;
            label3.Text = " ";
            // 
            // label4
            // 
            label4.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(124, 100);
            label4.Name = "label4";
            label4.Size = new Size(100, 28);
            label4.TabIndex = 7;
            label4.Text = " ";
            // 
            // btnNorth
            // 
            btnNorth.Location = new Point(493, 428);
            btnNorth.Name = "btnNorth";
            btnNorth.Size = new Size(62, 34);
            btnNorth.TabIndex = 8;
            btnNorth.Text = "North";
            btnNorth.UseVisualStyleBackColor = true;
            btnNorth.Click += btnNorth_Click;
            // 
            // btnWest
            // 
            btnWest.Location = new Point(412, 457);
            btnWest.Name = "btnWest";
            btnWest.Size = new Size(62, 34);
            btnWest.TabIndex = 9;
            btnWest.Text = "West";
            btnWest.UseVisualStyleBackColor = true;
            btnWest.Click += btnWest_Click;
            // 
            // btnEast
            // 
            btnEast.Location = new Point(573, 457);
            btnEast.Name = "btnEast";
            btnEast.Size = new Size(62, 34);
            btnEast.TabIndex = 10;
            btnEast.Text = "East";
            btnEast.UseVisualStyleBackColor = true;
            btnEast.Click += btnEast_Click;
            // 
            // btnSouth
            // 
            btnSouth.Location = new Point(493, 487);
            btnSouth.Name = "btnSouth";
            btnSouth.Size = new Size(62, 34);
            btnSouth.TabIndex = 11;
            btnSouth.Text = "South";
            btnSouth.UseVisualStyleBackColor = true;
            btnSouth.Click += btnSouth_Click;
            // 
            // btnUseWeapon
            // 
            btnUseWeapon.Location = new Point(620, 587);
            btnUseWeapon.Name = "btnUseWeapon";
            btnUseWeapon.Size = new Size(62, 34);
            btnUseWeapon.TabIndex = 12;
            btnUseWeapon.Text = "Use (W)";
            btnUseWeapon.UseVisualStyleBackColor = true;
            btnUseWeapon.Click += btnUseWeapon_Click;
            // 
            // btnUsePotion
            // 
            btnUsePotion.Location = new Point(620, 553);
            btnUsePotion.Name = "btnUsePotion";
            btnUsePotion.Size = new Size(62, 34);
            btnUsePotion.TabIndex = 13;
            btnUsePotion.Text = "Use (P)";
            btnUsePotion.UseVisualStyleBackColor = true;
            btnUsePotion.Click += btnUsePotion_Click;
            // 
            // rtbLocation
            // 
            rtbLocation.Location = new Point(347, 19);
            rtbLocation.Name = "rtbLocation";
            rtbLocation.ReadOnly = true;
            rtbLocation.Size = new Size(360, 105);
            rtbLocation.TabIndex = 14;
            rtbLocation.Text = "";
            // 
            // rtbMessages
            // 
            rtbMessages.Location = new Point(347, 130);
            rtbMessages.Name = "rtbMessages";
            rtbMessages.ReadOnly = true;
            rtbMessages.Size = new Size(360, 286);
            rtbMessages.TabIndex = 15;
            rtbMessages.Text = "";
            // 
            // cboPotions
            // 
            cboPotions.FormattingEnabled = true;
            cboPotions.Location = new Point(369, 559);
            cboPotions.Name = "cboPotions";
            cboPotions.Size = new Size(122, 28);
            cboPotions.TabIndex = 16;
            // 
            // cboWeapons
            // 
            cboWeapons.FormattingEnabled = true;
            cboWeapons.Location = new Point(369, 593);
            cboWeapons.Name = "cboWeapons";
            cboWeapons.Size = new Size(122, 28);
            cboWeapons.TabIndex = 17;
            // 
            // Control
            // 
            Control.AutoSize = true;
            Control.Location = new Point(620, 531);
            Control.Name = "Control";
            Control.Size = new Size(96, 20);
            Control.TabIndex = 18;
            Control.Text = "Select Action";
            // 
            // dgvInventory
            // 
            dgvInventory.AllowUserToAddRows = false;
            dgvInventory.AllowUserToDeleteRows = false;
            dgvInventory.AllowUserToResizeRows = false;
            dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInventory.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvInventory.EnableHeadersVisualStyles = false;
            dgvInventory.Location = new Point(16, 130);
            dgvInventory.MultiSelect = false;
            dgvInventory.Name = "dgvInventory";
            dgvInventory.ReadOnly = true;
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.RowHeadersWidth = 51;
            dgvInventory.ScrollBars = ScrollBars.Vertical;
            dgvInventory.Size = new Size(312, 109);
            dgvInventory.TabIndex = 19;
            // 
            // dgvQuests
            // 
            dgvQuests.AllowUserToAddRows = false;
            dgvQuests.AllowUserToDeleteRows = false;
            dgvQuests.AllowUserToResizeRows = false;
            dgvQuests.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQuests.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvQuests.EnableHeadersVisualStyles = false;
            dgvQuests.Location = new Point(16, 245);
            dgvQuests.MultiSelect = false;
            dgvQuests.Name = "dgvQuests";
            dgvQuests.ReadOnly = true;
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.RowHeadersWidth = 51;
            dgvQuests.ScrollBars = ScrollBars.Vertical;
            dgvQuests.Size = new Size(312, 189);
            dgvQuests.TabIndex = 20;
            // 
            // SimpleGame
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(717, 643);
            Controls.Add(dgvQuests);
            Controls.Add(dgvInventory);
            Controls.Add(Control);
            Controls.Add(cboWeapons);
            Controls.Add(cboPotions);
            Controls.Add(rtbMessages);
            Controls.Add(rtbLocation);
            Controls.Add(btnUsePotion);
            Controls.Add(btnUseWeapon);
            Controls.Add(btnSouth);
            Controls.Add(btnEast);
            Controls.Add(btnWest);
            Controls.Add(btnNorth);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblExperience);
            Controls.Add(lblLevel);
            Controls.Add(lblGold);
            Controls.Add(lblHitPoints);
            Controls.Add(label1);
            Name = "SimpleGame";
            Text = "My Game";
            ((System.ComponentModel.ISupportInitialize)dgvInventory).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvQuests).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label lblHitPoints;
        private Label lblGold;
        private Label lblLevel;
        private Label lblExperience;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button btnNorth;
        private Button btnWest;
        private Button btnEast;
        private Button btnSouth;
        private Button btnUseWeapon;
        private Button btnUsePotion;
        private RichTextBox rtbLocation;
        private RichTextBox rtbMessages;
        private ComboBox cboPotions;
        private ComboBox cboWeapons;
        private Label Control;
        private DataGridView dgvInventory;
        private DataGridView dgvQuests;
    }
}
