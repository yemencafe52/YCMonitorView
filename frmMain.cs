namespace YCMonitorView
{
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Reflection;
    using System.Windows.Forms;
    using YCMonitorVeiw;

    public partial class frmMain : Form
    {
        private object locker = new object();
        public static int speed = 5000;
        public frmMain()
        {
            InitializeComponent();
            listView1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(listView1, true, null);
            Preparing();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
           
            if (keyData == Keys.Escape)
            {
                this.Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool Preparing()
        {
            toolStripButton1.SelectedIndex = 5;
            MyImageView.UpdateView += UpdateStationInfo2;
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            this.Text = "YCMonitor Veiw" + version.ToString();
            PrintStaions();
            return true;
        }
        private void PrintStaions()
        {
            imageList2.Images.Clear();
            imageList2.ColorDepth = ColorDepth.Depth32Bit;
            /*********************************************/
            imageList2.ImageSize = new System.Drawing.Size(256, 256);
            this.listView1.LargeImageList = imageList2;
            this.listView1.VirtualListSize = SessionManager.GetStations.Count;
            listView1.Items.Clear();
        }

        private ListViewItem GetVI(int index)
        {
            var s = SessionManager.GetStations.Find(p => p.Number == (index+1));
            ListViewItem lvi = new ListViewItem(s.Name,index);
            return lvi;
        }

        private object obj = new object();
      
        private void UpdateStationInfo2(Station station)
        {
            if (this.listView1.InvokeRequired)
            {
                this.Invoke(new MyImageView.dUpdateView(UpdateStationInfo2), new object[] { station });
                return;
            }
            lock (obj)
            {
                {
                    this.listView1.Invalidate();
                }
            }
        }

        private void viewRemoteDeskTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedIndices.Count > 0)
                {
                    int index = listView1.SelectedIndices[0] + 1;
                    string name = listView1.Items[index].Text;
                    Station s = SessionManager.GetStations.Find(p => p.Name == name);

                    if (s != null)
                    {
                        ScreenMonitor.frmSM fSM = new ScreenMonitor.frmSM(968, s.Name);
                        fSM.Show();
                    }
                }
            }
            catch
            {
                MessageBox.Show("تعذر تنفيذ العملية", "نظام يمن كافي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = GetVI(e.ItemIndex);
        }
       
        private object o = new object();
        private async void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            //lock (o)
            {
                var s = SessionManager.GetStations.Find(p => p.Number == (e.ItemIndex + 1));

                Rectangle rec = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 10, 220);

                Image i = SessionManager.GetMyImages[e.ItemIndex].GetImage();
                e.Graphics.DrawImage(i, rec);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                /***************************************************/
                Font f = new Font(new FontFamily(GenericFontFamilies.Serif), 12f);
                /*****************************************************************/
                Rectangle r = new Rectangle(e.Bounds.X, e.Bounds.Y + (e.Bounds.Height - 50), e.Bounds.Width - 10, 30);

                e.Graphics.FillRectangle(Brushes.Black, r);
                e.Graphics.DrawString(s.Name, f, Brushes.White, r, stringFormat);
            }
        }


        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void toolStripButton1_SelectedIndexChanged(object sender, EventArgs e)
        {
            speed = (toolStripButton1.SelectedIndex * 1000) + 1;
        }
    }
}