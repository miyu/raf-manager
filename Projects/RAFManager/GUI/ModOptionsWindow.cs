using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Setting = RAFManager.RMPropInterpreter.RMPropSetting;

namespace RAFManager
{
    public partial class ModOptionsWindow : Form
    {
        Font font = new Font("Consolas", 8f, FontStyle.Regular);
        public ModOptionsWindow(RMPropInterpreter script)
        {
            InitializeComponent();

            this.Load += new EventHandler(ModOptionsWindow_Load);

            this.ControlBox = false;

            List<Setting> settings = script.Settings;
            Graphics g = this.CreateGraphics();
            
            int padding = 2;
            int comboBoxWidth = 150;
            int longestLabelWidth = 0;
            for (int i = 0; i < settings.Count; i++)
                longestLabelWidth = Math.Max(longestLabelWidth, (int)g.MeasureString(settings[i].Label, font).Width);

            this.ClientSize = new System.Drawing.Size(
                padding + longestLabelWidth + padding + comboBoxWidth + padding,
                this.ClientSize.Height
            );
            int offsetY = 2;
            for (int i = 0; i < settings.Count; i++)
            {
                Setting setting = settings[i];
                Label label = new Label();
                ComboBox comboBox = new ComboBox();

                label.Text = setting.Label;
                label.ForeColor = Color.FromArgb(230, 230, 230);
                comboBox.Items.AddRange(setting.GetOptions());
                comboBox.SelectedItem = setting.SelectedValue;
                comboBox.SelectedIndexChanged += delegate(object sender, EventArgs e)
                {
                    setting.SelectedValue = (string)comboBox.SelectedItem;
                };
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

                label.Top = offsetY;
                comboBox.Top = offsetY;
                comboBox.Width = comboBoxWidth;
                comboBox.Left = this.ClientSize.Width - comboBox.Width - padding;
                label.Width = (int)g.MeasureString(label.Text, font).Width;
                label.Left = comboBox.Left - label.Width - padding;
                label.Height = comboBox.Height;
                label.TextAlign = ContentAlignment.MiddleRight;

                offsetY += padding + Math.Max(label.Height, comboBox.Height);

                this.Controls.Add(label);
                this.Controls.Add(comboBox);
            }

            Button done = new Button();
            done.Text = "Done";
            done.Top = offsetY;
            done.Left = this.ClientSize.Width - done.Width - padding;
            done.Click += new EventHandler(done_Click);
            done.BackColor = SystemColors.Control;
            offsetY += done.Height + padding;
            this.Controls.Add(done);
            this.ClientSize = new Size(
                this.ClientSize.Width,
                offsetY
            );
        }

        void done_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ModOptionsWindow_Load(object sender, EventArgs e)
        {
            this.Left = Cursor.Position.X - this.Width/2;
            this.Top = Cursor.Position.Y - this.Height/2;
        }
    }
}
