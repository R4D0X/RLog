using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MetroSet_UI.Controls;
using System.Drawing.Text;
using System.IO;
using System.Reflection;

namespace Netro
{
    public partial class Form2 : MetroSet_UI.Forms.MetroSetForm
    {
        PrivateFontCollection pfc = new PrivateFontCollection();
        public Form2()
        {
            InitializeComponent();
            this.DropShadowEffect = false;
            this.StyleManager = styleManager1;

            linkLabel1.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.NeverUnderline;

            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "";
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Padding = new Padding(2);
            this.StartPosition = FormStartPosition.CenterScreen;

            //--------------------------------------------------------

            Panel ustPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(30, 30, 30)
            };
            this.Controls.Add(ustPanel);

            Label baslik = new Label
            {
                Text = "About",
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(135, 10),
                TextAlign = ContentAlignment.TopCenter,
            };
            ustPanel.Controls.Add(baslik);

            Button kapatma = new Button
            {
                Text = "✕",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(220, 53, 69),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(40, 30),
                Location = new Point(this.Width - 50, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            //--------------------------------------------------------

            kapatma.FlatAppearance.BorderSize = 0;
            kapatma.Click += (s, args) => this.Close();
            ustPanel.Controls.Add(kapatma);

            ustPanel.MouseDown += TopPanel_MouseDown;
            ustPanel.MouseMove += TopPanel_MouseMove;

            this.Resize += (s, args) =>
            {
                kapatma.Location = new Point(this.Width - kapatma.Width - 10, 5);
            };
        }

        private Point mouseDownLocation;

        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDownLocation = e.Location;
        }

        private void TopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - mouseDownLocation.X;
                this.Top += e.Y - mouseDownLocation.Y;
            }
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_NCLBUTTONDBLCLK = 0x00A3;
            const int WM_LBUTTONDBLCLK = 0x0203;

            if (m.Msg == WM_NCLBUTTONDBLCLK || m.Msg == WM_LBUTTONDBLCLK)
                return;

            base.WndProc(ref m);
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.DropShadowEffect = false;

        }
#pragma warning disable IDE1006
        private void label2_Click(object sender, EventArgs e)
        #pragma warning restore IDE1006
        {

        }
        #pragma warning disable IDE1006
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        #pragma warning restore IDE1006
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://spyhackerz.org",
                UseShellExecute = true
            });
        }
#pragma warning disable IDE1006
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
#pragma warning restore IDE1006
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://imhateam.org/",
                UseShellExecute = true
            });
        }
#pragma warning disable IDE1006
        private void linklabel1_MouseEnter(object sender, EventArgs e)
#pragma warning disable IDE1006
        {
            label3.Font = new Font(label3.Font, FontStyle.Underline);
        }
#pragma warning disable IDE1006
        private void linklabel2_MouseLeave(object sender, EventArgs e)
#pragma warning disable IDE1006
        {
            label3.Font = new Font(label3.Font, FontStyle.Regular);
        }
#pragma warning disable IDE1006
        private void label6_Click(object sender, EventArgs e)
#pragma warning restore IDE1006
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://t.me/radoxin",
                UseShellExecute = true
            });
        }
    }
}
