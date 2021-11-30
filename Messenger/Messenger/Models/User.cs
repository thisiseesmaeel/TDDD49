using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Media;

namespace Messenger.Models
{
    public class User : INotifyPropertyChanged
    {
        public User()
        {
            DisplayName = "";
            _port = "14000";
            _iP = "127.0.0.1";
            Chatpartner = null;
            ChatHistoryList = new List<ChatHistory>();
            ChatHistoryResultList = new List<ChatHistory>();
        }

        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;

        // Relative path in which database going to be created
        private const string DatabasePath = @"..\..\Database\database.json";

        private TcpClient client;

        private bool _connectionEnded;

        public bool ConnectionEnded
        {
            get { return _connectionEnded; }
            set { _connectionEnded = value; OnPropertyChanged("ConnectionEnded"); }
        }

        public string Chatpartner { get; set; }

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


        private string _port;
        public string Port
        {
            get { return _port; }
            set { _port = value; OnPropertyChanged("Port"); }
        }

        private Message _message;

        public Message Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged("Message"); }
        }

        private bool _showInvitationMessageBox;

        public bool ShowInvitationMessageBox
        {
            get { return _showInvitationMessageBox; }
            set { _showInvitationMessageBox = value; OnPropertyChanged("ShowInvitationMessageBox"); }
        }

        private bool _acceptRequest;

        public bool AcceptRequest
        {
            get { return _acceptRequest; }
            set { _acceptRequest = value; }
        }

        private bool _showSocketExceptionMessageBox;

        public bool ShowSocketExceptionMessageBox
        {
            get { return _showSocketExceptionMessageBox; }
            set 
            { 
                _showSocketExceptionMessageBox = value;
                OnPropertyChanged("ShowSocketExceptionMessageBox"); 
            }
        }

        private bool _responseToRequest;

        public bool ResponseToRequest
        {
            get { return _responseToRequest; }
            set { _responseToRequest = value; OnPropertyChanged("ResponseToRequest"); }
        }
        public List <ChatHistory> ChatHistoryList { set; get; }

        private List<ChatHistory> _chatHistoryResultList;
        public List <ChatHistory> ChatHistoryResultList
        {
            get { return _chatHistoryResultList; }
            set { _chatHistoryResultList = value; OnPropertyChanged("ChatHistoryResultList"); }
        }

        private bool _buzz;

        public bool Buzz
        {
            get { return _buzz; }
            set { _buzz = value; OnPropertyChanged("Buzz"); }
        }


        #endregion

        private void OnPropertyChanged(string PropertyName ="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        public void Listen()
        {
            Action action = () =>
            {
                TcpListener server = null;
                try
                {
                    Int32 port = Int32.Parse(Port);
                    IPAddress localAddr = IPAddress.Parse(IP);

                    server = new TcpListener(localAddr, port);

                    server.Start();

                    Byte[] bytes = new Byte[256];
                    String data = null;

                    ConnectionEnded = false;
                    // Enter the listening loop.
                    while (!ConnectionEnded)
                    {
                        client = server.AcceptTcpClient();

                        data = null;
                        NetworkStream stream = client.GetStream();
                        int i;

                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);

                            Message Msg = JsonConvert.DeserializeObject<Message>(data);
                            if (Msg.RequestType == "Establish")
                            {
                                Chatpartner = Msg.Sender;
                                ShowInvitationMessageBox = true;
                                
                                if (AcceptRequest)
                                {
                                    // Send back a response.
                                    Message response = new Message("RequestAccepted", DisplayName, new DateTime(), "");
                                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                                    stream.Write(msg, 0, msg.Length);
                                }
                                else
                                {
                                    Chatpartner = null;
                                    // Send back a response.
                                    Message response = new Message("RequestDenied", DisplayName, new DateTime(), "");
                                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                                    stream.Write(msg, 0, msg.Length);
                                    client.Close();
                                    break;
                                }

                            }
                            else if (Msg.RequestType == "Chat")
                            {
                                Message = Msg;

                                Chat TempChat = new Chat(Msg.MessageText, Msg.Sender);
                                ChatHistory SearchResult = ChatHistoryList.Find(x => x.ChatPartnerName == Chatpartner);
                                if ( SearchResult == null )
                                {
                                    SearchResult = new ChatHistory(DateTime.Now, Chatpartner);
                                    ChatHistoryList.Add(SearchResult);
                                }
                                SearchResult.ChatLog.Add(TempChat);
                                
                            }
                            else if(Msg.RequestType == "EndConnection")
                            {
                                Msg = new Message("Chat", Msg.Sender, new DateTime(), "Left the room.");
                                Message = Msg;
                                ConnectionEnded = true;
                                client.Close();
                                break;
                            }
                            else if(Msg.RequestType == "BUZZ")
                            {
                                // Play a sound and notify Buzz property changed to viewmodel in
                                // order to shake window
                                SystemSounds.Beep.Play();
                                Buzz = true;
                            }
                        }
                    }
                }
                #region Exception
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
                catch(System.IO.IOException e)
                {
                    Console.WriteLine("Connection ended: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    Console.WriteLine("Done listening ...");
                    server.Stop();
                }
                #endregion
            };

            Task.Factory.StartNew(action);
        }

        public void Connect()
        {
            Message Msg = new Message("Establish", DisplayName, new DateTime(), "");
            string message = JsonConvert.SerializeObject(Msg);
            
            Action action = () =>
            {
                try
                {
                    Int32 port = Int32.Parse(Port);
                    client = new TcpClient(IP, port);
                    Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    data = new Byte[256];
                    String ResponseData = String.Empty;
                    Message ResponseObj = null;
                    Int32 bytes;

                    bytes = stream.Read(data, 0, data.Length);
                    ResponseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                    ResponseObj = JsonConvert.DeserializeObject<Message>(ResponseData);

                    if (ResponseObj.RequestType == "RequestDenied")
                    {
                        Chatpartner = null;
                        // Feedback here
                        ResponseToRequest = false;
                        // Close everything.
                        stream.Close();
                        client.Close();
                        ConnectionEnded = true;
                    }
                    else // Request Accepted
                    {
                        Chatpartner = ResponseObj.Sender;
                        // Feedback here
                        ResponseToRequest = true;

                        ConnectionEnded = false;

                        // Read the batch of the TcpServer response bytes.
                        while ((bytes = stream.Read(data, 0, data.Length)) != 0)
                        {
                            ResponseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                            ResponseObj = JsonConvert.DeserializeObject<Message>(ResponseData);
                            if (ResponseObj.RequestType == "Chat")
                            {
                                Message = ResponseObj;

                                Chat TempChat = new Chat(Msg.MessageText, Msg.Sender);
                                ChatHistory SearchResult = ChatHistoryList.Find(x => x.ChatPartnerName == Chatpartner);
                                if (SearchResult == null)
                                {
                                    SearchResult = new ChatHistory(DateTime.Now, Chatpartner);
                                    ChatHistoryList.Add(SearchResult);
                                }
                                SearchResult.ChatLog.Add(TempChat);

                            }
                            else if (ResponseObj.RequestType == "EndConnection")
                            {
                                ConnectionEnded = true;
                                client.Close();
                                ResponseObj = new Message("Chat", ResponseObj.Sender, new DateTime(), "Left the room.");
                                Message = ResponseObj;
                                break;
                            }
                            else if(ResponseObj.RequestType == "BUZZ")
                            {
                                // Play a sound and notify Buzz property changed to viewmodel in
                                // order to shake window
                                SystemSounds.Beep.Play();
                                Buzz = true;
                            }

                        }
                    }   
                }
                #region exception
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                    ShowSocketExceptionMessageBox = true;

                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine("Connection ended: {0}", e);
                }
                #endregion
            };

           Task.Factory.StartNew(action);
        }

        public void Chat(string message)
        {
            try
            {
                Message Msg = new Message("Chat", DisplayName, new DateTime(), message);
                message = JsonConvert.SerializeObject(Msg, Formatting.Indented);

                Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);
                Msg.Sender = "Me";

                Chat TempChat = new Chat(Msg.MessageText, Msg.Sender);
                ChatHistory SearchResult = ChatHistoryList.Find(x => x.ChatPartnerName == Chatpartner);
                if (SearchResult == null)
                {
                    SearchResult = new ChatHistory(DateTime.Now, Chatpartner);
                    ChatHistoryList.Add(SearchResult);
                }
                SearchResult.ChatLog.Add(TempChat);

                Message = Msg;
            }
            #region Exception
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            #endregion
        }

        public void BUZZ()
        {
            try
            {
                Message BuzzMessage = new Message("BUZZ", DisplayName, DateTime.Now, "");
                string message = JsonConvert.SerializeObject(BuzzMessage);

                Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);
            }
            #region Exception
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            #endregion
        }

        public void TearDownConnection()
        {
            try
            {
                if(client !=null && client.Connected)
                {
                    Message Msg = new Message("EndConnection", DisplayName, new DateTime(), "");
                    Byte[] data = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Msg));
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);

                    // Do we need this line?
                    stream.Close();
                    //client.Close();
                }
                SaveHistory();
                ConnectionEnded = true;
            }
            #region Exception
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            #endregion
        }

        public void SaveHistory()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = Newtonsoft.Json.JsonConvert.SerializeObject(ChatHistoryList);
                writer = new StreamWriter(DatabasePath, false);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }

        public void LoadHistory()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(DatabasePath);
                var fileContents = reader.ReadToEnd();
                ChatHistoryList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChatHistory>>(fileContents);
                ChatHistoryList = ChatHistoryList.OrderByDescending(x => x.Date).ToList();
                ChatHistoryResultList = ChatHistoryList;
            }
            catch(System.IO.FileNotFoundException e)
            {
                Console.WriteLine("Database does not exist", e);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public void Search(string query)
        {
            // Use linq
            ChatHistoryResultList = (from chatHistory in ChatHistoryList
                                        where chatHistory.ChatPartnerName.ToUpper().Contains(query.ToUpper())
                                        select chatHistory).ToList();

            

            /*
            App.Current.Dispatcher.Invoke(() => {


            });*/
        }
    }
}
