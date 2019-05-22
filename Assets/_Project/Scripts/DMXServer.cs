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
        private string queuedName;

        private void OnEnable()
        {
            server = new HttpServer(port);
            server.OnPost += OnServerGet;
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
                OnColorReceived(Color.clear, GetNameFromQuery(req.QueryString));
                res.Close(WebSocketSharp.Net.HttpStatusCode.Accepted);
            }
        }

        private string GetNameFromQuery(NameValueCollection query)
        {
            return query.Contains("name") ? query["name"] : DMXManager.DefaultGroupName;
        }

        private void SetColorFromQuery(NameValueCollection query, WebSocketSharp.Net.HttpListenerResponse res)
        {
            if (query.Contains("r") && query.Contains("g") && query.Contains("b") && query.Contains("w"))
            {
                Color color = new Color32(byte.Parse(query["r"]), byte.Parse(query["g"]), byte.Parse(query["b"]), byte.Parse(query["w"]));
                OnColorReceived(color, GetNameFromQuery(query));

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
                dmx.SendColor(queuedColor, queuedName);
                sendColor = false;
            }
        }

        private void OnColorReceived(Color color, string name)
        {
            queuedName = name;
            queuedColor = color;
            sendColor = true;
        }

        private void OnDisable()
        {
            server.Stop();
        }
    }

}