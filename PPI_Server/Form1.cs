using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace PPI_Server
{
    public partial class Form1 : Form
    {

        public static bool doChange = false;
        public static string rgb = "";
        public Form1()
        {
            InitializeComponent();
            server.Start();
        }

        public Task server = new Task(async () =>
        {
            IPAddress ipAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
            IPEndPoint ipEndPoint = new(ipAddress, 8080);
            using Socket listener = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);
            listener.Bind(ipEndPoint);
            listener.Listen(100);
            while (true)
            {
                using (var handler = await listener.AcceptAsync())
                {
                    var buffer = new byte[1_024];
                    var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                     rgb = Encoding.UTF8.GetString(buffer, 0, received);
                    doChange = true;
                }
            }
        });

        

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 200;
            timer1.Start();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (doChange) {
                int r = 0, g = 0, b = 0;
                r = Int32.Parse(rgb.Substring(0, rgb.IndexOf(",")));
                rgb = rgb.Remove(0, rgb.IndexOf(",")+1);
                g = Int32.Parse(rgb.Substring(0, rgb.IndexOf(",")));
                rgb = rgb.Remove(0, rgb.IndexOf(",")+1);
                b = Int32.Parse(rgb.Substring(0));
                BackColor = Color.FromArgb(255, r, g, b);
                doChange = false;
                timer1.Start();
            }
        }
    }
}