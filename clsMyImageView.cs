namespace YCMonitorVeiw
{
    using ScreenMonitor;
    using System.Drawing;
    using System.Threading;
    using YCMonitorView;

    public class MyImageView
    {
        private readonly string ip;
        private readonly int port;
        private Image image;
        private Thread th0;
        private Station s;
        /*****************/
        private bool isRunning;
        public string Ip => ip;
        /**********************/
        internal delegate void dUpdateView(Station s);
        internal static event dUpdateView UpdateView;
        /********************************************/
        public MyImageView(string ip, int port, Station s)
        {
            this.ip = ip;
            this.port = port;
            this.s = s;
            this.image = YCMonitorVeiw.Y.default3;
            /***************************************************/
            this.th0 = new Thread(Start);
            this.th0.IsBackground = true;
            this.th0.Start();
        }

        private void Start()
        {
            this.isRunning = true;
            while (isRunning)
            {
                try
                {
                    using (var img = new MyImage(250, 250, 968, this.ip).GetImage)
                    {
                        if (!(img is null))
                        {
                            lock (obj)
                            {
                                this.image = (Image)img.Clone();
                            }
                            UpdateView(this.s);
                        }
                        else
                        {
                            Thread.Sleep(15000);
                        }
                    }
                }
                catch
                {
                    lock (obj)
                    {
                        this.image = YCMonitorVeiw.Y.default3;
                    }

                    Thread.Sleep(15000);
                }

                Thread.Sleep(frmMain.speed);
            }
        }

        public void Stop()
        {
            this.isRunning = false;
            this.th0.Join();
        }

        private object obj = new object();
        public Image GetImage()
        {
            if (!(image is null))
            {
                lock (obj)
                {
                    return (Image)image.Clone();
                }
            }
            return (Image)YCMonitorVeiw.Y.default3.Clone();
        }
    }
}

