﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TCPChatRoom
{
    public class Client : IObserver<TcpClient>
    {
        private IDisposable unsubscriber;
        private TcpClient client;
        private NetworkStream stream;
        private byte[] data;
        public Client()
        {
        }
        public void Subscribe(IObservable<TcpClient> server)
        {
            unsubscriber = server.Subscribe(this);
        }
        public void CreateClient(string message, string ipAddress, int portNumber)
        {
            try
            {
                 client = new TcpClient(ipAddress, portNumber);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}" , e);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            Thread newThread = new Thread(SendCommunication);
            newThread.Start();
            //SendCommunication();
        }
        public void SendCommunication()
        {//thread for sending
            while (true)
            {
                byte[] data = Encoding.ASCII.GetBytes(Console.ReadLine());
                 stream =  client.GetStream();

                stream.Write(data, 0, data.Length);
                Thread newThread = new Thread(RecieveCommunication);
                newThread.Start();
            }
        }
        public void RecieveCommunication()
        {//thread for recieving 
            data = new byte[256];

            string responseData = string.Empty;
            
            int bytes = stream.Read(data, 0, data.Length);
            responseData = Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
        }
    }
}
