# tcp-socket-chat

# Group Members : 
1. 220180495 - Calvin Kwan
2. 220180496 - Chrisanto Sinatra
3. 220180499 - Enrico Adi
4. 220180517 - Jonathan Bryan
5. 220180521 - Michael Kevin

# Description
This project is a simple messaging application using TCP protocol built with C# Visual Studio. 
At the beginning when the application runs, the program will ask what is the role of the user (either server or client)

![image](https://github.com/EnricoAdi/tcp-socket-chat/assets/107124591/2a48f2f1-629e-401c-9554-b3f3f7c49c4a)


# Server
If user run as a server, this user will connecting other user that works as the client so that each of client could sending message through this one server.
After user clicked start, user can see all the logs (like incoming chat, new client connected, etc) at the console window

![image](https://github.com/EnricoAdi/tcp-socket-chat/assets/107124591/eaf662b8-2c16-46a0-ac2b-aa992b160fac)

# Client
If user run as a client, this is the flow how user can send messages to other user
1.  Enter username and IP of the server

![image](https://github.com/EnricoAdi/tcp-socket-chat/assets/107124591/274a9ad9-9e0b-4a88-8782-aa427b4a91ba)

2. Choose chatting partner
Right here, the program will provide a list of connected users except the user itself (based on username). The list will updated with delay of 2 seconds.

![image](https://github.com/EnricoAdi/tcp-socket-chat/assets/107124591/c2bcfec8-d614-4285-b75c-c286c97f2ae4)

After selecting a partner, click connect to enter the chat room

3. Now user can chat with the partner, and all of the chat will stored at a listbox. Note that the chat showed at the listbox will be ordered by the newest first

![image](https://github.com/EnricoAdi/tcp-socket-chat/assets/107124591/19cc22d7-a532-42a0-ad2c-9174debd0e07)

The chat will also showed at the log of server

![image](https://github.com/EnricoAdi/tcp-socket-chat/assets/107124591/41d97c81-eb1a-4bfd-9191-b9ea0311d648)


# Code Documentation

```
 # For Server  
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);
            var ips = IPHelper.GetInterfaceIPAddress();
            Console.WriteLine(ipHost.HostName + " " + ipAddr.ToString());
 
            Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);
                         
                listener.Bind(localEndPoint);

                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting connection ... ");

                    Socket clientSocket = listener.Accept();

                    string[] pembanding = clientSocket.RemoteEndPoint.ToString().Split(':');
                    int port = Int32.Parse(pembanding[1]);

                    SocketListener obj = new SocketListener(clientSocket, pembanding[0], port);

                    Server.listSocket.Add(obj);

                    Console.WriteLine("---LIST CLIENT---");
                     

                    for (int i = 0; i < Server.listSocket.Count; i++)
                    {
                        SocketListener o = Server.listSocket[i];

                        Console.WriteLine($" ({i + 1}) USERNAME - IP - PORT : {o.username} - {o.lockIp} - {o.port}");
                          
                    }
                    Console.WriteLine("--- END LIST CLIENT---");

                    
                    //send username list
                    Thread tr = new Thread(new ThreadStart(obj.newClient));

                    tr.Start();
                }
```
Right here as can be seen, the server will hold a list of connected client socket. And for each new client connected, the server will create a new thread for that client.

```
#This is the protocol of each thread, which will listening to each client everytime a message is sent

 EndPoint sender = clientSocket.RemoteEndPoint;

                int numByte = clientSocket.Receive(bytes);

                data += Encoding.ASCII.GetString(bytes, 0, numByte);
                 

                string ipsender = ((IPEndPoint)sender).Address.ToString();

                if (data.IndexOf("<EOF>") > -1)
                {
                    string originalMsg = data.Substring(0, data.IndexOf("<EOF>"));

                    string[] arr = originalMsg.Split('|');
                    string action = arr[0];  
                    string usernameSender = arr[1];
                    string msg = arr[2];

                    string usernameReceive = arr[3]; 

                    if (action == "ASK")
                    {
                        string listUsername = "****-";

                        for (int i = 0; i < Server.listSocket.Count; i++)
                        {
                            SocketListener o = Server.listSocket[i];
                            listUsername += $"{o.username}-";
                        }
                        clientSocket.Send(IPHelper.MsgToByte(listUsername)); 
                    }
                    if (action == "SEND")
                    {   
                        Console.WriteLine($"FROM {ipsender} to {usernameReceive} : {msg} ");

                        //send to received ip

                        int idx = -1;
                        for (int i = 0; i < Server.listSocket.Count; i++)
                        {
                            SocketListener o = Server.listSocket[i]; 
                            if (o.username == usernameReceive)
                            {
                                idx = i;
                            }
                        }
                        if (idx != -1)
                        {
                            IPAddress rcvIp = IPAddress.Parse(Server.listSocket[idx].lockIp);
                            IPEndPoint responsetarget = new IPEndPoint(rcvIp, Server.listSocket[idx].port);

                            Server.listSocket[idx].clientSocket.SendTo(IPHelper.MsgToByte($"{usernameSender} :" + msg), responsetarget);
                        }
                        else
                        {
                            clientSocket.Send(IPHelper.MsgToByte("Target not found"));
                        }
                    }

                    if (action == "BYE")
                    {
                        int idx = -1;
                        for (int i = 0; i < Server.listSocket.Count; i++)
                        {
                            SocketListener o = Server.listSocket[i];

                            if (o.lockIp == ipsender)
                            {
                                idx = i;
                            }
                        }
                        Server.listSocket.RemoveAt(idx);
                        break;
                    }

                    if (action == "HELLO")
                    {   
                            int idxCariUsername = -1;
                            for (int i = 0; i < Server.listSocket.Count; i++)
                            {
                                SocketListener o = Server.listSocket[i];

                                if (o.lockIp == ipsender)
                                {
                                    idxCariUsername = i;
                                }
                            }

                            Server.listSocket[idxCariUsername].username = usernameSender;

                            Server.printListSocket(); 
                    }

                    data = null;
                }
```
For the chat mechanism, there is 4 different protocols, such as
1. ```Hello```
This protocol will triggered the server to register the username of connected client
2. ```Ask```
This protocol will send back to the client the list of username connected
3. ```Send```
This protocol will process the incoming message sent from a client to other
4. ```Bye```
This protocol will remove the user from list 

```
# Client's Thread
                    Thread.Sleep(500);
                    byte[] Buffer = new byte[255];
                    int rec = SocketClient.socket.Receive(Buffer, 0, Buffer.Length, 0);
                    Array.Resize(ref Buffer, rec);
                    string msgGet = Encoding.Default.GetString(Buffer);
                    if (msgGet != "")
                    {
                        Console.WriteLine("asdd" + msgGet); 
                        this.Invoke(new Action(() => this.addChat(msgGet)));   
                    } 
```
This code is used to make sure that client is receiving message sent from the server on a thread
this.Invoke is a code to access the form's component from inside a thread

```
 void sendChat(string message)
        { 
            byte[] sdata = Encoding.Default.GetBytes($"SEND|{SocketClient.username}|{message}|{SocketClient.usernameTujuan}<EOF>");
            SocketClient.socket.Send(sdata, 0, sdata.Length, 0);
        }
```
As described in each protocol before, this is the function used on client to send message to the server.
Everytime a client send a message, the client will send a message containing a collection of bytes that starts with SEND, and then separated with "|", after that the username, and then the message itself, and then the partner username, and ended with ```<EOF>``` 
