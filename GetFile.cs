using GetFileR.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetFileR
{
    public partial class GetFile : Form
    {
        public GetFile()
        {
            InitializeComponent();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectFTP cn = new ConnectFTP();
            cn.Show();
        }

        private void PasteClipboard(DataGridView myDataGridView)
        {
            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {
                if (myDataGridView.RowCount > 0)
                    myDataGridView.Rows.Clear();

                if (myDataGridView.ColumnCount > 0)
                    myDataGridView.Columns.Clear();

                bool columnsAdded = false;
                string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                foreach (string pastedRow in pastedRows)
                {
                    string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                    if (!columnsAdded)
                    {
                        for (int i = 0; i < pastedRowCells.Length; i++)
                            myDataGridView.Columns.Add("col" + i, "Column" + i);

                        columnsAdded = true;
                    }

                    myDataGridView.Rows.Add(pastedRowCells);
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.V && e.Control)
            {
                PasteClipboard(dataGridView1);
            }
        }

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            //FtpWebRequest request = FtpWebRequest.Create([FTPAddress] + "/" + filename) as FtpWebRequest;
            //request.Method = WebRequestMethods.Ftp.DownloadFile;
            List<FileAttribute> lstfile = new List<FileAttribute>();

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                lstfile.Add(new FileAttribute() { Folder = ConnectionFTP.link + ((item.Cells[0].Value != null) ? "/" + item.Cells[0].Value : ""), FileName = item.Cells[1].Value + "-" + item.Cells[2].Value + ".R" });
            }

            ConnectionFTP.DownloadFile(ref lstfile, txt_local.Text);
            dataGridView1.DataSource = lstfile;
            MessageBox.Show("Success!!!");
        }
    }
}
