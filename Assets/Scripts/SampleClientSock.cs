// A C# program for Client 
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using UnityEngine;

public class SampleClientSock : MonoBehaviour
{

    volatile bool keepReading = false;
    System.Threading.Thread SocketThread;
    private Socket sender;

    private void Start()
    {
        startClient();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMsgToServer("Test Client");
        }

    }

    void SendMsgToServer(string msg)
    {
        if (sender != null && sender.Connected)
        {
            Debug.Log("SendMsgToServer");
            byte[] messageSent = Encoding.ASCII.GetBytes(string.Format("{0}", msg));
            int byteSent = sender.Send(messageSent);
        }
        else
        {
            Debug.LogFormat("Not able to send: {0}", sender.Connected);
        }
    }

    void OnReceivedMsgFromServer(string msg)
    {
        Debug.LogFormat("Message from Server -> {0}", msg);
    }

    void OnDisable()
    {
        stopClient();
    }

    void startClient()
    {
        SocketThread = new System.Threading.Thread(ExecuteClient);
        SocketThread.IsBackground = true;
        SocketThread.Start();
    }

    void stopClient()
    {
        keepReading = false;

        //stop thread
        if (SocketThread != null)
        {
            SocketThread.Abort();
        }

        sender?.Shutdown(SocketShutdown.Both);
        sender?.Close();
    }

    private string getIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
            }

        }
        return localIP;
    }

    // ExecuteClient() Method 
    void ExecuteClient()
    {

        try
        {
            keepReading = true;
            // Establish the remote endpoint 
            // for the socket. This example 
            // uses port 11111 on the local 
            // computer. 
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] ipArray = Dns.GetHostAddresses(getIPAddress());
            IPEndPoint localEndPoint = new IPEndPoint(ipArray[0], 1755);

            // Creation TCP/IP Socket using 
            // Socket Class Costructor 
            sender = new Socket(ipArray[0].AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Connect Socket to the remote 
                // endpoint using method Connect() 
                sender.Connect(localEndPoint);

                // We print EndPoint information 
                // that we are connected 
                Debug.LogFormat("Socket connected to -> {0} ",
                            sender.RemoteEndPoint.ToString());

                while (keepReading)
                {
                    // Creation of messagge that 
                    // we will send to Server 
                    //byte[] messageSent = Encoding.ASCII.GetBytes("Test Client<EOF>"); 
                    //int byteSent = sender.Send(messageSent); 

                    // Data buffer 
                    byte[] messageReceived = new byte[1024];

                    // We receive the messagge using 
                    // the method Receive(). This 
                    // method returns number of bytes 
                    // received, that we'll use to 
                    // convert them to string 
                    int byteRecv = sender.Receive(messageReceived);
                    OnReceivedMsgFromServer(Encoding.ASCII.GetString(messageReceived, 0, byteRecv));

                    System.Threading.Thread.Sleep(1);
                }


                // Close Socket using 
                // the method Close() 
                //sender.Shutdown(SocketShutdown.Both); 
                //sender.Close(); 
            }

            // Manage of Socket's Exceptions 
            catch (ArgumentNullException ane)
            {

                Debug.LogFormat("ArgumentNullException : {0}", ane.ToString());
            }

            catch (SocketException se)
            {

                Debug.LogFormat("SocketException : {0}", se.ToString());
            }

            catch (Exception e)
            {
                Debug.LogFormat("Unexpected exception : {0}", e.ToString());
            }
        }

        catch (Exception e)
        {

            Debug.LogFormat(e.ToString());
        }
    }
}