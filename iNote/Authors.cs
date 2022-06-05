using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;

namespace iNote
{
    public partial class Authors : Form
    {
        public Authors()
        {
            InitializeComponent();
        }

        private void Authors_Shown(object sender, EventArgs e)
        {
            if (My.ShownClosing == true)
            {
                ((Control)sender).Refresh();
                for (Opacity = 0; Opacity < 1; Opacity += .03, System.Threading.Thread.Sleep(10)) ;
            }
        }

        private void Authors_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (My.ShownClosing == true)
            {
                for (; Opacity > 0; Opacity -= .03, System.Threading.Thread.Sleep(10)) ;
            }
        }

        private void Authors_Load(object sender, EventArgs e)
        {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("m-brody.ttf");
            label2.Font = new Font(pfc.Families[0], 36, FontStyle.Regular);
        }
    }
}
