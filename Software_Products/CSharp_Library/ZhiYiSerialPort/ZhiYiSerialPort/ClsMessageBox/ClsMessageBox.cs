using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZhiYiMessageBox
{
    public class ClsMessageBox
    {
        public static void Information(string mesg)
        {
            MessageBox.Show(mesg, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Error(string mesg)
        {
            MessageBox.Show(mesg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Exception(string mesg)
        {
            MessageBox.Show(mesg, "异常", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
