using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace ConnectFour
{
    public class ServerCommunication : MonoBehaviour
    {
        private TcpClient client;
        private NetworkStream stream;

        public void ConnectToServer(string ip, int port)
        {
            try
            {
                client = new TcpClient(ip, port);
                stream = client.GetStream();
                Debug.Log("Connected to server");
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting to server: " + e.Message);
            }
        }

        public void SendMove(int column, int player)
        {
            if (client == null || !client.Connected) return;

            string message = $"{player},{column}";
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log("Move sent: " + message);
        }

        public IEnumerator ReceiveMove(Action<int, int> onMoveReceived)
        {
            if (client == null || !client.Connected) yield break;

            byte[] data = new byte[256];
            int bytes = stream.Read(data, 0, data.Length);
            string response = Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log("Move received: " + response);

            string[] parts = response.Split(',');
            if (parts.Length == 2)
            {
                int player = int.Parse(parts[0]);
                int column = int.Parse(parts[1]);
                onMoveReceived(player, column);
            }

            yield return null;
        }

        private void OnApplicationQuit()
        {
            stream?.Close();
            client?.Close();
        }
    }
}
