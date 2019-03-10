using System;
using System.Collections.Specialized;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MMM.DMX
{
    public class DMXServer : MonoBehaviour
    {

        [SerializeField] private DMXManager dmx;
        [SerializeField] private int port = 8000;
        private WebSocketServer socketServer;
        private HttpServer server;
        private bool sendColor = false;
        private Color queuedColor;

        private void OnEnable()
        {
            server = new HttpServer(port);
            server.AddWebSocketService<DMXBehavior>("/DMX", () =>new DMXBehavior(OnColorReceived));
            server.OnGet += OnServerGet;
            server.Start();
        }

        private void OnServerGet(object sender, HttpRequestEventArgs e)
        {
            var req = e.Request;
            var res = e.Response;
            var path = req.RawUrl;
            var endPoint = path.Contains("?") ? path.Substring(0, path.IndexOf('?')) : path;

            if(endPoint.CompareTo("/dmx") == 0)
            {
                SetColorFromQuery(req.QueryString, res);
            }

            if(endPoint.CompareTo("/off") == 0)
            {
                OnColorReceived(Color.clear);
                res.Close(WebSocketSharp.Net.HttpStatusCode.Accepted);
            }
        }

        private void SetColorFromQuery(NameValueCollection query, WebSocketSharp.Net.HttpListenerResponse res)
        {
            if (query.Contains("r") && query.Contains("g") && query.Contains("b") && query.Contains("w"))
            {
                Color color = new Color32(byte.Parse(query["r"]), byte.Parse(query["g"]), byte.Parse(query["b"]), byte.Parse(query["w"]));
                OnColorReceived(color);

                res.StatusCode = 200;
                res.WriteContent(System.Text.Encoding.UTF8.GetBytes($"Set Color to {color.ToString()}"));

                res.Close();
            }
            else
            {
                res.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            }
        }

        // check if color was updated in background thread
        private void FixedUpdate()
        {
            if (sendColor)
            {
                dmx.SendColor(queuedColor);
                sendColor = false;
            }
        }

        private void OnColorReceived(Color color)
        {
            queuedColor = color;
            sendColor = true;
        }

        private void OnDisable()
        {
            server.Stop();
        }
    }

    public class DMXBehavior: WebSocketBehavior
    {
        public Action<Color> ColorReceived;

        public DMXBehavior() : this(null) { }
        
        public DMXBehavior (Action<Color> _colorReceived)
        {
            ColorReceived = _colorReceived;
        }
        
        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var color = JsonUtility.FromJson<Color>(e.Data);
            ColorReceived(color);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Debug.Log($"OnClose: {e}");
        }

        protected override void OnError(ErrorEventArgs e)
        {
            Debug.LogError($"Error: {e.Exception.ToString()} // {e.Message}");
        }
    }

}