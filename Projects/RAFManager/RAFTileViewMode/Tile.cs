using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RAFManager.RAFTileViewMode
{
    public partial class Tile : UserControl
    {
        public Tile(Bitmap icon)
        {
            InitializeComponent();
            //ManageLayout();
            this.fileIconPB.Image = icon;
        }
        /*
        int padding   = 3;
        private Size GetIdealIconSize(Bitmap bitmap)
        {
            Size availableBounds = new Size(
                this.Size.Width - padding * 2,
                this.Size.Height - this.fileNameLabel.Height - padding * 3
            );

            int smallestBound = Math.Min(availableBounds.Width, availableBounds.Height);

            Size oSize = bitmap.Size;
            if (oSize.Width > oSize.Height)
            {
                return new Size(
                    (int)((oSize.Width / oSize.Height) * idealSize),
                    idealSize
                );
            }
            else
            {
                return new Size(
                    idealSize,
                    (int)((oSize.Height / oSize.Width) * idealSize)
                );
            }
        }
        private void ManageLayout()
        {
            fileIconPB.Size = GetIdealIconSize((Bitmap)this.fileIconPB.Image);

        }
        /**/
    }
}
