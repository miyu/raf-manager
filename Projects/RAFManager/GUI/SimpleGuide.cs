using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RAFManager
{
    public partial class SimpleGuide : Form
    {
        MainForm mainForm = null;
        Timer t = null;
        public SimpleGuide(MainForm mainForm)
        {
            InitializeComponent();

            int bigPadding = 13;
            int smallPadding = 7;
            doDragDropLabel.Left = bigPadding;
            doDragDropLabel.Top = bigPadding;

            clickCheckBoxLabel.Left = bigPadding;
            clickCheckBoxLabel.Top = doDragDropLabel.Bottom + smallPadding;

            doPackLabel.Left = bigPadding;
            doPackLabel.Top = clickCheckBoxLabel.Bottom + smallPadding;

            this.ClientSize = new Size(
                Math.Max(
                    Math.Max(
                        doDragDropLabel.Right,
                        clickCheckBoxLabel.Right
                    ), doPackLabel.Right
                ) + bigPadding,
                doPackLabel.Bottom + bigPadding
            );

            this.mainForm = mainForm;

            t = new Timer();
            t.Interval = 100;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
            this.FormClosed += new FormClosedEventHandler(SimpleGuide_FormClosed);
        }

        void SimpleGuide_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Stop();
        }

        void t_Tick(object sender, EventArgs eventArgs)
        {
            try
            {
                List<ModEntry> modEntries = mainForm.ModEntries;
                UnboldEverything();

                if (modEntries.Count == 0)
                {
                    //doDragDropLabel.Font = new Font(doDragDropLabel.Font, FontStyle.Bold);
                    doDragDropLabel.BackColor = Color.FromArgb(64, 64, 64);
                }
                else if (modEntries.Where(
                    (Func<ModEntry, bool>)delegate(ModEntry e)
                    {
                        return e.IsChecked;
                    }
                ).Count() == 0)
                {
                    //clickCheckBoxLabel.Font = new Font(clickCheckBoxLabel.Font, FontStyle.Bold);
                    clickCheckBoxLabel.BackColor = Color.FromArgb(64, 64, 64);
                }
                else
                {
                    //doPackLabel.Font = new Font(doPackLabel.Font, FontStyle.Bold);
                    doPackLabel.BackColor = Color.FromArgb(64, 64, 64);
                }
            }
            catch { }//Technically not thread safe.
        }
        void UnboldEverything()
        {
            /*
            doDragDropLabel.Font = new Font(doDragDropLabel.Font, FontStyle.Regular);
            clickCheckBoxLabel.Font = new Font(doDragDropLabel.Font, FontStyle.Regular);
            doPackLabel.Font = new Font(doPackLabel.Font, FontStyle.Regular);
             */
            doDragDropLabel.BackColor = this.BackColor;
            clickCheckBoxLabel.BackColor = this.BackColor;
            doPackLabel.BackColor = this.BackColor;
        }
    }
}
