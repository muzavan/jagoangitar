using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading; 

namespace JagoanGitarBridge
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        private string PortName = "COM2";
        private int BaudRate = 9600;
        private StringBuilder tmpData;
        //private Socket dataSender;
        private Socket socket;
        private List<StateObject> activeClients = new List<StateObject>();

        public class StateObject
        {
            // Client  socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();
        }

        public Form1()
        {
            InitializeComponent();
            tmpData = new StringBuilder();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //serialPort = new SerialPort(PortName, BaudRate);
            //serialPort.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
            //serialPort.Open();
            //System.Console.WriteLine(serialPort.IsOpen);

            //dataSender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //dataSender.Connect("127.0.0.1", 1231);

            //byte[] msg = Encoding.ASCII.GetBytes("0.24\n");
            //dataSender.Send(msg);

            //Thread.Sleep(1000);
            //dataSender.Send(msg);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Loopback, 1231));
            socket.Listen(100);
            socket.BeginAccept(new AsyncCallback(AcceptCallback), socket);
        }

        private void dataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            
            string receivedData = serialPort.ReadExisting();
            tmpData.Append(receivedData);

            if (receivedData.Contains("\n"))
            {
                string strToSend = tmpData.ToString();
                if (!strToSend.Contains("clip"))
                {
                    strToSend = strToSend.Replace("hz", "").Replace("\n", "").Trim() + "\n";

                    Console.WriteLine(strToSend);
                    send(strToSend);
                    //byte[] msg = Encoding.ASCII.GetBytes(strToSend);
                    //dataSender.Send(msg);
                }
                tmpData.Clear();
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            clearSock();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            
            activeClients.Add(state);
            socket.BeginAccept(new AsyncCallback(AcceptCallback), socket);
        }

        private void send(string message)
        {
            foreach (StateObject state in activeClients)
            {
                if (state.workSocket.Connected)
                {
                    byte[] msg = Encoding.ASCII.GetBytes(message);
                    try
                    {
                        state.workSocket.Send(msg);
                    }
                    catch (Exception x)
                    {
                        //activeClients.Remove(state);
                    }
                }
                else
                {
                    //activeClients.Remove(state);
                    Console.WriteLine("TES");
                }
            }
        }

        private void clearSock()
        {
            List<StateObject> disc = new List<StateObject>();
            foreach (StateObject state in activeClients)
            {
                if (!state.workSocket.Connected)
                {
                    disc.Add(state);
                }
            }

            foreach (StateObject dis in disc)
                activeClients.Remove(dis);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            //String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead == 0)
            {
                Console.WriteLine("DISKONEK");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            send("Testing\n");
        }
    }
}
