using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace isiConnect
{
    public partial class FormEnterUsername : Form
    {
        public string username = "";
        public Form1 form1;
        public FormEnterUsername()
        {
            InitializeComponent();

            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                // will save to working directory  ( for C# WPF in VS 2019: C:\Users\{user}\source\repos\{project}\{project}\bin\Debug )
                bitmap.Save("test.jpg", ImageFormat.Jpeg);
            }


            button1.FlatStyle = FlatStyle.Flat;
        }

        public FormEnterUsername(Form1 form1)
        {
            InitializeComponent();


            this.form1 = form1;
        }

        private void FormEnterUsername_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            username = textUsername.Text;
            if (username == "")
            {
                MessageBox.Show("Enter username!");
                return;
            }
            else if(username.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 symbols!");
                return;
            }

            this.form1.Username = this.username;
            this.Close();
        }
    }
}
