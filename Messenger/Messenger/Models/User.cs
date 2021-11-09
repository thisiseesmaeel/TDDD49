using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;

namespace Messenger.Models
{
    public class User : INotifyPropertyChanged
    {
        public User()
        {}

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string PropertyName ="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        private String _displayName;

        public String DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; OnPropertyChanged("DisplayName"); }
        }

        private String _iP;
        public String IP
        {
            get { return _iP; }
            set { _iP = value; OnPropertyChanged("IP"); }
        }


        private int _port;
        public int Port
        {
            get { return _port; }
            set { _port = value; OnPropertyChanged("Port"); }
        }

        
        private String _message;

        public String Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged("Message"); }
        }

        public void Listen()
        {
            Action action = () =>
            {
                TcpListener server = null;
                try
                {
                    // Set the TcpListener on port 13000.
                    Int32 port = Port;
                    IPAddress localAddr = IPAddress.Parse(IP);

                    // TcpListener server = new TcpListener(port);
                    server = new TcpListener(localAddr, port);

                    // Start listening for client requests.
                    server.Start();

                    // Buffer for reading data
                    Byte[] bytes = new Byte[256];
                    String data = null;

                    // Enter the listening loop.
                    while (true)
                    {
                        Console.Write("Waiting for a connection... ");

                        // Perform a blocking call to accept requests.
                        // You could also use server.AcceptSocket() here.
                        TcpClient client = server.AcceptTcpClient();
                        Console.WriteLine("Connected!");

                        data = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i;

                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            Console.WriteLine("Received: {0}", data);

                            Message Msg = JsonConvert.DeserializeObject<Message>(data);
                            
                            // Process the data sent by the client.
                            data = data.ToUpper();

                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Sent: {0}", data);
                        }

                        // Shutdown and end connection
                        client.Close();
                    }
                    
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("Insert a valid IP: {0}", e);
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Insert a valid IP: {0}", e);
                }

                finally
                {                    
                    // Stop listening for new clients.
                    server.Stop();
                }
     

                Console.WriteLine("\nHit enter to continue...");
                Console.Read();
            };

            Task.Factory.StartNew(action);
        }

        public void Connect(String message = "Hello World!", String server = "127.0.0.1")
        {
            Message initializeMsg = new Message("Establish", "Jack", new DateTime(), "");
            message = JsonConvert.SerializeObject(initializeMsg);
            
            Action action = () =>
            {
                try
                {
                    // Create a TcpClient.
                    // Note, for this client to work you need to have a TcpServer
                    // connected to the same address as specified by the server, port
                    // combination.
                    Int32 port = Port;
                    TcpClient client = new TcpClient(server, port);

                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                    // Get a client stream for reading and writing.
                    //  Stream stream = client.GetStream();

                    NetworkStream stream = client.GetStream();

                    // Send the message to the connected TcpServer.
                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", message);

                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    this.Message = responseData;
                    Console.WriteLine("Received: {0}", responseData);

                    // Close everything.
                    stream.Close();
                    client.Close();
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
            };

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();

            Task.Factory.StartNew(action);
        }
    }
}
