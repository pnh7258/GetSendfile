using GetFileR.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetFileR
{
    public partial class ConnectFTP : Form
    {
        public ConnectFTP()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionFTP.link = txt_link.Text;
            ConnectionFTP.user = txt_user.Text;
            ConnectionFTP.password = txt_password.Text;
            if (ConnectionFTP.Instance())
                MessageBox.Show("connected!!!");
            else
                MessageBox.Show("can not connect!!!");
        }
    }
}
