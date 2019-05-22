using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace MMM.DMX
{
    public class DMXClient : MonoBehaviour
    {

        private WebSocket client;
        [SerializeField] private string address = "localhost";
        [SerializeField] private int port = 4649;

        private void OnEnable()
        {
            string url = $"ws://{address}:{port}/DMX";
            client = new WebSocket(url);

            client.OnOpen += (sender, e) =>
            {
                Debug.Log("Client Opened Connection");
            };

            client.OnMessage += OnMessage;

            client.OnError += (sender, e) =>
            {
                Debug.LogError($"Client got error {e.Message}");
            };

            client.OnClose += (sender, e) =>
            {
                Debug.Log("Client Closed Connection");
            };

            Invoke("Connect", 1);
        }

        private void Connect()
        {
            client.Connect();
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            Debug.Log($"Client got message {e.Data}");
        }

        private void OnDisable()
        {
            if (client.IsConnected) client.Close();
        }

        [SerializeField] private Color onColor;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                client.Send(JsonUtility.ToJson(onColor));
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                client.Send(JsonUtility.ToJson(Color.clear));
            }
        }
    }

}