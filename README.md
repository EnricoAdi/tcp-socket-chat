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
 

