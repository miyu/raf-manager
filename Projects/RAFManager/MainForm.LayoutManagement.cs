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
    public partial class MainForm:Form
    {
        Panel bottomPanel = null;
        Panel modEntriesPanel = null;
        PictureBox dropFileToBeginPB = new PictureBox();

        public void InitializeLayoutManagement()
        {
            bottomPanel = splitContainer1.Panel2;
            modEntriesPanel = splitContainer2.Panel2;

            //Initialize buttons & their states
            wGUI.InitializeButton(  consoleButtonPB,
                                    Properties.Resources.ConsoleButton_Normal,
                                    Properties.Resources.ConsoleButton_Highlight,
                                    Properties.Resources.ConsoleButton_Down,
                                    false
            );
            wGUI.InitializeButton(  optionsButtonPB,
                                    Properties.Resources.OptionsButton_Normal,
                                    Properties.Resources.OptionsButton_Highlight,
                                    Properties.Resources.OptionsButton_Down,
                                    false
            );

            //Binds tabs to their panels
            BindTab(consoleButtonPB, bottomPanelConsoleTab);
            BindTab(optionsButtonPB, bottomPanelOptionsTab);

            //Handle resizing
            int bottomPanelHeight = 0;
            this.ResizeBegin += delegate(object sender, EventArgs e)
            {
                bottomPanelHeight = splitContainer1.Panel2.Height;
            };
            this.Resize += delegate(object sender, EventArgs e)
            {
                try
                {
                    HandleUI();
                    splitContainer1.SplitterDistance = splitContainer1.Height - bottomPanelHeight;
                }
                catch { }
            };

            this.splitContainer1.SplitterMoved += delegate(object sender, SplitterEventArgs e)
            {
                //For if we're minimized and such
                try
                {
                    HandleUI();
                }
                catch { }
            };

            this.modEntriesScrollbar.Scroll += new ScrollEventHandler(modEntriesScrollbar_Scroll);
            TreeViewStylizer.Stylize(rafContentView);

            //ToolStripManager.Renderer
            toolStrip1.Renderer = new BlackCustomRenderer(new BlackCustomProfessionalColors());

            dropFileToBeginPB.Image = Properties.Resources.DropFileToBegin;
            dropFileToBeginPB.Visible = false;
            modEntriesPanel.Controls.Add(dropFileToBeginPB);

            HandleUI();
            HandleTabClick(consoleButtonPB);
        }
        List<Panel> panels = new List<Panel>();
        private void BindTab(PictureBox tab, Panel panel)
        {
            tab.Tag = panel;
            panel.Tag = tab;

            panels.Add(panel);

            tab.Click += delegate(object sender, EventArgs e)
            {
                HandleTabClick(tab);
            };
        }
        private void HandleTabClick(PictureBox tab)
        {
            for (int i = 0; i < panels.Count; i++)
                if (panels[i].Tag != tab)
                {
                    panels[i].Visible = false;
                    ((PictureBox)panels[i].Tag).Top = 6;
                }
                else
                {
                    panels[i].Visible = true;
                    ((PictureBox)panels[i].Tag).Top = 2;
                }
        }
        private void HandleUI()
        {
            splitContainer1.Top = toolStrip1.Height;
            splitContainer1.Left = 0;
            splitContainer1.Width = this.ClientSize.Width;
            splitContainer1.Height = this.ClientSize.Height - toolStrip1.Height;

            bottomPanelTabBar.Top = 2;
            bottomPanelTabBar.Height = bottomPanelTabBar.Controls[0].Height + 2;
            int offsetX = 2;
            for (int i = 0; i < bottomPanelTabBar.Controls.Count; i++)
            {
                Control c = bottomPanelTabBar.Controls[i];
                c.Left = offsetX;
                //c.Top = 2; //not necessary, since we call tab formatting immediately after this call in Init
                offsetX += c.Width + 2;
            }

            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].Top = bottomPanelTabBar.Bottom;
                panels[i].Height = bottomPanelTabContainer.Height - bottomPanelTabBar.Bottom - 4;
                panels[i].Left = 4;
                panels[i].Width = bottomPanelTabContainer.Width - 8;
            }

            this.splitContainer3.SplitterDistance = this.archiveBrowserTitleLabel.Height;

            ManageModEntriesLayout();
        }

        private PictureBox GetSelectedTab()
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].Visible)
                    return (PictureBox)panels[i].Tag;
            }
            return null;
        }

        List<ModEntry> modEntries = new List<ModEntry>();
        public List<ModEntry> ModEntries
        {
            get
            {
                return new List<ModEntry>(modEntries);
            }
        }
        private ModEntry CreateAndAppendModEntry(string modName, string modCreator, string modURL, TreeNode[] content)
        {
            ModEntry result = new ModEntry(modName, modCreator, modURL, content);
            modEntriesPanel.Controls.Add(result);
            modEntries.Add(result);
            result.Resize += delegate(object sender, EventArgs e)
            {
                if (result.Height == 1)
                {
                    //Delete it
                    modEntries.Remove(result);
                    modEntriesPanel.Controls.Remove(result);
                }
                ManageModEntriesLayout();
            };
            ManageModEntriesLayout();
            return result;
        }

        private void ManageModEntriesLayout()
        {
            ManageModEntriesLayout(false);
        }
        private void ManageModEntriesLayout(bool isRecursiveCall)
        {
            //Measure total height
            int totalHeight = 0;
            for (int i = 0; i < modEntries.Count; i++)
                totalHeight += modEntries[i].Height;

            modEntriesScrollbar.Maximum = totalHeight;


            if (modEntriesScrollbar.Maximum < modEntriesPanel.Height)
            {
                modEntriesScrollbar.Visible = false;
                modEntriesScrollbar.Value = 0;
            }
            else
            {
                modEntriesScrollbar.Visible = true;
            }

            int offsetY = -modEntriesScrollbar.Value;
            for (int i = 0; i < modEntries.Count; i++)
            {
                modEntries[i].Top = offsetY;
                if (modEntriesScrollbar.Visible)
                    modEntries[i].Width = modEntriesPanel.Width - modEntriesScrollbar.Width;
                else
                    modEntries[i].Width = modEntriesPanel.Width;
                offsetY += modEntries[i].Height + 1; //1 for padding
            }

            modEntriesScrollbar.Minimum = 0;
            modEntriesScrollbar.Maximum = offsetY + modEntriesScrollbar.Value;
            modEntriesScrollbar.LargeChange = modEntriesPanel.Height;

            if (modEntries.Count == 0)
            {
                dropFileToBeginPB.Visible = true;
                dropFileToBeginPB.Width = dropFileToBeginPB.Image.Width;
                dropFileToBeginPB.Height = dropFileToBeginPB.Image.Height;
                dropFileToBeginPB.Left = (modEntriesPanel.Width - dropFileToBeginPB.Width) / 2;
                dropFileToBeginPB.Top = (modEntriesPanel.Height - dropFileToBeginPB.Height) / 2;
            }
            else
                dropFileToBeginPB.Visible = false;
        }
        void modEntriesScrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            ManageModEntriesLayout();
        }
        private void ClearModEntries()
        {
            for (int i = 0; i < modEntries.Count; i++)
            {
                modEntriesPanel.Controls.Remove(modEntries[i]);
            }
            modEntries.Clear();

            Log("Mod Entries Cleared");
        }
    }
}
