namespace YCMonitorView
{
    using System;
    using System.Windows.Forms;
    using System.Threading;
    static class EntryPoint
    {
        private static Mutex myMutex = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            bool has_created = false;
            myMutex = new Mutex(true, "YCMonitorViewM", out has_created);

            if (!has_created)
            {
                MessageBox.Show("النظام يعمل حالياً", "يمن كافي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            new SessionManager();
            Application.Run(new frmMain());
            myMutex.Close();
        }
    }
}
