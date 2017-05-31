using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeSpeedrun
{
    public partial class ScoreDisplay : Form
    {
        public ScoreDisplay()
        {
            InitializeComponent();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.DrawRectangle(Pens.Cyan, 0, 0, Width - 1, Height - 1);
        }
    }
}
