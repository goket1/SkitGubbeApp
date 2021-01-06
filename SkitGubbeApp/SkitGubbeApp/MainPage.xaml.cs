using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkitGubbeApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // Call the websocket test method
            StartWebSocketConnection();
        }

        public async void StartWebSocketConnection()
        {
            #region Initialize and connect
            // Create the websocket object
            ClientWebSocket ws = new ClientWebSocket();

            // Connect to websocket endpoint
            await ws.ConnectAsync(new Uri("ws://193.106.164.115/CardGameServer-17570996071548594830.0-SNAPSHOT/ws"),
                CancellationToken.None);
            #endregion


            #region Send
            // Create the data to send
            var data = "Hello from skit gubbe app";
            // UTF-8 Encode the data to send
            var encoded = Encoding.UTF8.GetBytes(data);
            // Put the data to send in a buffer
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            // Debug the data to send
            Debug.WriteLine($"Sending data: {data}");
            // Send the UTF-8 encoded buffer over the websocket
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            #endregion


            #region Receive
            // Create a buffer to hold the data read
            ArraySegment<Byte> readBuffer = new ArraySegment<byte>(new Byte[8192]);
            // Continue receiving data while the web socket is open
            while (ws.State == WebSocketState.Open)
            {
                // Run the receive data code in a try block to catch a socket exception when the socket is closed e.g. when the server closes the connection
                try
                {
                    // Await and read data from the websocket into the readbuffer
                    var result = await ws.ReceiveAsync(readBuffer, CancellationToken.None);
                    // Get string from the buffer
                    var str = System.Text.Encoding.Default.GetString(readBuffer.Array, readBuffer.Offset, result.Count);
                    // Debug write the data buffer received
                    Debug.WriteLine($"Data received: {str}");
                }
                catch (TaskCanceledException)
                {
                    // Debug write when a socket exeption is caught and the websocket is closed
                    Debug.WriteLine("Websocket closed");
                }
            }
            #endregion

            // TODO Move to it's own class
            // TODO JSON Serialization
            // TODO Implement IDispose and dispose of the websocket in a prober way
            // TODO Prober UI Callback or smth like that

            //TestComment
        }
    }
}
