using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SlimDX;
using SlimDX.Direct3D9;

using System.IO;

using ItzWarty;

namespace DDS_Viewer
{
    public partial class TextureViewer : Form
    {
        string texturePath = "";
        public TextureViewer(string texturePath)
        {
            InitializeComponent();

            this.texturePath = texturePath;
            this.Text = this.Text + "(" + texturePath+")";

            Load += new EventHandler(TextureViewer_Load);
        }
        PresentParameters pp;
        Device d3d_device;
        Sprite sprite;
        Texture texture;
        void TextureViewer_Load(object sender, EventArgs e)
        {
            this.Show();
            Application.DoEvents();
            pp = new PresentParameters();
            pp.BackBufferCount = 2;
            pp.SwapEffect = SwapEffect.Discard;
            pp.Windowed = true;

            Direct3D d3d = new Direct3D();
            d3d_device = new Device(d3d, 0, DeviceType.Hardware, d3dPanel.Handle, CreateFlags.HardwareVertexProcessing, pp);
            texture = Texture.FromFile(d3d_device, texturePath);

            int texWidth = texture.GetLevelDescription(0).Width;
            int texHeight = texture.GetLevelDescription(0).Height;
            d3dPanel.Top = menuStrip1.Height;
            d3dPanel.Left = 0;
            d3dPanel.ClientSize = new Size(texWidth, texHeight);
            this.ClientSize = new System.Drawing.Size(
                d3dPanel.Width,
                d3dPanel.Height + menuStrip1.Height
            );
            pp.BackBufferWidth = texWidth;
            pp.BackBufferHeight = texHeight;

            d3d_device.Reset(pp);
            sprite = new Sprite(d3d_device);

            while (this.Visible)
            {
                d3d_device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, new Color4(Color.Black), 1.0f, 0);
                d3d_device.BeginScene();
                sprite.Begin(SpriteFlags.AlphaBlend);
                Rectangle rect = new Rectangle()
                {
                    X = 0,
                    Y = 0,
                    Width = texture.GetLevelDescription(0).Width,
                    Height = texture.GetLevelDescription(0).Height
                };
                sprite.Draw(texture, rect, new Color4(Color.White));
                sprite.End();
                d3d_device.EndScene();
                d3d_device.Present();
                Application.DoEvents();
            }
        }

        private void openDdsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "DirectDraw Surface|*.dds";
            ofd.ShowDialog();

            if (ofd.FileName != "")
                new TextureViewer(ofd.FileName).Show();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Bitmap File|*.bmp";
            sfd.ShowDialog();
            if(sfd.FileName != "")
                Surface.ToFile(d3d_device.GetBackBuffer(0, 0), sfd.FileName, ImageFileFormat.Bmp);
        }

        private void saveToDiskddsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "DirectDraw Surface|*.dds";
            sfd.ShowDialog();
            if (sfd.FileName != "")
                File.Copy(texturePath, sfd.FileName);
        }
    } 
}
