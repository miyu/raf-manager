using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
namespace RAFManager
{
    public class BlackCustomProfessionalColors : ProfessionalColorTable
    {
        public override Color ToolStripBorder
        { get { return Color.FromArgb(64, 64, 64); } }

        public override Color ToolStripGradientBegin
        { get { return ToolStripBorder; } }

        public override Color ToolStripGradientMiddle
        { get { return ToolStripBorder; } }

        public override Color ToolStripGradientEnd
        { get { return ToolStripBorder; } }
        
        public override Color GripLight
        { get { return ToolStripBorder; } }

        public override Color GripDark
        { get { return ToolStripBorder; } }

        public override Color MenuStripGradientBegin
        { get { return Color.Salmon; } }

        public override Color MenuStripGradientEnd
        { get { return Color.OrangeRed; } }

        public override Color ToolStripDropDownBackground
        {
            get
            {
                return Color.FromArgb(80, 80, 80);
            }
        }
        public override Color ButtonPressedGradientBegin
        {
            get
            {
                return Color.Red;
            }
        }
    }
}
