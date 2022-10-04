using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace isiConnect
{
    public partial class FormIncomeConnection : Form
    {
        public string Connecting { get; set; }
        public Form1 Form;
        public FormIncomeConnection(Form1 form,string connecting)
        {
            InitializeComponent();
            Form = form;
            Connecting = connecting;
            labelUsername.Text = connecting;




        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            Form.ConnectedUser = Connecting;
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void labelUsername_Click(object sender, EventArgs e)
        {

        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
