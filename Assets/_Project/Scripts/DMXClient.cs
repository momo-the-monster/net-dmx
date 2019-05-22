using UnityEngine;
using UnityEngine.Networking;

namespace MMM.DMX
{
    public class DMXClient : MonoBehaviour
    {

        [SerializeField] private string address = "localhost";
        [SerializeField] private int port = 4649;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
                SendColor(color);
                Camera.main.backgroundColor = color;
            }

            if (Input.GetMouseButtonUp(0))
            {
                var color = Color.clear;
                SendColor(color);
                Camera.main.backgroundColor = color;
            }
        }

        public void SendColor(Color32 color)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(GetQueryForColor(color));
            webRequest.SendWebRequest();
        }

        public string GetQueryForColor(Color32 color)
        {
            return $"http://{address}:{port}/dmx?r={color.r}&g={color.g}&b={color.b}&w={color.a}";
        }

    }

}