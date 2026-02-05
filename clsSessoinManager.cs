namespace YCMonitorView
{
    using System.Collections.Generic;
    using YCMonitorVeiw;

    internal class SessionManager
    {
        private static List<Station> stations;
        private static List<MyImageView> myImageViews;
        public SessionManager()
        {
            SessionManager.Init();
        }
      
        public static void Init()
        {
            if (stations is null)
            {
                stations = new List<Station>();
                myImageViews = new List<MyImageView>();

                for (int i = 1; i < 256; i++)
                {
                    string pName = "Pc" + i.ToString("0#");
                    var s = new Station() { Name = pName, Number = i };
                    stations.Add(s);
                    myImageViews.Add(new MyImageView(s.Name, 968, s));
                }
            }
        }
        internal static List<Station> GetStations
        {
            get
            {
                return stations;
            }
        }

        internal static List<MyImageView> GetMyImages
        {
            get
            {
                return myImageViews;
            }
        }
    }
}
