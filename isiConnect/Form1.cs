using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Threading;
using System.Net.NetworkInformation;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace isiConnect
{
    public partial class Form1 : Form
    {

        TcpClient tcpclnt = new TcpClient();

        //Declare and Initialize the IP Adress
        IPAddress ipAd = IPAddress.Parse("127.0.0.1");

        public string ConnectedUser { get; set; } = "";

        public string ConnectedToUser { get; set; } = "";


        //Declare and Initilize the Port Number;
        int PortNumber = 8888;

        public string Username
        {
            get { return labelUsername.Text; }
            set { labelUsername.Text = value; }
                
        }
        public Form1()
        {
            InitializeComponent();

            Username = "iskender";


            // Status circle
            using (var gp = new GraphicsPath())
            {
                gp.AddEllipse(0, 0, pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Location = new Point(labelUsername.Location.X + labelUsername.Width + 5, labelUsername.Location.Y + (labelUsername.Height / 4));
                Region rg = new Region(gp);
                pictureBox2.Region = rg;
                pictureBox2.BackColor = Color.Red;
            }


            Thread thr = new Thread(fetchClient);
            thr.Start();

            // Get PING

            //var times = new List<double>();
            //for (int i = 0; i < 4; i++)
            //{
            //    var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    sock.Blocking = true;

            //    var stopwatch = new Stopwatch();

            //    // Measure the Connect call only
            //    stopwatch.Start();
            //    sock.Connect(IPAddress.Parse("8.8.8.8"), 80);
            //    stopwatch.Stop();

            //    double t = stopwatch.Elapsed.TotalMilliseconds;
            //    textLogs.Text += ("{0:0.00}ms", t);
            //    times.Add(t);

            //    sock.Close();

            //    Thread.Sleep(1000);
            //}
            //textLogs.Text += ("{0:0.00} {1:0.00} {2:0.00}", times.Min(), times.Max(), times.Average());



            //Ping x = new Ping();
            //PingReply reply = x.Send("127.0.0.1:8888");


            //MessageBox.Show(reply.RoundtripTime.ToString() + reply.Status.ToString());


            //  this.Visible = false;
            //FormEnterUsername win = new FormEnterUsername(this);
            //win.ShowDialog();
            //connectToServer();

            btnConnect.FlatStyle = FlatStyle.Flat;
            btnSend.FlatStyle = FlatStyle.Flat;
            textUsername.BorderStyle = BorderStyle.FixedSingle;

           
        }


        private void fetchClient()
        {
            string userConnecting = "";
            while (true)
            {
                if (!tcpclnt.Connected)
                {
                    try
                    {
                        tcpclnt.Connect(ipAd, PortNumber);
                        changeStatus(tcpclnt.Connected);
                        textLogs.Text += "\nConnected";
                        sendText(Username + "$username");
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

               
                Stream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = new byte[70000];

                stm.Read(ba, 0, 70000);

                string dataFromClient = System.Text.Encoding.ASCII.GetString(ba);

                if (dataFromClient.Contains("$ConnectingUser"))
                {
                    userConnecting = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    FormIncomeConnection form = new FormIncomeConnection(this, userConnecting);
                    form.ShowDialog();
                    Stream stmm = tcpclnt.GetStream();
                    ASCIIEncoding asenn = new ASCIIEncoding();
                    if (ConnectedUser == "")
                    {
                       

                        
                        byte[] baa = asenn.GetBytes("$reject");
                        stmm.Write(baa, 0, baa.Length);
                    }
                    else
                    {

                      
                        byte[] baa = asenn.GetBytes("$accept");
                        stmm.Write(baa, 0, baa.Length);
                    }


                }

                if(ConnectedUser != "")
                {

                    Rectangle bounds = Screen.GetBounds(Point.Empty);
                    Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
                    Graphics g = Graphics.FromImage(bitmap);

                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);

                    bitmap = new Bitmap(bitmap, new Size(960, 640));

                    MemoryStream stream = new MemoryStream();
                    try
                    {
                        bitmap.Save(stream, ImageFormat.Jpeg);
                    }
                    catch (Exception ex)
                    {
                    }

                    byte[] bit = stream.ToArray();

                    stm.Write(bit, 0, bit.Length);
                }




            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void connectToServer()
        {
            try
            {
                tcpclnt.Connect(ipAd, PortNumber);
                changeStatus(tcpclnt.Connected);
                textLogs.Text += "\nConnected";
                sendText(Username + "$username");
            }
            catch (Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //sendText(textMessage.Text + "$");
            //sendPicture();

            Thread th = new Thread(sendPicture);
            th.Start();
        }

        public void changeStatus(bool status)
        {
            if(status)
            {
                pictureBox2.BackColor = Color.SeaGreen;
            }
            else
            {
                pictureBox2.BackColor = Color.Red;
            }

            
        }
        public void sendPicture()
        {
            while (true)
            {

                Stream stm = tcpclnt.GetStream();

                Rectangle bounds = Screen.GetBounds(Point.Empty);
                Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
                Graphics g = Graphics.FromImage(bitmap);

                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);

                bitmap = new Bitmap(bitmap, new Size(960, 640));

                MemoryStream stream = new MemoryStream();
                try
                {
                    bitmap.Save(stream, ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                }

                byte[] bit = stream.ToArray();

                stm.Write(bit, 0, bit.Length);
            }
        }

        public void sendText(string text)
        {
            String str = text;
            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);

            stm.Write(ba, 0, ba.Length);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string user = textUsername.Text;
            if(user == "" || user.Length < 3)
            {
                MessageBox.Show("Please enter username correctly!");
                return;
            }
            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(user + "$ConnectingToUser");

            stm.Write(ba, 0, ba.Length);


            byte[] bit = new byte[20];

            stm.Read(bit, 0, 20);
            string dataFromClient = System.Text.Encoding.ASCII.GetString(bit);

            if (dataFromClient.Contains("false"))
            {
                MessageBox.Show("Can't connect to a user!");
            }
            else if(dataFromClient.Contains("reject"))
            {
                MessageBox.Show("User rejected your request!");
            }
            else
            {

            }

        }
    }
}
