using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace RAFManager.GUI
{
    public class WProgressBar : ProgressBar
    {
        public Color ForegroundColor
        {
            get;
            set;
        }
        public Color BorderColor
        {
            get;
            set;
        }
        public int BorderThickness
        {
            get;
            set;
        }
        public Color FontColor
        {
            get;
            set;
        }
        public Color FontBorderColor
        {
            get;
            set;
        }
        public override Font Font
        {
            get;
            set;
        }

        public ContentAlignment TextAlign
        {
            get;
            set;
        }
        public string AText
        {
            get;
            set;
        }
        public WProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.ForegroundColor = ColorTranslator.FromHtml("#707070");
            this.BackColor = ColorTranslator.FromHtml("#303030");
            this.BorderColor = ColorTranslator.FromHtml("#000000");
            this.BorderThickness = 1;

            this.FontColor = Color.Purple;
            this.FontBorderColor = Color.Red;
            this.Font = new Font("Lucida Console", 8.5f);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.AText = "Example";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Draw background
            using (SolidBrush backgroundBrush = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(backgroundBrush, BorderThickness, BorderThickness, e.ClipRectangle.Width - 2 * BorderThickness, e.ClipRectangle.Height - 2 * BorderThickness);
            }

            //Gets the rectangle to fill the progress bar.
            int fillableWidth = e.ClipRectangle.Width - 2 * BorderThickness;
            int fillableHeight = e.ClipRectangle.Height - 2 * BorderThickness;
            using(SolidBrush fgb = new SolidBrush(ForegroundColor))
            {
                e.Graphics.FillRectangle(fgb, BorderThickness, BorderThickness,
                                         (int)(fillableWidth * ((double)Value / (double)Maximum)),
                                         fillableHeight
                );
            }
            //Draw our text...
            using (SolidBrush fontColorBrush = new SolidBrush(FontColor))
            using (SolidBrush fontBorderBrush = new SolidBrush(FontBorderColor))
            {
                SizeF textSize = e.Graphics.MeasureString(this.AText, Font);
                PointF contentPosition = wGUI.GetContentPosition(this.TextAlign, textSize, this.Size);
                e.Graphics.DrawString(this.AText, Font, fontColorBrush, contentPosition);
                for (int oX = -1; oX <= 1; oX++)
                    for (int oY = -1; oY <= 1; oY++)
                        if (oX != 0 && oY != 0)
                            e.Graphics.DrawString(this.Text, Font, fontBorderBrush, new PointF(contentPosition.X + oX, contentPosition.Y + oY));
            }

            //Draw border
            using (Pen borderPen = new Pen(BorderColor, BorderThickness))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
            }
        }
    }
}
