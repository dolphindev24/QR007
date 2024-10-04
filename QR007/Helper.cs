using System;
using System.Reflection;
using System.Windows.Forms;

namespace QR007
{
    public static class Helper
    {
        public static string UserName { get; set; }
        public static string ID { get; set; }

        public static void DoubleBufferded(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
